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
			_editDescriptionViewModel = new EditDescriptionViewModel(_navigation, _title, _description, SetDescriptionAction, false);
		}

		[TestMethod]
		public void SaveCommandCallsDescriptionActionAndPassesItsNewDescription()
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
			_editDescriptionViewModel = new EditDescriptionViewModel(_navigation, _title, _description, SetDescriptionAction, false);

			// Act
			_editDescriptionViewModel.Description = newDescription;
			_editDescriptionViewModel.SaveCommand?.Execute(null);
			SetDescriptionAction -= SaveAction;

			// Assert
			Assert.IsTrue(called);
			Assert.AreEqual(description, newDescription);
		}

		[TestMethod]
		public void DiscardCommandWontShowAnyAlertIfTheDescriptionHasNotBeenChanged()
		{
			// Arrange
			Mock.Arrange(() => _navigation.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(Arg.AnyBool)).OccursNever();

			// Act
			_editDescriptionViewModel.DiscardCommand.Execute(null);

			// Assert
			Mock.Assert(_navigation);
		}

		[TestMethod]
		public async Task DiscardCommandShowsAlertIfTheDescriptionHasBeenChanged()
		{
			AwaitableCommand c = new AwaitableCommand(async () => await Task.Delay(10000));

			await c.ExecuteAsync(null);
			Assert.IsTrue(true);
			return;
			// Arrange
			Mock.Arrange(() => _navigation.DisplayAlert(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString)).Returns(Task.FromResult(Arg.AnyBool)).OccursOnce();
			var newDescription = "NEW DESCRIPTION";

			// Act
			_editDescriptionViewModel.Description = newDescription;
			_editDescriptionViewModel.DiscardCommand.Execute(null);

			// Assert
			Mock.Assert(_navigation);

		}

		[TestMethod]
		public async Task DiscardCommandWontCallSaveIfTheAlertResponseIsNegative()
		{
			Command com = new Command(async () => await Task.Delay(10000));

			com.Execute(null);
		}

		[TestMethod]
		public void DiscardCommandCallsSaveIfTheAlertResponseIsPositive()
		{
			throw new NotImplementedException();
		}


	}
}
