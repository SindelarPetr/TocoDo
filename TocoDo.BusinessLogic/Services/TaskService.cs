using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
	public class TaskService : ITaskService
	{
		#region Fields

		private readonly ObservableCollection<ITaskViewModel> _tasks;

		#endregion

		public TaskService()
		{
			_tasks   = new ObservableCollection<ITaskViewModel>();
			AllTasks = new ReadOnlyObservableCollection<ITaskViewModel>(_tasks);
		}

		#region Interface

		public IModelFactory      ModelFactory     { get; set; }
		public IDateTimeProvider  DateTimeProvider { get; set; }
		public INavigationService Navigation       { get; set; }
		public IPersistance       Persistance      { get; set; }

		public ReadOnlyObservableCollection<ITaskViewModel> AllTasks { get; }

		public async Task LoadAsync()
		{
			MyLogger.WriteStartMethod();
			
			var result = await Persistance.GetTasks();
			result.ForEach(t =>
			{
				MyLogger.WriteStartMethod("null", "Start of Loop of LoadAsync");
				var task = new TaskViewModel(this, Navigation, t);
				MyLogger.WriteInMethod("Adding a task");
				_tasks.Add(task);
				MyLogger.WriteEndMethod("null", "End of Loop of LoadAsync");
			});

			MyLogger.WriteEndMethod();
		}

		public void StartCreation(DateTime? date)
		{
			MyLogger.WriteStartMethod();
			_tasks.Add(new TaskViewModel(this, Navigation) {ScheduleDate = date});
			MyLogger.WriteEndMethod();
		}

		public async Task ConfirmCreationAsync(ITaskViewModel task)
		{
			var taskModel = ModelFactory.CreateTaskModel(task);
			await Persistance.InsertAsync(taskModel);
		}

		public void CancelCreation(ITaskViewModel task)
		{
			_tasks.Remove(task);
		}

		public async Task UpdateAsync(ITaskViewModel task)
		{
			await Persistance.UpdateAsync(ModelFactory.CreateTaskModel(task));
		}

		public async Task DeleteAsync(ITaskViewModel task)
		{
			_tasks.Remove(task);
			await Persistance.DeleteAsync(ModelFactory.CreateTaskModel(task));
		}



		#endregion
	}
}