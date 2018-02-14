using System;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActualHabitView : ContentView, IEntryFocusable<HabitViewModel>
	{
		public HabitViewModel ViewModel { get; set; }

		public ActualHabitView (HabitViewModel model)
		{
			ViewModel = model;
			InitializeComponent ();
		}

		public void FocusEntry() { }

		private void Button_OnClicked(object sender, EventArgs e)
		{
			if (ViewModel.ModelHabitType == HabitType.Unit && ViewModel.ModelRepeatsToday >= ViewModel.ModelMaxRepeatsADay)
			{
				ViewModel.ModelRepeatsToday %= ViewModel.ModelMaxRepeatsADay + 1;
				((Button) sender).IsEnabled = false;
			}
		}
	}
}