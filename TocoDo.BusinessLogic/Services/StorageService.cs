using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
	// TODO: Unit test the service
	public class StorageService : IStorageService
	{
		#region Collections

		private readonly ObservableCollection<ITaskViewModel> _tasks;
		private readonly ObservableCollection<IHabitViewModel> _habits;
		private IDateTimeProvider _dateTimeProvider;
		private HabitScheduleHelper _scheduleHelper;

		public ReadOnlyObservableCollection<ITaskViewModel> AllTasks  { get; }
		public ReadOnlyObservableCollection<IHabitViewModel> AllHabits { get; }

		public IModelFactory ModelFactory { get; set; }
		public IDateTimeProvider DateTimeProvider
		{
			get => _dateTimeProvider;
			set
			{
				_dateTimeProvider = value;
				_scheduleHelper   = new HabitScheduleHelper(_dateTimeProvider);
			}
		}
		public INavigationService Navigation  { get; set; }
		public IPersistance       Persistance { get; set; }

		#endregion

		public StorageService()
		{
			#region Setup collections

			_tasks   = new ObservableCollection<ITaskViewModel>();

			AllTasks = new ReadOnlyObservableCollection<ITaskViewModel>(_tasks);

			_habits   = new ObservableCollection<IHabitViewModel>();
			AllHabits = new ReadOnlyObservableCollection<IHabitViewModel>(_habits);

			#endregion
		}

		public StorageService(IPersistance persistance, INavigationService navigation, IModelFactory modelFactory, IDateTimeProvider dateTimeProvider) : this()
		{
			MyLogger.WriteStartMethod();

			Persistance      = persistance;
			Navigation       = navigation;
			ModelFactory     = modelFactory;
			DateTimeProvider = dateTimeProvider;

			MyLogger.WriteEndMethod();
		}

		public async Task InitAsync()
		{
			MyLogger.WriteStartMethod();
			Persistance.Init();
			await InitDb();
			await LoadFromDb();
			MyLogger.WriteEndMethod();
		}

		#region Habits

		public void StartCreatingHabit()
		{
			_habits.Add(new HabitViewModel(this, Navigation));
		}

		public async Task ConfirmCreationOfHabit(IHabitViewModel habit)
		{
			await Persistance.InsertAsync(ModelFactory.CreateHabitModel(habit));
		}

		public void CancelCreationOfHabit(IHabitViewModel habit)
		{
			_habits.Remove(habit);
		}

		public async Task UpdateHabit(IHabitViewModel habit)
		{
			await Persistance.UpdateAsync(ModelFactory.CreateHabitModel(habit));
		}

		public async Task DeleteHabit(IHabitViewModel habit)
		{
			_habits.Remove(habit);
			await Persistance.DeleteAsync(ModelFactory.CreateHabitModel(habit));
		}

		#endregion

		#region Tasks

		public void StartCreatingTask(DateTime? date)
		{
			MyLogger.WriteStartMethod();
			_tasks.Add(new TaskViewModel(this, Navigation) {ScheduleDate = date});
			MyLogger.WriteEndMethod();
		}

		public async Task ConfirmCreationOfTask(ITaskViewModel task)
		{
			await Persistance.InsertAsync(ModelFactory.CreateTaskModel(task));
		}

		public void CancelCreationOfTask(ITaskViewModel task)
		{
			_tasks.Remove(task);
		}

		public async Task UpdateTask(ITaskViewModel task)
		{
			await Persistance.UpdateAsync(ModelFactory.CreateTaskModel(task));
		}

		public async Task DeleteTask(ITaskViewModel task)
		{
			_tasks.Remove(task);
			await Persistance.DeleteAsync(ModelFactory.CreateTaskModel(task));
		}

		#endregion

		#region Private methods

		private async Task InitDb()
		{
			MyLogger.WriteStartMethod();
			try
			{
				await Persistance.CreateTables();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}

			MyLogger.WriteEndMethod();
		}

		private async Task LoadFromDb()
		{
			MyLogger.WriteStartMethod();
			try
			{
				await LoadTasks();
				await LoadHabits();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}

			MyLogger.WriteEndMethod();
		}

		private async Task LoadTasks()
		{
			var result = await Persistance.GetTasks();
			result.ForEach(t =>
			{
				var task = new TaskViewModel(t);
				_tasks.Add(task);
			});
		}

		private async Task LoadHabits()
		{
			var models    = await Persistance.GetHabits();
			var habitList = models.Select(m => new HabitViewModel(this, Navigation, m));

			var filtred                 = new List<HabitViewModel>();
			var yesterdayFinishedHabits = new List<HabitViewModel>();
			foreach (var habit in habitList)
			{
				if (_scheduleHelper.IsHabitFinished(habit))
				{
					yesterdayFinishedHabits.Add(habit);
					continue;
				}

				filtred.Add(habit);
			}

			filtred.ForEach(_habits.Add);
		}

		#endregion
	}
}