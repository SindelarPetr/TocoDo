using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.UnitTests.ViewModels
{
	[TestClass]
	public class EditDescriptionViewModelTests
	{
		private INavigationService _navigation;
		private string _title;
		private string _description;
		private event Action<string> SetDescriptionAction;
		private EditDescriptionViewModel _editDescriptionViewModel;

		[TestInitialize]
		public void TestInit()
		{
			_navigation = Mock.Create<INavigationService>();
			_title = "TITLE";
			_description = "DESCRIPTION";
			SetDescriptionAction = d => _description = d;
			_editDescriptionViewModel = new EditDescriptionViewModel(_navigation, new EditDescriptionInfo(_title, _description, SetDescriptionAction, false));
		}

		[TestMethod]
		public async Task SaveCommandCallsDescriptionActionAndPassesItsNewDescription()
		{
			// Arrange
			bool called = false;
			string description = "";
			string newDescription = "NEW DESCRIPTION";
			void SaveAction(string s)
			{
				called = true;
				description = s;
			}
			SetDescriptionAction += SaveAction;
			_editDescriptionViewModel = new EditDescriptionViewModel(_navigation, new EditDescriptionInfo(_title, _description, SetDescriptionAction, false));

			// Act
			_editDescriptionViewModel.Description = newDescription;
			await _editDescriptionViewModel.SaveCommand.ExecuteAsync(null);
			SetDescriptionAction -= SaveAction;

			// Assert
			Assert.IsTrue(called);
			Assert.AreEqual(description, newDescription);
		}

		[TestMethod]
		public async Task DiscardCommandWontShowAnyAlertIfTheDescriptionHasNotBeenChanged()
		{
			// Arrange
			Mock.Arrange(() => _navigation.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(Arg.AnyBool)).OccursNever();

			// Act
			await _editDescriptionViewModel.DiscardCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_navigation);
		}

		[TestMethod]
		public async Task DiscardCommandShowsAlertIfTheDescriptionHasBeenChanged()
		{
			// Arrange
			Mock.Arrange(() => _navigation.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(Arg.AnyBool)).OccursOnce();
			var newDescription = "NEW DESCRIPTION";

			// Act
			_editDescriptionViewModel.Description = newDescription;
			await _editDescriptionViewModel.DiscardCommand.ExecuteAsync(null);

			// Assert
			Mock.Assert(_navigation);

		}

		[TestMethod]
		public async Task DiscardCommandWontCallSaveIfTheAlertResponseIsNegative()
		{
			// Arrange
			bool called = false;
			void SaveActionWhichShouldntBeCalled(string str)
			{
				called = true;
			}
			Mock.Arrange(() => _navigation.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(false));
			_editDescriptionViewModel = new EditDescriptionViewModel(_navigation, new EditDescriptionInfo(_title, _description, SaveActionWhichShouldntBeCalled, false));

			// Act
			await _editDescriptionViewModel.DiscardCommand.ExecuteAsync(null);

			// Assert
			Assert.IsFalse(called);
		}

		[TestMethod]
		public void DiscardCommandCallsSaveIfTheAlertResponseIsPositive()
		{
			throw new NotImplementedException();
		}


	}
}
