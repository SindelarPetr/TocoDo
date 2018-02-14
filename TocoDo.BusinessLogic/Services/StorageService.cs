using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
	public class StorageService : IStorageService
	{
		private readonly IPersistance _persistance;
		private readonly INavigationService _navigation;
		private readonly IModelFactory _modelFactory;

		#region Collections
		private ObservableCollection<TaskViewModel> _tasks;
		private ObservableCollection<HabitViewModel> _habits;

		public ReadOnlyObservableCollection<TaskViewModel> AllTasks { get; }

		public ReadOnlyObservableCollection<HabitViewModel> AllHabits { get; }

		#endregion

		public StorageService(IPersistance persistance, INavigationService navigation, IModelFactory modelFactory)
		{
			_persistance = persistance;
			_navigation = navigation;
			_modelFactory = modelFactory;

			#region Setup collections
			_tasks = new ObservableCollection<TaskViewModel>();
			AllTasks = new ReadOnlyObservableCollection<TaskViewModel>(_tasks);

			_habits = new ObservableCollection<HabitViewModel>();
			AllHabits = new ReadOnlyObservableCollection<HabitViewModel>(_habits);
			#endregion
		}

		public void StartCreatingHabit()
		{
			_habits.Add(new HabitViewModel(this, _navigation));
		}

		public Task ConfirmCreationOfHabit(HabitViewModel habit)
		{
			throw new NotImplementedException();
		}

		public void CancelCreationOfHabit(HabitViewModel habit)
		{
			throw new NotImplementedException();
		}

		public Task UpdateHabit(HabitViewModel habit)
		{
			throw new NotImplementedException();
		}

		public Task DeleteHabit(HabitViewModel habit)
		{
			throw new NotImplementedException();
		}

		public Task DeleteTask(TaskViewModel taskViewModel)
		{
			throw new NotImplementedException();
		}

		public Task UpdateTask(TaskViewModel task)
		{
			throw new NotImplementedException();
		}

		public Task StartCreatingTask(DateTime? date)
		{
			throw new NotImplementedException();
		}

		public Task ConfirmCreationOfTask(TaskViewModel task)
		{
			throw new NotImplementedException();
		}

		public Task CancelCreationOfTask(TaskViewModel task)
		{
			throw new NotImplementedException();
		}
	}
}