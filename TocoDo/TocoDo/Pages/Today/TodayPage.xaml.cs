using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using TocoDo.Models;
using TocoDo.Services;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodayPage : ContentPage
	{
		public static TodayPage Instance;

		private ObservableCollection<TaskModel> _todayTasks;
		public ObservableCollection<TaskModel> TodayTasks
		{
			get => _todayTasks;
			set
			{
				_todayTasks = value;
				OnPropertyChanged();
			}
		}

		private Action<DateTime> _globalDatePickerAction;

		#region Selected time
		public static BindableProperty SelectedTimeProperty = BindableProperty.Create("SelectedTime", typeof(DateTime), typeof(DateTime), DateTime.Now);

		public DateTime SelectedTime
		{
			get => (DateTime)GetValue(SelectedTimeProperty);
			set
			{
				SetValue(SelectedTimeProperty, value);
				_selectedTimeAction?.Invoke(value.TimeOfDay);
				_selectedTimeAction = null;
			}
		}

		private Action<TimeSpan> _selectedTimeAction;
		#endregion

		public TodayPage()
		{
			Debug.WriteLine("---------- Called constructor of TodayPage");

			InitializeComponent();

			Instance = this;

			Debug.WriteLine("---------- Finished calling of constructor of TodayPage");
		}
		#region Global date picker
		private void GlobalDatePicker_DateSelected(object sender, DateChangedEventArgs e)
		{
			_globalDatePickerAction?.Invoke(e.NewDate);
			_globalDatePickerAction = null;
		}

		public void ShowGlobalDatePicker(DateTime showDate, Action<DateTime> pickedAction, DateTime? minDate = null)
		{
			GlobalDatePicker.Date = showDate;
			GlobalDatePicker.MinimumDate = minDate ?? DateTime.Today;
			ShowGlobalDatePicker(pickedAction);
		}
		public void ShowGlobalDatePicker(Action<DateTime> pickedAction)
		{
			_globalDatePickerAction = pickedAction;
			GlobalDatePicker.Focus();
		}
		#endregion

		#region Global time picker
		public void ShowGlobalTimePicker(Action<TimeSpan> timePickedAction)
		{
			_selectedTimeAction = timePickedAction;
			GlobalTimePicker.Focus();
		}

		private void GlobalTimePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(GlobalTimePicker.Time))
			{
				SelectedTime = SelectedTime.Date + GlobalTimePicker.Time;
			}
		}
		#endregion

		private void ButtonAddToday_OnClicked(object sender, EventArgs e)
		{
			StorageService.AddTaskToTheList(new TaskViewModel { ScheduleDate = DateTime.Today });
		}
	}
}