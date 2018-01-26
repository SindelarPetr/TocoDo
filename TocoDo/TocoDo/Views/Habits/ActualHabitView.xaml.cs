using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TocoDo.Models;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views.Habits
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
			if (ViewModel.ModelHabitType == HabitType.Unit && ViewModel.ModelDailyFillingCount >= ViewModel.ModelRepeatsADay)
			{
				ViewModel.ModelDailyFillingCount %= ViewModel.ModelRepeatsADay + 1;
				((Button) sender).IsEnabled = false;
			}
		}
	}
}