using System;
using System.Diagnostics;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyHabitPage : ContentPage
	{
		public HabitViewModel Habit
		{
			get => (HabitViewModel)BindingContext;
			set => BindingContext = value;
		}

		[Obsolete]
		public ModifyHabitPage()
		{
			Habit = new HabitViewModel(new Models.HabitModel
			{
				Id = 1,
				Description = "This is description of a habit.",
				Title = "Habit title.",
				HabitType = Models.HabitType.Daylong,
				RepeatsADay = 2,
				RepeatType = Models.RepeatType.Mon,
				StartDate = DateTime.Today + TimeSpan.FromDays(3),
				DailyFillingCount = 3
			});
			InitializeComponent();
		}

		public ModifyHabitPage(HabitViewModel habit)
		{
			try
			{
				Habit = habit;
				InitializeComponent();
			}
			catch (Exception e)
			{
				Debug.WriteLine($"---------- Exception thrown in ModifyHabitPage constructor: {e.Message} <<--- with following stack trace: {Environment.NewLine} {e.StackTrace}");
			}
		}

		private void EntryTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			var title = EntryTitle.Text.Trim();

			if (string.IsNullOrWhiteSpace(title))
				return;

			EntryTitle.Text = Habit.ModelTitle = title;
		}
	}
}