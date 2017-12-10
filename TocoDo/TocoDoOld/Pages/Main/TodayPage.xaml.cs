using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System.Collections.Generic;
using TocoDo.Services;
using TocoDo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodayPage : ContentPage
	{
		public TodayPage()
		{

			List<DataPoint> points = new List<DataPoint>
			{
				new DataPoint(1, 1),
				new DataPoint(2, 3),
				new DataPoint(3, 2),
				new DataPoint(4, 4),
				new DataPoint(5, 5)
			};

			var leftAxis = new LinearAxis
			{
				Position = AxisPosition.Left,
				AbsoluteMinimum = 0,
				IsPanEnabled = false,
			};
			ApplyAxisStyle(leftAxis);

			var bottomAxis = new LinearAxis
			{
				Position = AxisPosition.Bottom,
			};
			ApplyAxisStyle(bottomAxis);

			var model = new PlotModel
			{
				PlotAreaBorderColor = Color.Transparent.ToOxyColor(),
				PlotAreaBorderThickness = new OxyThickness(0),
				//Title = "Progress",
				PlotType = PlotType.XY,
				LegendBorderThickness = 0,

			};
			model.Axes.Clear();
			model.Axes.Add(leftAxis);
			model.Axes.Add(bottomAxis);
			model.TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView;


			var series = new AreaSeries
			{
				Color = Color.Green.ToOxyColor(),
				ItemsSource = points,
				Selectable = false,
				LineStyle = LineStyle.Solid,
				MarkerType = MarkerType.Circle,
				//Color = OxyColors.White,
				//Color2 = OxyColors.Red,
				//MarkerSize = 5,
				//MarkerStrokeThickness = 5,
				MarkerFill = OxyColors.Green,
				MarkerStroke = OxyColors.Black,
				//StrokeThickness = 5,
				//Smooth = true,
				MarkerResolution = 1,

			};

			model.Series.Add(series);
			model.PlotAreaBorderColor = OxyColor.FromArgb(0, 0, 0, 0);

			InitializeComponent();

			LoadTodayTasks();

			ProductivityPlot.Model = model;
		}

		private void ApplyAxisStyle(LinearAxis axis)
		{
			axis.MinorGridlineThickness = 1;
			axis.MinorStep = 2;
			axis.MajorStep = 2;
			//axis.MinorGridlineColor = Color.GreenYellow.ToOxyColor();
			axis.AxislineColor = Color.SpringGreen.ToOxyColor();
			axis.MajorGridlineColor = Color.Transparent.ToOxyColor();
			//axis.TicklineColor = OxyColor.FromArgb(0, 100, 100, 100);
			axis.TextColor = Color.Transparent.ToOxyColor();

			axis.ExtraGridlineColor = Color.Yellow.ToOxyColor();
			axis.MinimumPadding = 0.1;
			axis.MaximumPadding = 0.1;

			axis.IsZoomEnabled = false;
		}

		private async void LoadTodayTasks()
		{
			var tasks = await TocodoService.GetTodayTasks();

			tasks.ForEach(t => LayoutTodayTasks.Children.Add(new TodoItemView(t)));
		}

	}
}