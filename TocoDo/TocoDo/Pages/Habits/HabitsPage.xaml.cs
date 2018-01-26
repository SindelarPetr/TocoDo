
using System;
using System.Diagnostics;
using TocoDo.Services;
using TocoDo.ViewModels;
using TocoDo.Views.Habits;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitsPage : ContentPage
	{
		// TODO: Get rid of this static instance
		public static HabitsPage Instance { get; set; }

		public Func<HabitViewModel, HabitView> CreateHabitFunc { get; set; } = h => new HabitView(h);
		public HabitsPage()
		{
			Instance = this;
			InitializeComponent();
		}

		private void AddButton_OnTapped(object sender, EventArgs e)
		{
			Debug.WriteLine("---------- Button add a habit called.");
			var habit = new HabitViewModel();
			Debug.WriteLine("------------- Created a new habit and will add it to the habits list");
			StorageService.AddHabitToTheList(habit);
			Debug.WriteLine("---------- Finished call of Button Add a habit.");
		}
	}
}