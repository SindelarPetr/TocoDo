using System;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyHabitPage : ContentPage
	{
		#region Properties

		public ModifyHabitViewModel ViewModel { get; set; }

		#endregion

		public ModifyHabitPage(HabitViewModel habit)
		{
			MyLogger.WriteStartMethod();
			try
			{
				ViewModel = new ModifyHabitViewModel(((App)App.Current).HabitService, ((App)App.Current).Navigation, habit);
				InitializeComponent();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}

			MyLogger.WriteEndMethod();
		}

		private void EntryTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			ViewModel.EditTitleCommand.Execute(EntryTitle.Text);
		}
	}
}