using System.Linq;
using Microcharts;
using SkiaSharp.Views.Forms;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace TocoDo.UI.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitProgressPage : ContentPage
	{
		public HabitProgressPage(HabitViewModel habit)
		{
			ViewModel = habit;
			InitializeComponent();
			SetupProgressChart();
		}

		public HabitViewModel ViewModel { get; set; }

		private async void SetupProgressChart()
		{
			var color = Color.White.ToSKColor();

			var list = ViewModel.Filling.ToList();
			list.Sort((p1, p2) => p1.Key > p2.Key ? 1 :
				p1.Key == p2.Key                     ? 0 : -1);
			var entries = list.Select(p => new Entry(p.Value)
			{
				Color = color
			});

			var pointSize = list.Count < 20 ? 15 :
				list.Count < 40                ? 10 : 0;
			var chart = new LineChart
			{
				LineAreaAlpha   = 80,
				LineMode        = LineMode.Straight,
				PointSize       = pointSize,
				LabelTextSize   = 24,
				LineSize        = 5,
				Entries         = entries,
				BackgroundColor = Color.Transparent.ToSKColor()
			};

			ChartProgress.Chart = chart;
		}
	}
}