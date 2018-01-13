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

			var workDayColor = SKColor.Parse("#CFD8DC");
			var weekendColor = ((Color)App.Current.Resources["ColorPrimaryLight"]).ToSKColor();//SKColor.Parse("#26A69A");
			var entries = new[]
			{
				new Entry(5)
				{
					Label = "Sun",
					ValueLabel = "1",
					Color = weekendColor,
					TextColor = weekendColor
				},
				new Entry(5)
				{
					Label = "Mon",
					ValueLabel = "5",
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
					ValueLabel = "3",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Entry(8)
				{
					Label = "Thu",
					ValueLabel = "3",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Entry(13)
				{
					Label = "Fri",
					ValueLabel = "3",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Entry(4)
				{
					Label = "Sat",
					ValueLabel = "3",
					Color = weekendColor,
					TextColor = weekendColor
				},
			};

			var chart = new LineChart();
			chart.LineAreaAlpha = 80;
			chart.LineMode = LineMode.Spline;
			chart.PointSize = 25;
			chart.LabelTextSize = 24;
			chart.LineSize = 15;
			chart.Entries = entries;
			chart.BackgroundColor = Color.Transparent.ToSKColor();
			ChartProgress.Chart = chart;

			Instance = this;

			Debug.WriteLine("---------- Finished calling of constructor of TodayPage");
		}

		#region Global date picker
		private void GlobalDatePicker_DateSelected(object sender, DateChangedEventArgs e)
		{
			_globalDatePickerAction?.Invoke(e.NewDate);
			_globalDatePickerAction = null;


		}

		public void ShowGlobalDatePicker(DateTime showDate, Action<DateTime> pickedAction)
		{
			//GlobalDatePicker.Date = showDate;
			ShowGlobalDatePicker(pickedAction);
		}
		public void ShowGlobalDatePicker(Action<DateTime> pickedAction)
		{
			_globalDatePickerAction = pickedAction;
			//GlobalDatePicker.Focus();
		}
		#endregion

		#region Global time picker
		public void ShowGlobalTimePicker(Action<TimeSpan> timePickedAction)
		{
			_selectedTimeAction = timePickedAction;
			//GlobalTimePicker.Focus();
		}

		private void GlobalTimePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//if (e.PropertyName == nameof(GlobalTimePicker.Time))
			//{
			//	SelectedTime = SelectedTime.Date + GlobalTimePicker.Time;
			//}
		}
		#endregion

		//private void ApplyAxisStyle(LinearAxis axis)
		//{
		//	axis.MinorGridlineThickness = 1;
		//	axis.MinorStep = 2;
		//	axis.MajorStep = 2;
		//	//axis.MinorGridlineColor = Color.GreenYellow.ToOxyColor();
		//	axis.AxislineColor = Color.SpringGreen.ToOxyColor();
		//	axis.MajorGridlineColor = Color.Transparent.ToOxyColor();
		//	//axis.TicklineColor = OxyColor.FromArgb(0, 100, 100, 100);
		//	axis.TextColor = Color.Transparent.ToOxyColor();

		//	axis.ExtraGridlineColor = Color.Yellow.ToOxyColor();
		//	axis.MinimumPadding = 0.1;
		//	axis.MaximumPadding = 0.1;

		//	axis.IsZoomEnabled = false;
		//}

		private void ButtonAddToday_OnClicked(object sender, EventArgs e)
		{
			//NewTaskView.IsVisible = true;
			//NewTaskView.Focus();
			//NewTaskView.DefaulDateTime = DateTime.Today;
		}
	}
}