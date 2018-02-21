using System;
using Rg.Plugins.Popup.Services;
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
		public ModifyHabitPage(HabitViewModel habit)
		{
			try
			{
				Habit = habit;
				InitializeComponent();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}
		}

		public HabitViewModel Habit
		{
			get => (HabitViewModel) BindingContext;
			set => BindingContext = value;
		}

		private void EntryTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			Habit.EditTitleCommand?.Execute(EntryTitle.Text);
			EntryTitle.Text = Habit.Title;
		}

		private async void IconButton_OnClicked(object sender, EventArgs e)
		{
			try
			{
				var repeatTypePopup  = new HabitRepeatTypePopup(Habit.RepeatType, Habit.DaysToRepeat);
				repeatTypePopup.Save += (repeatType, daysToRepeat) =>
				{
					Habit.RepeatType   = repeatType;
					Habit.DaysToRepeat = daysToRepeat;
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
			Habit.HabitType = HabitType.Unit;
		}

		private void BtnDaylong_Clicked(object sender, EventArgs e)
		{
			Habit.HabitType = HabitType.Daylong;
		}
	}
}