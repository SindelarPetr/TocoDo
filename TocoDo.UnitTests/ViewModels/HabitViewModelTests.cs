using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using TocoDo.BusinessLogic.DependencyInjection;
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

		[TestInitialize]
		public void TestInit()
		{
			_navigationService = Mock.Create<INavigationService>();
			_habitService = Mock.Create<IHabitService>();
			_habitViewModel = new HabitViewModel(_habitService, _navigationService);
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

		// Move following code to ModifyHabitViewModelTests
		//[TestMethod]
		//public async Task RemoveCommandShouldRiseAnAlert()
		//{
		//	// Arrange
		//	Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(Arg.AnyBool)).OccursOnce();

		//	// Act
		//	await _habitViewModel.DeleteCommand.ExecuteAsync(null);

		//	// Assert
		//	Mock.Assert(_navigationService);
		//}

		//[TestMethod]
		//public async Task RemoveCommandAfterAcceptingAlertShouldCallDeleteAsyncOfTaskService()
		//{
		//	// Arrange
		//	Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(true)).InOrder();
		//	Mock.Arrange(() => _habitService.DeleteAsync(_habitViewModel)).Returns(Task.CompletedTask).InOrder().OccursOnce();

		//	// Act
		//	await _habitViewModel.RemoveCommand.ExecuteAsync(null);

		//	// Assert
		//	Mock.Assert(_habitService);
		//}

		//[TestMethod]
		//public async Task RemoveCommandAfterRefusingTheAlertShouldntCallDeleteAsync()
		//{
		//	// Arrange
		//	Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(false)).InOrder();
		//	Mock.Arrange(() => _habitService.DeleteAsync(_habitViewModel)).Returns(Task.CompletedTask).OccursNever();

		//	// Act
		//	await _habitViewModel.RemoveCommand.ExecuteAsync(null);

		//	// Assert
		//	Mock.Assert(_habitService);
		//}

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
	}
}
