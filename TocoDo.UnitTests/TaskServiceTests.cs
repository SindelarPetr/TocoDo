using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Services;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.UnitTests
{
	[TestClass]
	public class TaskServiceTests
	{
		#region Private fields
		private IPersistance _persistance;
		private ITaskService _taskService;
		private IDateTimeProvider _dateTime;
		private INavigationService _navigationService;
		private IModelFactory _modelFactory;
		private ITaskViewModel _taskViewModel;
		#endregion

		[TestInitialize]
		public void InitializeTest()
		{
			_persistance = Mock.Create<IPersistance>();
			_dateTime = Mock.Create<IDateTimeProvider>();
			_navigationService = Mock.Create<INavigationService>();
			_modelFactory = Mock.Create<IModelFactory>();
			_taskService = new TaskService
			           {
				           Persistance      = _persistance,
				           DateTimeProvider = _dateTime,
				           ModelFactory     = _modelFactory,
				           Navigation       = _navigationService,
			           };
			_taskViewModel = Mock.Create<ITaskViewModel>();
		}

		[TestMethod]
		public void StartCreationAddsANewTaskToTheListWithAllTasksButNotToThestorage()
		{
			// Arrange
			Mock.Arrange(() => _persistance.InsertAsync(Arg.AnyObject)).OccursNever();

			// Act
			_taskService.StartCreation(null);

			// Assert
			Assert.AreEqual(1, _taskService.AllTasks.Count);
			Mock.Assert(_persistance);
		}

		class Model : ITaskModel
		{
			public int Id { get; set; }
			public string Title { get; set; }
			public DateTime? Done { get; set; }
			public string Description { get; set; }
			public DateTime? ScheduleDate { get; set; }
			public DateTime? Deadline { get; set; }
			public DateTime CreateTime { get; set; }
			public DateTime? Reminder { get; set; }
		}

		[TestMethod]
		public async Task ConfirmCreatingTaskInsertsTheTaskToTheDatabaseAndWillNotCallAnyUpdate()
		{
			// Arrange
			Mock.Arrange(() => _persistance.InsertAsync(Arg.AnyObject)).Returns(Task.CompletedTask).OccursOnce();
			Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).OccursNever();
			Mock.Arrange(() => _modelFactory.CreateTaskModel(_taskViewModel)).Returns(new Model());
			
			// Act
			_taskService.StartCreation(null);
			var task = _taskService.AllTasks[0];
			await _taskService.ConfirmCreationAsync(task);

			// Assert
			Mock.Assert(_persistance);
		}

		[TestMethod]
		public void CancelCreatingTaskRemovesTheTaskFromAllTasksListAndWontInsertOrUpdateAnythingToTheDatabase()
		{
			// Arrange
			Mock.Arrange(() => _persistance.InsertAsync(Arg.AnyObject)).OccursNever();
			Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).OccursNever();

			// Act
			_taskService.StartCreation(null);
			_taskService.CancelCreation(_taskService.AllTasks[0]);

			// Assert
			Assert.AreEqual(0, _taskService.AllTasks.Count);
			Mock.Assert(_persistance);
		}

		[TestMethod]
		public async Task DeleteTaskCallsDeleteOnThePersistance()
		{
			// Arrange
			Mock.Arrange(() => _persistance.DeleteAsync(Arg.AnyObject)).Returns(Task.CompletedTask).OccursOnce();

			// Act
			_taskService.StartCreation(null);
			var task = _taskService.AllTasks[0];
			await _taskService.ConfirmCreationAsync(task);
			await _taskService.DeleteAsync(task);

			// Assert
			Mock.Assert(_persistance);
		}

		[TestMethod]
		public async Task DeleteTaskRemovesTheTaskFromTheList()
		{
			// Arrange

			// Act
			_taskService.StartCreation(null);
			var task = _taskService.AllTasks[0];
			await _taskService.ConfirmCreationAsync(task);
			await _taskService.DeleteAsync(task);

			// Assert
			Assert.AreEqual(0, _taskService.AllTasks.Count);
		}

		[TestMethod]
		public async Task UpdateTaskCallsUpdateToThePersistence()
		{
			// Arrange
			Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).Returns(Task.CompletedTask).OccursOnce();

			// Act
			_taskService.StartCreation(null);
			var task = _taskService.AllTasks[0];
			await _taskService.ConfirmCreationAsync(task);
			await _taskService.UpdateAsync(task);

			// Assert
			Mock.Assert(_persistance);
		}
	}
}
