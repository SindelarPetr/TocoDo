using System;
using System.Diagnostics;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using TocoDo.Models;
using TocoDo.Popups;
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
			//var title = EntryTitle.Text.Trim();

			//if (string.IsNullOrWhiteSpace(title))
			//	return;

			//EntryTitle.Text = Habit.ModelTitle = title;
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			
		}

		private async void IconButton_OnClicked(object sender, EventArgs e)
		{
			try
			{
				var repeatTypePopup = new HabitRepeatTypePopup(Habit.ModelRepeatType, Habit.ModelDaysToRepeat);
				repeatTypePopup.Save += (repeatType, daysToRepeat) =>
				{
					Habit.ModelRepeatType = repeatType;
					Habit.ModelDaysToRepeat = daysToRepeat;
					
				};
				await PopupNavigation.Instance.PushAsync(repeatTypePopup);
			}
			catch (Exception ex)
			{
				MyLogger.WriteException(ex);
				throw;
			}
		}

		private void BtnUnit_Clicked(object sender, EventArgs e)
		{
			Habit.ModelHabitType = HabitType.Unit;
		}

		private void BtnDaylong_Clicked(object sender, EventArgs e)
		{
			Habit.ModelHabitType = HabitType.Daylong;
		}
	}
}