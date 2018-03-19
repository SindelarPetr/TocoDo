using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitsPastPage : ContentPage
	{
		public PastHabitsViewModel ViewModel { get; set; }

		public HabitsPastPage ()
		{
			ViewModel = new PastHabitsViewModel(((App)App.Current).HabitService, ((App)App.Current).Navigation);
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await ViewModel.LoadPastHabitsCommand.ExecuteAsync(null);
		}
	}
}