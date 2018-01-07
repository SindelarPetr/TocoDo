using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TocoDo.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

			//#region Plot
			//List<DataPoint> points = new List<DataPoint>
			//{
			//	new DataPoint(1, 1),
			//	new DataPoint(2, 3),
			//	new DataPoint(3, 2),
			//	new DataPoint(4, 4),
			//	new DataPoint(5, 5)
			//};

			//var leftAxis = new LinearAxis
			//{
			//	Position = AxisPosition.Left,
			//	AbsoluteMinimum = 0,
			//	IsPanEnabled = false,
			//};
			//ApplyAxisStyle(leftAxis);

			//var bottomAxis = new LinearAxis
			//{
			//	Position = AxisPosition.Bottom,
			//};
			//ApplyAxisStyle(bottomAxis);

			//var model = new PlotModel
			//{
			//	PlotAreaBorderColor = Color.Transparent.ToOxyColor(),
			//	PlotAreaBorderThickness = new OxyThickness(0),
			//	//Title = "Progress",
			//	PlotType = PlotType.XY,
			//	LegendBorderThickness = 0,

			//};
			//model.Axes.Clear();
			//model.Axes.Add(leftAxis);
			//model.Axes.Add(bottomAxis);
			//model.TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView;


			//var series = new AreaSeries
			//{
			//	Color = Color.Green.ToOxyColor(),
			//	ItemsSource = points,
			//	Selectable = false,
			//	LineStyle = LineStyle.Solid,
			//	MarkerType = MarkerType.Circle,
			//	//Color = OxyColors.White,
			//	//Color2 = OxyColors.Red,
			//	//MarkerSize = 5,
			//	//MarkerStrokeThickness = 5,
			//	MarkerFill = OxyColors.Green,
			//	MarkerStroke = OxyColors.Black,
			//	//StrokeThickness = 5,
			//	//Smooth = true,
			//	MarkerResolution = 1,

			//};

			//model.Series.Add(series);
			//model.PlotAreaBorderColor = OxyColor.FromArgb(0, 0, 0, 0);
			//#endregion

			InitializeComponent();

			Instance = this;

			//ProductivityPlot.Model = model;
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
			NewTaskView.IsVisible = true;
			NewTaskView.Focus();
			NewTaskView.DefaulDateTime = DateTime.Today;
		}
	}
}