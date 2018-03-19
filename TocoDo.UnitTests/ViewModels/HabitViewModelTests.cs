using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Extensions;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Services;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.UnitTests
{
	[TestClass]
	public class HabitViewModelTests
	{
		private INavigationService _navigationService;
		private IHabitService _habitService;
		private HabitViewModel _habitViewModel;
		private IDateTimeProvider _dateProvider;

		[TestInitialize]
		public void TestInit()
		{
			_dateProvider = Mock.Create<IDateTimeProvider>();
			_navigationService = Mock.Create<INavigationService>();
			_habitService = Mock.Create<IHabitService>();
			_habitViewModel = new HabitViewModel(_dateProvider, _habitService, _navigationService);
		}

		[TestMethod]
		public async Task EditCommandShouldNavigateToAHabitModifyPage()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.PushAsync(PageType.ModifyHabitPage, _habitViewModel))
				.Returns(Task.CompletedTask).OccursOnce();

			// Act
			await _habitViewModel.EditCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_navigationService);
		}

		[TestMethod]
		public void EditTitleCommandShouldModifyTheTitleIfASimpleTitleIsPassed()
		{
			// Arrange
			var oldTitle = "Tit";
			var expectedTitle = "TITLE";
			_habitViewModel.Title = oldTitle;

			// Act
			_habitViewModel.EditTitleCommand.Execute(expectedTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _habitViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldModifyTheTitleAndTrimTheValueIfATitleIsPassed()
		{
			// Arrange
			var oldTitle = "Tit";
			var newTitle = "   TITLE";
			var expectedTitle = "TITLE";
			_habitViewModel.Title = oldTitle;

			// Act
			_habitViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _habitViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldntDoAnythingIfTheNewTitleIsWhiteSpace()
		{
			// Arrange
			var oldTitle = "Tit";
			var newTitle = "   ";
			var expectedTitle = "Tit";
			_habitViewModel.Title = oldTitle;

			// Act
			_habitViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _habitViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldntDoAnythingIfTheNewTitleIsNotAString()
		{
			// Arrange
			var oldTitle = "Tit";
			var newTitle = new Mock();
			var expectedTitle = "Tit";
			_habitViewModel.Title = oldTitle;
			Mock.Arrange(() => _habitService.UpdateAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_habitViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _habitViewModel.Title);
			Mock.Assert(_habitService);
		}

		[TestMethod]
		public async Task ConfirmFinishCommandShouldCallConfirmCreationOfHabitServiceAndWontCallUpdateWhenANotNullOrWhitespaceTitleIsProvided()
		{
			// Arrange
			string finalTitle = "TITLE";
			Mock.Arrange(() => _habitService.ConfirmCreationAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursOnce();
			Mock.Arrange(() => _habitService.UpdateAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			await _habitViewModel.FinishCreationCommand.ExecuteAsync(finalTitle);

			// Assert
			Mock.Assert(_habitService);
		}

		[TestMethod]
		public async Task ConfirmFinishCommandShouldCallCancelCreationOfTaskServiceAndWontCallUpdateWhenANullTitleIsProvided()
		{
			// Arrange
			string finalTitle = null;
			Mock.Arrange(() => _habitService.CancelCreation(_habitViewModel)).OccursOnce();
			Mock.Arrange(() => _habitService.UpdateAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			await _habitViewModel.FinishCreationCommand.ExecuteAsync(finalTitle);

			// Assert
			Mock.Assert(_habitService);
		}

		[TestMethod]
		public async Task ConfirmFinishCommandShouldCallCancelCreationOfTaskServiceAndWontCallUpdateWhenAWhitespaceTitleIsProvided()
		{
			// Arrange
			string finalTitle = "  ";
			Mock.Arrange(() => _habitService.CancelCreation(_habitViewModel)).OccursOnce();
			Mock.Arrange(() => _habitService.UpdateAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			await _habitViewModel.FinishCreationCommand.ExecuteAsync(finalTitle);

			// Assert
			Mock.Assert(_habitService);
		}

		[TestMethod]
		public async Task RemoveCommandShouldRiseAnAlert()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(Arg.AnyBool)).OccursOnce();

			// Act
			await _habitViewModel.DeleteCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_navigationService);
		}

		[TestMethod]
		public async Task RemoveCommandAfterAcceptingAlertShouldCallDeleteAsyncOfTaskService()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(true)).InOrder();
			Mock.Arrange(() => _habitService.DeleteAsync(_habitViewModel)).Returns(Task.CompletedTask).InOrder().OccursOnce();

			// Act
			await _habitViewModel.DeleteCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_habitService);
		}

		[TestMethod]
		public async Task RemoveCommandAfterRefusingTheAlertShouldntCallDeleteAsync()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(false)).InOrder();
			Mock.Arrange(() => _habitService.DeleteAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			await _habitViewModel.DeleteCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_habitService);
		}

		[TestMethod]
		public async Task UpdateCommandShouldCallUpdateAsyncOfTaskService()
		{
			// Arrange
			Mock.Arrange(() => _habitService.UpdateAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursOnce();

			// Act
			await _habitViewModel.UpdateCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_habitService);
		}

		// Test habit filling - initialization, increment, save, load, increment, save, load
		[TestMethod]
		public void HabitInitializesFillingToEmptyCollectionIfStartDateIsLaterThanToday()
		{
			// Arrange
			var firstMondayDate = new DateTime(2017, 3, 5);
			var today = new DateTime(2017, 3, 4);
			var startDate = firstMondayDate.AddDays(2);
			var repeatType = RepeatType.Mon | RepeatType.Wed;
			var daysToRepeat = 3;

			var model = Mock.Create<IHabitModel>();
			Mock.Arrange(() => model.RepeatType).Returns(repeatType);
			Mock.Arrange(() => model.DaysToRepeat).Returns(daysToRepeat);
			Mock.Arrange(() => model.StartDate).Returns(startDate);
			Mock.Arrange(() => _dateProvider.Now).Returns(today);

			// Act
			var habit = new HabitViewModel(_dateProvider, _habitService, _navigationService, model);

			// Assert
			Assert.IsNotNull(habit.Filling);
			Assert.AreEqual(0, habit.Filling.Count());
		}

		// Test habit filling - initialization, increment, save, load, increment, save, load
		[TestMethod]
		public void HabitInitializesFillingWhenTheDateTodayIsStartDay()
		{
			// Arrange
			var firstMondayDate = new DateTime(2017, 3, 5);
			var startDate = firstMondayDate.AddDays(2);
			var repeatType = RepeatType.Mon | RepeatType.Wed;
			var daysToRepeat = 3;
			var expectedFilling = new ObservableDictionary<DateTime, int>
								  {
									  // First Week
				                      { startDate, 0},

									  { startDate.AddDays(5), 0},
									  { startDate.AddDays(7), 0},

									  { startDate.AddDays(5 + 7), 0},
									  { startDate.AddDays(7 + 7), 0},
								  };

			var model = Mock.Create<IHabitModel>();
			Mock.Arrange(() => model.RepeatType).Returns(repeatType);
			Mock.Arrange(() => model.DaysToRepeat).Returns(daysToRepeat);
			Mock.Arrange(() => model.StartDate).Returns(startDate);
			Mock.Arrange(() => _dateProvider.Now).Returns(firstMondayDate);

			// Act
			var habit = new HabitViewModel(_dateProvider, _habitService, _navigationService, model);


			// Assert
			CustomAsserts.DictionariesAreEqual(expectedFilling, habit.Filling);
		}
	}
}
