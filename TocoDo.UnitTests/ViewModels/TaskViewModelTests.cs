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
	public class ITaskViewModelTests
	{
		private ITaskService _taskService;
		private INavigationService _navigationService;
		private ITaskViewModel _ITaskViewModel;

		[TestInitialize]
		public void TestInit()
		{
			_taskService = Mock.Create<ITaskService>();
			_navigationService = Mock.Create<INavigationService>();
			_ITaskViewModel = new TaskViewModel(_taskService, _navigationService);
		}

		[TestMethod]
		public async Task EditCommandShouldNavigateToATaskModifyPage()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.PushAsync(PageType.ModifyTaskPage, Arg.IsAny<ITaskViewModel>()))
			    .Returns(Task.CompletedTask).OccursOnce();

			// Act
			await _ITaskViewModel.EditCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_navigationService);
		}

		[TestMethod]
		public void EditTitleCommandShouldModifyTheTitleIfASimpleTitleIsPassed()
		{
			// Arrange
			var oldTitle = "Tit";
			var expectedTitle = "TITLE";
			_ITaskViewModel.Title = oldTitle;

			// Act
			_ITaskViewModel.EditTitleCommand.Execute(expectedTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _ITaskViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldModifyTheTitleAndTrimTheValueIfATitleIsPassed()
		{
			// Arrange
			var oldTitle = "Tit";
			var newTitle = "   TITLE";
			var expectedTitle = "TITLE";
			_ITaskViewModel.Title = oldTitle;

			// Act
			_ITaskViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _ITaskViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldntDoAnythingIfTheNewTitleIsWhiteSpace()
		{
			// Arrange
			var oldTitle         = "Tit";
			var newTitle         = "   ";
			var expectedTitle    = "Tit";
			_ITaskViewModel.Title = oldTitle;

			// Act
			_ITaskViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _ITaskViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldntDoAnythingIfTheNewTitleIsNotAString()
		{
			// Arrange
			var oldTitle         = "Tit";
			var newTitle         = new Mock();
			var expectedTitle    = "Tit";
			_ITaskViewModel.Title = oldTitle;
			Mock.Arrange(() => _taskService.UpdateAsync(_ITaskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_ITaskViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _ITaskViewModel.Title);
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void ConfirmFinishCommandShouldCallConfirmCreationOfTaskServiceAndWontCallUpdateWhenANotNullOrWhitespaceTitleIsProvided()
		{
			// Arrange
			string finalTitle = "TITLE";
			Mock.Arrange(() => _taskService.ConfirmCreationAsync(_ITaskViewModel)).Returns(Task.CompletedTask).OccursOnce();
			Mock.Arrange(() => _taskService.UpdateAsync(_ITaskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_ITaskViewModel.FinishCreationCommand.Execute(finalTitle);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void ConfirmFinishCommandShouldCallCancelCreationOfTaskServiceAndWontCallUpdateWhenANullTitleIsProvided()
		{
			// Arrange
			string finalTitle = null;
			Mock.Arrange(() => _taskService.CancelCreation(_ITaskViewModel)).OccursOnce();
			Mock.Arrange(() => _taskService.UpdateAsync(_ITaskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_ITaskViewModel.FinishCreationCommand.Execute(finalTitle);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void ConfirmFinishCommandShouldCallCancelCreationOfTaskServiceAndWontCallUpdateWhenAWhitespaceTitleIsProvided()
		{
			// Arrange
			string finalTitle = "  ";
			Mock.Arrange(() => _taskService.CancelCreation(_ITaskViewModel)).OccursOnce();
			Mock.Arrange(() => _taskService.UpdateAsync(_ITaskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_ITaskViewModel.FinishCreationCommand.Execute(finalTitle);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public async Task RemoveCommandShouldRiseAnAlert()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).OccursOnce();

			// Act
			await _ITaskViewModel.RemoveCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_navigationService);
		}

		[TestMethod]
		public async Task RemoveCommandAfterAcceptingAlertShouldCallDeleteAsyncOfTaskService()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString,Arg.AnyString,Arg.AnyString,Arg.AnyString)).Returns(Task.FromResult(true)).InOrder();
			Mock.Arrange(() => _taskService.DeleteAsync(_ITaskViewModel)).Returns(Task.CompletedTask).InOrder().OccursOnce();

			// Act
			await _ITaskViewModel.RemoveCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public async Task RemoveCommandAfterRefusingTheAlertShouldntCallDeleteAsync()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(false)).InOrder();
			Mock.Arrange(() => _taskService.DeleteAsync(_ITaskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			await _ITaskViewModel.RemoveCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public async Task UpdateCommandShouldCallUpdateAsyncOfTaskService()
		{
			// Arrange
			Mock.Arrange(() => _taskService.UpdateAsync(_ITaskViewModel)).Returns(Task.CompletedTask).OccursOnce();

			// Act
			await _ITaskViewModel.UpdateCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_taskService);
		}
	}
}
