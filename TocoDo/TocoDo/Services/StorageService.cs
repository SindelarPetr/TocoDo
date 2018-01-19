using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TocoDo.Helpers;
using TocoDo.Models;
using TocoDo.ViewModels;

namespace TocoDo.Services
{
	public static class StorageService
	{
		#region Collections
		#region Tasks
		/// <summary>
		/// Dont add tasks to this list, call InsertTask to insert it also to the database.
		/// </summary>
		public static ObservableCollection<TaskViewModel> AllTasks { get; } = new ObservableCollection<TaskViewModel>();

		public static ObservableCollection<TaskViewModel> TodayTasks { get; } = new ObservableCollection<TaskViewModel>();
		public static ObservableCollection<TaskViewModel> SomedayTasks { get; } = new ObservableCollection<TaskViewModel>();
		public static ObservableCollection<TaskViewModel> ScheduledTasks { get; } = new ObservableCollection<TaskViewModel>();
		#endregion

		#region Habits
		public static ObservableCollection<HabitViewModel> AllHabits { get; } = new ObservableCollection<HabitViewModel>();

		public static ObservableCollection<HabitViewModel> CurrentHabits { get; } = new ObservableCollection<HabitViewModel>();
		public static ObservableCollection<HabitViewModel> ScheduledHabits { get; } = new ObservableCollection<HabitViewModel>();
		public static ObservableCollection<HabitViewModel> UnscheduledHabits { get; } = new ObservableCollection<HabitViewModel>();
		public static ObservableCollection<HabitViewModel> RecommendedHabits { get; } = new ObservableCollection<HabitViewModel>();
		#endregion
		#endregion

		private static SQLiteAsyncConnection _connection;

		#region Set up connection
		/// <summary>
		/// Initialize the database.
		/// </summary>
		public static async Task Init()
		{
			Debug.WriteLine("---------- Init method of StorageService called.");
			if (_connection != null) return;

			await InitDb();
			await LoadFromDb();

			Debug.WriteLine("---------- Finished call of Init method of StorageService.");
		}

		private static async Task InitDb()
		{
			try
			{
				Debug.WriteLine("---------- Before getting path.");
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MySQLite.db3");

				Debug.WriteLine($"---------------- Creating a new database connection on path: \"{path}\"");
				_connection = new SQLiteAsyncConnection(path);
				Debug.WriteLine("Created a new database connection.");


				await InitTasks();
				await InitHabits();
			}
			catch (Exception e)
			{
				Debug.Write("------------- Exception thrown while setting up the database: " + e.Message);
				throw;
			}
		}

		private static async Task LoadFromDb()
		{
			try
			{
				await LoadTasks();
				await LoadHabits();
			}
			catch (Exception e)
			{
				Debug.Write($"-------------- Exception of type {e.GetType()} thrown while Loading tasks: \"{e.Message}\" -- with following stack trace: ");
				Debug.Write(e.StackTrace);
				throw;
			}
		}
		#endregion

		#region Tasks
		private static async Task InitTasks()
		{
			//_connection.Table<TaskModel>().ToListAsync().Result.ForEach(async t => await _connection.DeleteAsync(t));
			////Debug.WriteLine("Creating a new database table.");
			//await Task.Delay(3000);
			//await _connection.DropTableAsync<TaskModel>();
			//Debug.WriteLine("Before Create");

			await _connection.CreateTableAsync<TaskModel>();
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
		#endregion

		#region Habits
		static async Task InitHabits()
		{
			//await _connection.CreateTableAsync<HabitModel>();
		}

		public static HabitViewModel GetExampleHabitViewModel()
		{
			return new HabitViewModel(new HabitModel
			{
				DailyFillingCount = 3,
				Title = "Meditation",
				Description = "Sit somewhere where its quiet and calm down all thoughts for 20 minutes.",
				Filling = new Dictionary<DateTime, int>
				{
					{ DateTime.Today - TimeSpan.FromDays(4), 2 },
					{ DateTime.Today - TimeSpan.FromDays(3), 3 },
					{ DateTime.Today - TimeSpan.FromDays(2), 1 },
					{ DateTime.Today - TimeSpan.FromDays(1), 2 },
					{ DateTime.Today, 1 },
				},
				HabitType = HabitType.Unit,
				Id = FakeIdGenerator.GetId(),
				IsRecommended = false,
				RepeatType = RepeatType.Days - 1,
				DaysToRepeat = 10,
				StartDate = DateTime.Today - TimeSpan.FromDays(4)
			});
		}

		static async Task LoadHabits()
		{
			CurrentHabits.Add(GetExampleHabitViewModel());

			int id = 2;
			ScheduledHabits.Add(new HabitViewModel(new HabitModel
			{
				DailyFillingCount = 3,
				Title = "Morning push ups",
				Description = "Every morning make 40 push ups in 2 iterations (20 in each).",
				Filling = new Dictionary<DateTime, int>
				{
					{ DateTime.Today - TimeSpan.FromDays(4), 2 },
					{ DateTime.Today - TimeSpan.FromDays(3), 3 },
					{ DateTime.Today - TimeSpan.FromDays(2), 1 },
					{ DateTime.Today - TimeSpan.FromDays(1), 2 },
					{ DateTime.Today, 1 },
				},
				HabitType = HabitType.Unit,
				Id = FakeIdGenerator.GetId(),
				IsRecommended = false,
				RepeatType = RepeatType.Days - 1,
				DaysToRepeat = 10,
				StartDate = DateTime.Today - TimeSpan.FromDays(4)
			}));
		}

		public static async void UpdateHabit(HabitViewModel habit)
		{
			//await _connection.UpdateAsync(habit.GetHabitModel());
		}

		public static async void DeleteHabit(HabitViewModel habit)
		{
			//await _connection.DeleteAsync(habit.GetHabitModel());
		}

		/// <summary>
		/// Inserts the given habit to the database and fills his ModelId.
		/// </summary>
		/// <param name="habit">The habit which will be inserted to the database.</param>
		public static async Task InsertHabit(HabitViewModel habit)
		{
			try
			{
				var model = habit.GetHabitModel();
				//await _connection.InsertAsync(model);
				habit.SetModelId(FakeIdGenerator.GetId()); //model.Id);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"---------- Exception thrown during Inserting a habit: \"{ ex.Message }\"");
				Debug.WriteLine($"And the stack trace: \"{ ex.StackTrace }\"");
			}
		}

		public static void AddHabitToTheList(HabitViewModel habit)
		{
			UnscheduledHabits.Add(habit);
		}

		public static void RemoveHabitFromTheList(HabitViewModel habit)
		{
			UnscheduledHabits.Remove(habit);
		}
		#endregion

	}
}