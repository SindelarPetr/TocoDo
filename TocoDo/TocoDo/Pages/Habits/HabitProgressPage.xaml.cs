using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp.Views.Forms;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace TocoDo.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitProgressPage : ContentPage
	{
		public HabitViewModel ViewModel { get; set; }
		public HabitProgressPage (HabitViewModel habit)
		{
			ViewModel = habit;
			InitializeComponent ();
			SetupProgressChart();
		}

		private void SetupProgressChart()
		{
			var todayDayColor = Color.White.ToSKColor();
			var workDayColor = Color.White.ToSKColor();
			var weekendColor = Color.White.ToSKColor();

			var todayEntry = new Microcharts.Entry(4)
			{
				Label = "Sat",
				//ValueLabel = "4",
				Color = weekendColor,
				TextColor = weekendColor
			};
			var entries = new[]
			{
				new Microcharts.Entry(0)
				{
					Label = "Sun",
					//ValueLabel = "2",
					Color = weekendColor,
					TextColor = weekendColor
				},
				new Microcharts.Entry(6)
				{
					Label = "Mon",
					//ValueLabel = "6",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Microcharts.Entry(7)
				{
					Label = "Tue",
					//ValueLabel = "7",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Microcharts.Entry(10)
				{
					Label = "Wed",
					//ValueLabel = "10",
					Color = todayDayColor,
					TextColor = todayDayColor
				},
				new Microcharts.Entry(8)
				{
					Label = "Thu",
					//ValueLabel = "8",
					Color = workDayColor,
					TextColor = workDayColor
				},
				new Entry(13)
				{
					Label = "Fri",
					//ValueLabel = "13",
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
				LineSize = 5,
				Entries = entries,
				BackgroundColor = Color.Transparent.ToSKColor(),
			};

			ChartProgress.Chart = chart;
		}
	}
}