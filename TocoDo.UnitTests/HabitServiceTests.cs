using System;
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
	public class HabitServiceTests
	{
		#region Private fields
		private IPersistance _persistance;
		private IHabitService _storage;
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
			_storage = new HabitService
			{
				Persistance = _persistance,
				DateTimeProvider = _dateTime,
				ModelFactory = _modelFactory,
				Navigation = _navigationService,
			};
			_habitViewModel = Mock.Create<IHabitViewModel>();
		}

		[TestMethod]
		public void StartCreationDoesNotInsertAnythingToTheDatabase()
		{
			// Arrange
			Mock.Arrange(() => _persistance.InsertAsync(new object())).Returns(Task.CompletedTask).OccursNever();

			// Act
			_storage.StartCreation();

			// Assert
			Mock.Assert(_persistance);
		}

		[TestMethod]
		public void StartCreationAddsHabitToTheStorageListWithAllHabits()
		{
			// Arrange
			var expectedResult = 1;

			// Act
			_storage.StartCreation();

			// Assert
			Assert.AreEqual(expectedResult, _storage.AllHabits.Count);
		}

		[TestMethod]
		public void CancelCreatingHabitRemovesTheHabitFromAllHabitsListAndWontInsertOrUpdateAnythingToTheDatabase()
		{
			// Arrange
			Mock.Arrange(() => _persistance.InsertAsync(Arg.AnyObject)).Returns(Task.CompletedTask).OccursNever();
			Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_storage.StartCreation();
			_storage.CancelCreation(_storage.AllHabits[0]);

			// Assert
			Assert.AreEqual(0, _storage.AllHabits.Count);
			Mock.Assert(_persistance);
		}

		[TestMethod]
		public async Task DeleteHabitCallsDeleteOnThePersistance()
		{
			// Arrange
			Mock.Arrange(() => _persistance.DeleteAsync(Arg.AnyObject)).Returns(Task.CompletedTask).OccursOnce();

			// Act
			_storage.StartCreation();
			var habit = _storage.AllHabits[0];
			await _storage.ConfirmCreationAsync(habit);
			await _storage.DeleteAsync(habit);

			// Assert
			Mock.Assert(_persistance);
		}

		[TestMethod]
		public async Task DeleteHabitRemovesTheHabitFromTheList()
		{
			// Arrange

			// Act
			_storage.StartCreation();
			var habit = _storage.AllHabits[0];
			await _storage.ConfirmCreationAsync(habit);
			await _storage.DeleteAsync(habit);

			// Assert
			Assert.AreEqual(0, _storage.AllHabits.Count);
		}

		[TestMethod]
		public async Task UpdateHabitCallsUpdateToThePersistence()
		{
			// Arrange
			Mock.Arrange(() => _persistance.UpdateAsync(Arg.AnyObject)).Returns(Task.CompletedTask).OccursOnce();

			// Act
			_storage.StartCreation();
			var habit = _storage.AllHabits[0];
			await _storage.ConfirmCreationAsync(habit);
			await _storage.UpdateAsync(habit);

			// Assert
			Mock.Assert(_persistance);
		}
	}
}