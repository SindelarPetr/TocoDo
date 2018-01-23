using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using TocoDo.Models;
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

			SetupProgressChart();

			Instance = this;

			Debug.WriteLine("---------- Finished calling of constructor of TodayPage");
		}

		private void SetupProgressChart()
		{
			//	var todayDayColor = Color.LimeGreen.ToSKColor();
			//	var workDayColor = SKColor.Parse("#CFD8DC");
			//	var weekendColor = ((Color)App.Current.Resources["ColorPrimaryLight"]).ToSKColor();//SKColor.Parse("#26A69A");
			
			var todayDayColor = Color.White.ToSKColor();
			var workDayColor = Color.White.ToSKColor();
			var weekendColor = Color.White.ToSKColor();

			var todayEntry = new Entry(4)
			{
				Label = "Sat",
				ValueLabel = "4",
				Color = weekendColor,
				TextColor = weekendColor
			};
			var entries = new[]
			{
				new Entry(2)
				{
					Label = "Sun",
					ValueLabel = "2",
					Color = weekendColor,
					TextColor = weekendColor
				},
				new Entry(6)
				{
					Label = "Mon",
					ValueLabel = "6",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Entry(7)
				{
					Label = "Tue",
					ValueLabel = "7",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Entry(10)
				{
					Label = "Wed",
					ValueLabel = "10",
					Color = todayDayColor,
					TextColor = todayDayColor
				},
				new Entry(8)
				{
					Label = "Thu",
					ValueLabel = "8",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Entry(13)
				{
					Label = "Fri",
					ValueLabel = "13",
					Color = workDayColor,
					TextColor = workDayColor
				},
				todayEntry,
			};

			var chart = new LineChart
			{
				LineAreaAlpha = 80,
				LineMode = LineMode.Straight,
				PointSize = 15,
				LabelTextSize = 24,
				LineSize = 7,
				Entries = entries,
				BackgroundColor = Color.Transparent.ToSKColor()
			};

			ChartProgress.Chart = chart;
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
			NewTaskView.IsVisible = true;
			NewTaskView.Focus();
			NewTaskView.DefaulDateTime = DateTime.Today;
		}
	}
}