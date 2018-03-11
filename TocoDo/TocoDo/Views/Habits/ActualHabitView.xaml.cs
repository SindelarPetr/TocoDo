using System;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActualHabitView : BaseHabitView
	{
		public ActualHabitView(IHabitViewModel model) : base(model)
		{
			InitializeComponent();
		}

		public override void FocusEntry()
		{
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			if (ViewModel.HabitType == HabitType.Unit && ViewModel.RepeatsToday >= ViewModel.MaxRepeatsADay)
			{
				ViewModel.RepeatsToday %= ViewModel.MaxRepeatsADay + 1;
				((Button) sender).IsEnabled =  false;
			}
		}
	}
}