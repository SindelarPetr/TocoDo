
using System;
using TocoDo.Services;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitsPage : ContentPage
	{
		public HabitsPage()
		{
			InitializeComponent();
		}

		private void AddButton_OnTapped(object sender, EventArgs e)
		{
			var habit = new HabitViewModel();
			StorageService.AddHabitToTheList(habit);
		}
	}
}