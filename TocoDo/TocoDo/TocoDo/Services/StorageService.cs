using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TocoDo.Models;
using TocoDo.Resources;
using TocoDo.ViewModels;
using Xamarin.Forms.Internals;

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

		#region Public Init + CRUD
		/// <summary>
		/// Initialize the database.
		/// </summary>
		public static async Task Init()
		{
			Debug.WriteLine("Init method of StorageService called.");
			if (_connection != null) return;

			Debug.WriteLine("Creating a new database connection.");
			_connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.Current.LocalStorage.Path, AppStrings.DatabaseName));
			Debug.WriteLine("Created a new database connection.");

			try
			{
				//_connection.Table<TaskModel>().ToListAsync().Result.ForEach(async t => await _connection.DeleteAsync(t));
				////Debug.WriteLine("Creating a new database table.");
				//await Task.Delay(3000);
				//await _connection.DropTableAsync<TaskModel>();
				//Debug.WriteLine("Before Create");
				
				await _connection.CreateTableAsync<TaskModel>();
				await LoadTasks();
			}
			catch (Exception e)
			{

			}
		}
		public static async Task<TaskViewModel> InsertTask(string title, DateTime? scheduleDate)
		{
			var newTask = new TaskModel
			{
				Title = title,
				ScheduleDate = DateTime.Today,
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
			GetListForDate(task.ScheduleDate).Add(task);
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
			await _connection.InsertAsync(taskModel);

		}
	}
}
