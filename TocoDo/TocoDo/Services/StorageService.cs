using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetBox.Extensions;
using TocoDo.Helpers;
using TocoDo.Models;
using TocoDo.ViewModels;

namespace TocoDo.Services
{
	// TODO: Make StorageService non static
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

		public static ObservableCollection<HabitViewModel> ActiveHabits { get; } = new ObservableCollection<HabitViewModel>();
		public static ObservableCollection<HabitViewModel> CurrentHabits { get; } = new ObservableCollection<HabitViewModel>();
		public static ObservableCollection<HabitViewModel> ScheduledHabits { get; } = new ObservableCollection<HabitViewModel>();
		public static ObservableCollection<HabitViewModel> UnscheduledHabits { get; } = new ObservableCollection<HabitViewModel>();
		public static ObservableCollection<HabitViewModel> YesterdayFinishedHabits { get; } = new ObservableCollection<HabitViewModel>();
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

			var viewModel = new TaskViewModel(newTask);

			await InsertTask(viewModel);

			return viewModel;
		}

		public static async Task InsertTask(TaskViewModel viewModel, bool addToTheList = true)
		{
			var task = viewModel.GetTaskModel();
			await InsertTask(task);
			viewModel.Id = task.Id;

			if(!addToTheList)
				return;

			AddTaskToTheList(viewModel);
			BindTask(viewModel);
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

		public static void AddTaskToTheList(TaskViewModel task)
		{
			Debug.Write("------------- Adding task to the list");
			Debug.Write("------------- Id of the task is: " + task.Id);

			GetTaskListForDate(task.ScheduleDate).Add(task);
			Debug.Write("------------- Finished adding task to the list");
		}

		private static void BindTask(TaskViewModel task)
		{
			task.PropertyChanging += TaskOnPropertyChanging;
			task.PropertyChanged += TaskOnPropertyChanged;
		}

		private static void TaskOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if(propertyChangedEventArgs.PropertyName == nameof(TaskViewModel.ScheduleDate))
				AddTaskToTheList((TaskViewModel)sender);
		}

		private static void TaskOnPropertyChanging(object sender, object oldValue, object newValue, string propertyName)
		{
			if (propertyName == nameof(TaskViewModel.ScheduleDate))
				RemoveTaskFromTheList((TaskViewModel)sender);
		}

		private static ObservableCollection<TaskViewModel> GetTaskListForDate(DateTime? date)
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

		public static void RemoveTaskFromTheList(TaskViewModel task)
		{
			GetTaskListForDate(task.ScheduleDate).Remove(task);
		}

		private static void UnbindTask(TaskViewModel task)
		{
			task.PropertyChanging -= TaskOnPropertyChanging;
			task.PropertyChanged -= TaskOnPropertyChanged;
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
			MyLogger.WriteStartMethod();
			await _connection.CreateTableAsync<HabitModel>();
			MyLogger.WriteEndMethod();
		}

		public static HabitViewModel GetExampleHabitViewModel()
		{
			//return new HabitViewModel(new HabitModel
			//{
			//	RepeatsToday = 3,
			//	Title = "Meditation",
			//	Description = "Sit somewhere where its quiet and calm down all thoughts for 20 minutes.",
			//	Filling = new Dictionary<DateTime, int>
			//	{
			//		{ DateTime.Today - TimeSpan.FromDays(4), 2 },
			//		{ DateTime.Today - TimeSpan.FromDays(3), 3 },
			//		{ DateTime.Today - TimeSpan.FromDays(2), 1 },
			//		{ DateTime.Today - TimeSpan.FromDays(1), 2 },
			//		{ DateTime.Today, 1 },
			//	},
			//	HabitType = HabitType.Unit,
			//	Id = FakeIdGenerator.GetId(),
			//	IsRecommended = false,
			//	RepeatType = RepeatType.Days - 2,
			//	DaysToRepeat = 13,
			//	StartDate = DateTime.Today - TimeSpan.FromDays(4)
			//});
			return new HabitViewModel();
		}

		private static Dictionary<DateTime, int> GenerateFilling()
		{
			var rand = new Random();
			var dic = new Dictionary<DateTime, int>();
			for (int i = 0; i < 100; i++)
			{
				dic.Add(DateTime.Today.AddDays(-50).AddDays(i), (i < 50)? rand.Next(0, 2) : 0);
			}

			return dic;
		}

		private static List<HabitViewModel> GetFakeHabits()
		{
			return new List<HabitViewModel>();
			//{
			//	GetExampleHabitViewModel(),
			//	new HabitViewModel(new HabitModel
			//	{
			//		RepeatsADay = 5,
			//		Title = "Today started 4 days ago",
			//		Description = "Every morning make 40 push ups in 2 iterations (20 in each).",
			//		Filling = GenerateFilling(),
			//		HabitType = HabitType.Unit,
			//		Id = FakeIdGenerator.GetId(),
			//		IsRecommended = false,
			//		RepeatType = RepeatType.Fri,
			//		DaysToRepeat = 12,
			//		StartDate = DateTime.Today - TimeSpan.FromDays(4)
			//	}),
			//	new HabitViewModel(new HabitModel
			//	{
			//		RepeatsADay = 5,
			//		Title = "Today starts today",
			//		Description = "Every morning make 40 push ups in 2 iterations (20 in each).",
			//		Filling = new Dictionary<DateTime, int>
			//		{
			//			{ DateTime.Today - TimeSpan.FromDays(4), 2 },
			//			{ DateTime.Today - TimeSpan.FromDays(3), 3 },
			//			{ DateTime.Today - TimeSpan.FromDays(2), 1 },
			//			{ DateTime.Today - TimeSpan.FromDays(1), 2 },
			//			{ DateTime.Today, 1 },
			//		},
			//		HabitType = HabitType.Unit,
			//		Id = FakeIdGenerator.GetId(),
			//		IsRecommended = false,
			//		RepeatType = RepeatType.Fri,
			//		DaysToRepeat = 12,
			//		StartDate = DateTime.Today
			//	}),
			//	new HabitViewModel(new HabitModel
			//	{
			//	RepeatsADay = 5,
			//	Title = "Current started 4 days ago (Thursday + Saturday)",
			//	Description = "Every morning make 40 push ups in 2 iterations (20 in each).",
			//	Filling = new Dictionary<DateTime, int>
			//	{
			//		{ DateTime.Today - TimeSpan.FromDays(4), 2 },
			//		{ DateTime.Today - TimeSpan.FromDays(3), 3 },
			//		{ DateTime.Today - TimeSpan.FromDays(2), 1 },
			//		{ DateTime.Today - TimeSpan.FromDays(1), 2 },
			//		{ DateTime.Today, 1 },
			//	},
			//	HabitType = HabitType.Daylong,
			//	Id = FakeIdGenerator.GetId(),
			//	IsRecommended = false,
			//	RepeatType = RepeatType.Sat | RepeatType.Thu,
			//	DaysToRepeat = 12,
			//	StartDate = DateTime.Today - TimeSpan.FromDays(4)
			//}),
			//	new HabitViewModel(new HabitModel
			//	{
			//	RepeatsADay = 5,
			//	Title = "Finished 1 year ago",
			//	Description = "Every morning make 40 push ups in 2 iterations (20 in each).",
			//	Filling = new Dictionary<DateTime, int>
			//	{
			//		{ DateTime.Today - TimeSpan.FromDays(4), 2 },
			//		{ DateTime.Today - TimeSpan.FromDays(3), 3 },
			//		{ DateTime.Today - TimeSpan.FromDays(2), 1 },
			//		{ DateTime.Today - TimeSpan.FromDays(1), 2 },
			//		{ DateTime.Today, 1 },
			//	},
			//	HabitType = HabitType.Daylong,
			//	Id = FakeIdGenerator.GetId(),
			//	IsRecommended = false,
			//	RepeatType = RepeatType.Years,
			//	DaysToRepeat = 4,
			//	StartDate = DateTime.Today.AddYears(-5),
			//}),
			//	new HabitViewModel(new HabitModel
			//	{
			//	RepeatsADay = 5,
			//	Title = "Will start tomorrow",
			//	Description = "Every morning make 40 push ups in 2 iterations (20 in each).",
			//	Filling = new Dictionary<DateTime, int>
			//	{
			//		{ DateTime.Today - TimeSpan.FromDays(4), 2 },
			//		{ DateTime.Today - TimeSpan.FromDays(3), 3 },
			//		{ DateTime.Today - TimeSpan.FromDays(2), 1 },
			//		{ DateTime.Today - TimeSpan.FromDays(1), 2 },
			//		{ DateTime.Today, 1 },
			//	},
			//	HabitType = HabitType.Daylong,
			//	Id = FakeIdGenerator.GetId(),
			//	IsRecommended = false,
			//	RepeatType = RepeatType.Days,
			//	DaysToRepeat = 20,
			//	StartDate = DateTime.Today + TimeSpan.FromDays(1)
			//})
			//};


		}

		static async Task LoadHabits()
		{
			var models = await _connection.Table<HabitModel>().Where(h => h.IsFinished == false).ToListAsync();
			//var habitList = GetFakeHabits();
			var habitList = models.Select(m => new HabitViewModel(m));

			var filtred = new List<HabitViewModel>();
			foreach (var habit in habitList)
			{
				if (IsHabitFinished(habit))
				{
					YesterdayFinishedHabits.Add(habit);
					continue;
				}

				filtred.Add(habit);

				if (IsHabitToday(habit))
				{
					ActiveHabits.Add(habit);
				}
			}

			filtred.ForEach(AddHabitToTheList);
		}

		public static async Task UpdateHabit(HabitViewModel habit)
		{
			await _connection.UpdateAsync(habit.GetHabitModel());
		}

		public static async Task DeleteHabit(HabitViewModel habit)
		{
		 	var list = GetHabitList(habit);
			list.Remove(habit);
			await _connection.DeleteAsync(habit.GetHabitModel());
			await PageService.PopAsync();
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
				//habit.SetModelId(FakeIdGenerator.GetId()); //model.Id);
				await _connection.InsertAsync(model);
				habit.SetModelId(model.Id);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"---------- Exception thrown during Inserting a habit: \"{ ex.Message }\"");
				Debug.WriteLine($"And the stack trace: \"{ ex.StackTrace }\"");
			}
		}

		public static void AddHabitToTheList(HabitViewModel habit)
		{
			// Get the propper position for the habit
			var list = GetHabitList(habit);
			list.Add(habit);
			habit.PropertyChanging += OnHabitOnPropertyChanging;
		}

		private static void OnHabitOnPropertyChanging(object habit, object oldValue, object newValue, string propertyName)
		{
			if (propertyName != nameof(HabitViewModel.ModelStartDate)) return;

			var vm = (HabitViewModel) habit;
			var oldList = GetHabitList(vm);
			var newList = GetHabitList((DateTime?) newValue, vm.ModelRepeatType, vm.ModelDaysToRepeat);

			if (oldList == newList)
				return;

			RemoveHabitFromTheList(vm);
			newList.Add(vm);
			vm.PropertyChanging += OnHabitOnPropertyChanging;
		}

		public static void RemoveHabitFromTheList(HabitViewModel habit)
		{
			var	list = GetHabitList(habit);
			list?.Remove(habit);
			habit.PropertyChanging -= OnHabitOnPropertyChanging;
		}

		private static ObservableCollection<HabitViewModel> GetHabitList(HabitViewModel habit) =>
			GetHabitList(habit.ModelStartDate, habit.ModelRepeatType, habit.ModelDaysToRepeat);

		private static ObservableCollection<HabitViewModel> GetHabitList(DateTime? start, RepeatType repeatType, int daysToRepeat)
		{
			if (start == null)
				return UnscheduledHabits;

			// Habit will begin in the future
			if (start > DateTime.Now)
			{
				return ScheduledHabits;
			}

			// check if habit already ended
			if (DateTimeHelper.IsHabitFinished(start, repeatType, daysToRepeat))
				return null;

			return CurrentHabits;
		}

		private static TimeSpan HabitLength(DateTime start, RepeatType repeatType, int daysToRepeat)
		{
			switch (repeatType)
			{
				case RepeatType.Years:
					return start.AddYears(daysToRepeat) - start;
				case RepeatType.Months:
					return start.AddMonths(daysToRepeat) - start;
				case RepeatType.Days:
					return start.AddDays(daysToRepeat) - start;
			}

			int fromWeekMissing = 0;
			switch (repeatType)
			{
				case RepeatType.Sat:
					fromWeekMissing = 1;
					break;
				case RepeatType.Fri:
					fromWeekMissing = 2;
					break;
				case RepeatType.Thu:
					fromWeekMissing = 3;
					break;
				case RepeatType.Wed:
					fromWeekMissing = 4;
					break;
				case RepeatType.Tue:
					fromWeekMissing = 5;
					break;
				case RepeatType.Mon:
					fromWeekMissing = 6;
					break;
			}

			return start.AddDays(daysToRepeat * 7 - fromWeekMissing) - start;
		}

		private static bool IsHabitToday(HabitViewModel habit)
		{
			return DateTimeHelper.IsHabitToday(habit.ModelStartDate, habit.ModelRepeatType, habit.ModelDaysToRepeat);
		}

		private static bool IsHabitFinished(HabitViewModel habit) =>
			DateTimeHelper.IsHabitFinished(habit.ModelStartDate, habit.ModelRepeatType, habit.ModelDaysToRepeat);

		private static bool IsHabitCurrent(HabitViewModel habit)
		{
			return DateTimeHelper.IsHabitCurrent(habit.ModelStartDate, habit.ModelRepeatType, habit.ModelDaysToRepeat);
		}
		#endregion
	}
}