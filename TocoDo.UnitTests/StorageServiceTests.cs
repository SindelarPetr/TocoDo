using System;
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
	public class StorageServiceTests
	{
		[TestClass]
		public class HabitTests
		{
			#region Private fields
			private IPersistance _persistance;
			private IStorageService _storage;
			private IDateTimeProvider _dateTime;
			private INavigationService _navigationService;
			private IModelFactory _modelFactory;
			private IHabitViewModel _habitViewModel;
			#endregion

			[TestInitialize]
			public void InitializeTest()
			{
				_persistance = Mock.Create<IPersistance>();
				_dateTime = Mock.Create<IDateTimeProvider>();
				_navigationService = Mock.Create<INavigationService>();
				_modelFactory = Mock.Create<IModelFactory>();
				_storage = new StorageService(_persistance, _navigationService, _modelFactory, _dateTime);
				_habitViewModel = Mock.Create<IHabitViewModel>();
			}

			[TestMethod]
			public void StartCreatingHabitDoesNotInsertAnythingToTheDatabase()
			{
				// Arrange
				Mock.Arrange(() => _persistance.InsertAsync(new object())).OccursNever();

				// Act
				_storage.StartCreatingHabit();

				// Assert
				Mock.Assert(_persistance);
			}

			[TestMethod]
			public void StartCreatingHabitAddsHabitToTheStorageListWithAllHabits()
			{
				// Arrange
				var expectedResult = 1;

				// Act
				_storage.StartCreatingHabit();

				// Assert
				Assert.AreEqual(expectedResult, _storage.AllHabits.Count);
			}

			[TestMethod]
			public void CancelCreatingHabitRemovesTheHabitFromAllHabitsListAndWontInsertOrUpdateAnythingToTheDatabase()
			{
				// Arrange
				Mock.Arrange(() => _persistance.InsertAsync(Arg.AnyObject)).OccursNever();
				Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).OccursNever();

				// Act
				_storage.StartCreatingHabit();
				_storage.CancelCreationOfHabit(_storage.AllHabits[0]);

				// Assert
				Assert.AreEqual(0, _storage.AllHabits.Count);
				Mock.Assert(_persistance);
			}

			[TestMethod]
			public void DeleteHabitCallsDeleteOnThePersistance()
			{
				// Arrange
				Mock.Arrange(() => _persistance.DeleteAsync(Arg.AnyObject)).OccursOnce();

				// Act
				_storage.StartCreatingHabit();
				var habit = _storage.AllHabits[0];
				_storage.ConfirmCreationOfHabit(habit);
				_storage.DeleteHabit(habit);

				// Assert
				Mock.Assert(_persistance);
			}

			[TestMethod]
			public void DeleteHabitRemovesTheHabitFromTheList()
			{
				// Arrange

				// Act
				_storage.StartCreatingHabit();
				var habit = _storage.AllHabits[0];
				_storage.ConfirmCreationOfHabit(habit);
				_storage.DeleteHabit(habit);

				// Assert
				Assert.AreEqual(0, _storage.AllHabits.Count);
			}

			[TestMethod]
			public void UpdateHabitCallsUpdateToThePersistence()
			{
				// Arrange
				Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).OccursOnce();

				// Act
				_storage.StartCreatingHabit();
				var habit = _storage.AllHabits[0];
				_storage.ConfirmCreationOfHabit(habit);
				_storage.UpdateHabit(habit);

				// Assert
				Mock.Assert(_persistance);
			}
		}

		[TestClass]
		public class TaskTests
		{
			#region Private fields
			private IPersistance _persistance;
			private IStorageService _storage;
			private IDateTimeProvider _dateTime;
			private INavigationService _navigationService;
			private IModelFactory _modelFactory;
			private ITaskViewModel _taskViewModel;
			#endregion

			[TestInitialize]
			public void InitializeTest()
			{
				_persistance       = Mock.Create<IPersistance>();
				_dateTime          = Mock.Create<IDateTimeProvider>();
				_navigationService = Mock.Create<INavigationService>();
				_modelFactory      = Mock.Create<IModelFactory>();
				_storage           = new StorageService(_persistance, _navigationService, _modelFactory, _dateTime);
				_taskViewModel     = Mock.Create<ITaskViewModel>();
			}

			[TestMethod]
			public void StartCreatingTaskAddsANewTaskToTheListWithAllTasksButNotToThestorage()
			{
				// Arrange
				Mock.Arrange(() => _persistance.InsertAsync(Arg.AnyObject)).OccursNever();

				// Act
				_storage.StartCreatingTask(null);

				// Assert
				Assert.AreEqual(1, _storage.AllTasks.Count);
				Mock.Assert(_persistance);
			}

			[TestMethod]
			public void ConfirmCreatingTaskInsertsTheTaskToTheDatabaseAndWillNotCallAnyUpdate()
			{
				// Arrange
				Mock.Arrange(() => _persistance.InsertAsync(Arg.AnyObject)).OccursOnce();
				Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).OccursNever();

				// Act
				_storage.StartCreatingTask(null);
				_storage.ConfirmCreationOfTask(_taskViewModel);

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
				_storage.StartCreatingTask(null);
				_storage.CancelCreationOfTask(_storage.AllTasks[0]);

				// Assert
				Assert.AreEqual(0, _storage.AllTasks.Count);
				Mock.Assert(_persistance);
			}

			[TestMethod]
			public void DeleteTaskCallsDeleteOnThePersistance()
			{
				// Arrange
				Mock.Arrange(() => _persistance.DeleteAsync(Arg.AnyObject)).OccursOnce();

				// Act
				_storage.StartCreatingTask(null);
				var task = _storage.AllTasks[0];
				_storage.ConfirmCreationOfTask(task);
				_storage.DeleteTask(task);

				// Assert
				Mock.Assert(_persistance);
			}

			[TestMethod]
			public void DeleteTaskRemovesTheTaskFromTheList()
			{
				// Arrange

				// Act
				_storage.StartCreatingTask(null);
				var task = _storage.AllTasks[0];
				_storage.ConfirmCreationOfTask(task);
				_storage.DeleteTask(task);

				// Assert
				Assert.AreEqual(0, _storage.AllTasks.Count);
			}

			[TestMethod]
			public void UpdateTaskCallsUpdateToThePersistence()
			{
				// Arrange
				Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).OccursOnce();

				// Act
				_storage.StartCreatingTask(null);
				var task = _storage.AllTasks[0];
				_storage.ConfirmCreationOfTask(task);
				_storage.UpdateTask(task);

				// Assert
				Mock.Assert(_persistance);
			}
		}
	}
}