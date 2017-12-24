using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TocoDo.Models;
using TocoDo.ViewModels;

namespace TocoDo.Services
{
	public class StorageService
	{
		private static List<ChallengeModel> _fakeChallenges = new List<ChallengeModel>
		{
			new ChallengeModel
			{
				ChallengeType = ChallengeType.Daylong,
				DaysCount = 12,
			}
		};

		/// <summary>
		/// Dont add tasks to this list, call InsertTask to insert it also to the database.
		/// </summary>
		public static ObservableCollection<TaskViewModel> AllTasks { get; } = new ObservableCollection<TaskViewModel>();

		public static ObservableCollection<TaskViewModel> TodayTasks { get; } = new ObservableCollection<TaskViewModel>();
		public static ObservableCollection<TaskViewModel> SomedayTasks { get; } = new ObservableCollection<TaskViewModel>();
		public static ObservableCollection<TaskViewModel> ScheduledTasks { get; } = new ObservableCollection<TaskViewModel>();

		private static SQLiteAsyncConnection _connection;

		#region Init + CRUD
		/// <summary>
		/// Initialize the database.
		/// </summary>
		public static async Task Init()
		{
			Debug.WriteLine("---------- Init method of StorageService called.");
			if (_connection != null) return;

			try
			{
				Debug.WriteLine("---------- Before getting path.");
				//string path = Path.Combine(PathServiceProvider.GetPath(), "MySQLite.db3");//AppStrings.DatabaseName);
				string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MySQLite.db3");//AppStrings.DatabaseName);
																																		//path = System.IO.Path.GetFullPath("Mysql");
				Debug.WriteLine($"---------------- Creating a new database connection on path: \"{path}\"");
				_connection = new SQLiteAsyncConnection(path);
				Debug.WriteLine("Created a new database connection.");


				//_connection.Table<TaskModel>().ToListAsync().Result.ForEach(async t => await _connection.DeleteAsync(t));
				////Debug.WriteLine("Creating a new database table.");
				//await Task.Delay(3000);
				//await _connection.DropTableAsync<TaskModel>();
				//Debug.WriteLine("Before Create");

				await _connection.CreateTableAsync<TaskModel>();
			}
			catch (Exception e)
			{
				Debug.Write("------------- Exception thrown while setting up the database: " + e.Message);
				throw;
			}

			try
			{
				await LoadTasks();
			}
			catch (Exception e)
			{
				Debug.Write($"-------------- Exception of type {e.GetType()} thrown while Loading tasks: \"{e.Message}\" -- with following stack trace: ");
				Debug.Write(e.StackTrace);
				throw;
			}
			Debug.WriteLine("---------- Finished call of Init method of StorageService.");
		}
		public static async Task<TaskViewModel> InsertTask(string title, DateTime? scheduleDate)
		{
			var newTask = new TaskModel
			{
				Title = title,
				ScheduleDate = scheduleDate,
				CreateTime = DateTime.Now
			};

			await InsertTask(newTask);

			var viewModel = new TaskViewModel(newTask);
			AddTaskToTheList(viewModel);
			BindTask(viewModel);
			return viewModel;
		}

		public static async Task UpdateTask(TaskViewModel task)
		{
			await _connection.UpdateAsync(task.GetTaskModel());
		}

		public static async Task DeleteTask(TaskViewModel task)
		{
			await _connection.DeleteAsync(task.GetTaskModel());
			RemoveTaskFromTheList(task);
			UnbindTask(task);
		}

		public static async Task<Dictionary<DateTime, List<TaskViewModel>>> GetUpcommingTasks()
		{
			Dictionary<DateTime, List<TaskViewModel>> upcomming = new Dictionary<DateTime, List<TaskViewModel>>();

			(await _connection.Table<TaskModel>().Where(t => t.Deadline != null && t.Deadline != DateTime.Today).ToListAsync()).ForEach(t =>
			{
				if (t.ScheduleDate != null && t.ScheduleDate.Value.Date >= DateTime.Today + TimeSpan.FromDays(1))
				{
					if (upcomming.ContainsKey(t.ScheduleDate.Value.Date))
					{
						upcomming[t.ScheduleDate.Value.Date].Add(new TaskViewModel(t));
					}
					else
					{
						upcomming[t.ScheduleDate.Value.Date] = new List<TaskViewModel> { new TaskViewModel(t) };
					}
				}
			});

			return upcomming;
		}
		#endregion

		private static async Task LoadTasks()
		{
			var result = await _connection.QueryAsync<TaskModel>(
				"SELECT * FROM TaskModel WHERE ScheduleDate IS NULL OR ScheduleDate >= '" + DateTime.Today.Ticks + "'");

			result.ForEach(t =>
			{
				var task = new TaskViewModel(t);
				AddTaskToTheList(task);
				BindTask(task);
			});
		}

		private static void AddTaskToTheList(TaskViewModel task)
		{
			Debug.Write("------------- Adding task to the list");
			Debug.Write("------------- Id of the task is: " + task.Id);

			GetListForDate(task.ScheduleDate).Add(task);
			Debug.Write("------------- Finished adding task to the list");
		}

		private static void BindTask(TaskViewModel task)
		{
			task.OnScheduleDateChanging += Task_OnScheduleDateChanging;
			task.OnScheduleDateChanged += TaskOnOnScheduleDateChanged;
		}

		private static void Task_OnScheduleDateChanging(TaskViewModel taskViewModel, DateTime? oldDateTime, DateTime? newDateTime)
		{
			RemoveTaskFromTheList(taskViewModel);
		}

		private static void TaskOnOnScheduleDateChanged(TaskViewModel taskViewModel, DateTime? oldDateTime, DateTime? newDateTime)
		{
			AddTaskToTheList(taskViewModel);
		}

		private static ObservableCollection<TaskViewModel> GetListForDate(DateTime? date)
		{
			date = date?.Date;

			if (date == null)
			{
				return SomedayTasks;
			}

			if (date == DateTime.Today)
			{
				return TodayTasks;
			}

			return ScheduledTasks;
		}

		private static void RemoveTaskFromTheList(TaskViewModel task)
		{
			GetListForDate(task.ScheduleDate).Remove(task);
		}

		private static void UnbindTask(TaskViewModel task)
		{
			task.OnScheduleDateChanging -= Task_OnScheduleDateChanging;
			task.OnScheduleDateChanged -= TaskOnOnScheduleDateChanged;
		}

		private static async Task InsertTask(TaskModel taskModel)
		{
			Debug.Write("------------- Inserting task");
			try
			{
				await _connection.InsertAsync(taskModel);
			}
			catch (Exception e)
			{
				Debug.Write("----------- Exception thrown during inserting a task: " + e.Message);
				throw;
			}
			Debug.Write("------------- Finished inserting task");
		}
	}
}