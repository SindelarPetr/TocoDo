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
	public class TaskViewModelTests
	{
		private ITaskService _taskService;
		private INavigationService _navigationService;
		private ITaskViewModel _taskViewModel;

		[TestInitialize]
		public void TestInit()
		{
			_taskService = Mock.Create<ITaskService>();
			_navigationService = Mock.Create<INavigationService>();
			_taskViewModel = new TaskViewModel(_taskService, _navigationService);
		}

		[TestMethod]
		public void EditCommandShouldNavigateToATaskModifyPage()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.PushAsync(PageType.ModifyTaskPage, Arg.IsAny<ITaskViewModel>()))
			    .Returns(Task.CompletedTask).OccursOnce();

			// Act
			_taskViewModel.EditCommand.Execute(null);

			// Assert
			Mock.Assert(_navigationService);
		}

		[TestMethod]
		public void EditTitleCommandShouldModifyTheTitleIfASimpleTitleIsPassed()
		{
			// Arrange
			var oldTitle = "Tit";
			var expectedTitle = "TITLE";
			_taskViewModel.Title = oldTitle;

			// Act
			_taskViewModel.EditTitleCommand.Execute(expectedTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _taskViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldModifyTheTitleAndTrimTheValueIfATitleIsPassed()
		{
			// Arrange
			var oldTitle = "Tit";
			var newTitle = "   TITLE";
			var expectedTitle = "TITLE";
			_taskViewModel.Title = oldTitle;

			// Act
			_taskViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _taskViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldntDoAnythingIfTheNewTitleIsWhiteSpace()
		{
			// Arrange
			var oldTitle         = "Tit";
			var newTitle         = "   ";
			var expectedTitle    = "Tit";
			_taskViewModel.Title = oldTitle;

			// Act
			_taskViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _taskViewModel.Title);
		}

		[TestMethod]
		public void EditTitleCommandShouldntDoAnythingIfTheNewTitleIsNotAString()
		{
			// Arrange
			var oldTitle         = "Tit";
			var newTitle         = new Mock();
			var expectedTitle    = "Tit";
			_taskViewModel.Title = oldTitle;
			Mock.Arrange(() => _taskService.UpdateAsync(_taskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_taskViewModel.EditTitleCommand.Execute(newTitle);

			// Assert
			Assert.AreEqual(expectedTitle, _taskViewModel.Title);
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void ConfirmFinishCommandShouldCallConfirmCreationOfTaskServiceAndWontCallUpdateWhenANotNullOrWhitespaceTitleIsProvided()
		{
			// Arrange
			string finalTitle = "TITLE";
			Mock.Arrange(() => _taskService.ConfirmCreationAsync(_taskViewModel)).Returns(Task.CompletedTask).OccursOnce();
			Mock.Arrange(() => _taskService.UpdateAsync(_taskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_taskViewModel.FinishCreationCommand.Execute(finalTitle);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void ConfirmFinishCommandShouldCallCancelCreationOfTaskServiceAndWontCallUpdateWhenANullTitleIsProvided()
		{
			// Arrange
			string finalTitle = null;
			Mock.Arrange(() => _taskService.CancelCreation(_taskViewModel)).OccursOnce();
			Mock.Arrange(() => _taskService.UpdateAsync(_taskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_taskViewModel.FinishCreationCommand.Execute(finalTitle);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void ConfirmFinishCommandShouldCallCancelCreationOfTaskServiceAndWontCallUpdateWhenAWhitespaceTitleIsProvided()
		{
			// Arrange
			string finalTitle = "  ";
			Mock.Arrange(() => _taskService.CancelCreation(_taskViewModel)).OccursOnce();
			Mock.Arrange(() => _taskService.UpdateAsync(_taskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_taskViewModel.FinishCreationCommand.Execute(finalTitle);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void RemoveCommandShouldRiseAnAlert()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).OccursOnce();

			// Act
			_taskViewModel.RemoveCommand.Execute(null);

			// Assert
			Mock.Assert(_navigationService);
		}

		[TestMethod]
		public void RemoveCommandAfterAcceptingAlertShouldCallDeleteAsyncOfTaskService()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString,Arg.AnyString,Arg.AnyString,Arg.AnyString)).Returns(Task.FromResult(true)).InOrder();
			Mock.Arrange(() => _taskService.DeleteAsync(_taskViewModel)).Returns(Task.CompletedTask).InOrder().OccursOnce();

			// Act
			_taskViewModel.RemoveCommand.Execute(null);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void RemoveCommandAfterRefusingTheAlertShouldntCallDeleteAsync()
		{
			// Arrange
			Mock.Arrange(() => _navigationService.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(false)).InOrder();
			Mock.Arrange(() => _taskService.DeleteAsync(_taskViewModel)).Returns(Task.CompletedTask).OccursNever();

			// Act
			_taskViewModel.RemoveCommand.Execute(null);

			// Assert
			Mock.Assert(_taskService);
		}

		[TestMethod]
		public void UpdateCommandShouldCallUpdateAsyncOfTaskService()
		{
			// Arrange
			Mock.Arrange(() => _taskService.UpdateAsync(_taskViewModel)).Returns(Task.CompletedTask).OccursOnce();

			// Act
			_taskViewModel.UpdateCommand.Execute(null);

			// Assert
			Mock.Assert(_taskService);
		}
	}
}
