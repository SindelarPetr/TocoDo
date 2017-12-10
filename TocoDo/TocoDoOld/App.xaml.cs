using System;
using TocoDo.Pages.Main;
using Xamarin.Forms;

namespace TocoDo
{
	public partial class App : Application
	{
		public static Action BarColorChanged;

		private TabbedPage _tabbed;
		public App()
		{
			InitializeComponent();
			var main = new MainPage();
			_tabbed = main.TabbedPage;
			var page = new NavigationPage(main)
			{
				Title = "Toco Do"
			};


			page.SetDynamicResource(NavigationPage.BarBackgroundColorProperty, "BarColor");

			_tabbed.CurrentPageChanged += MainTabbedPage_OnCurrentPageChanged;

			MainPage = page;
			MainTabbedPage_OnCurrentPageChanged(null, null);
		}
		private void MainTabbedPage_OnCurrentPageChanged(object sender, EventArgs e)
		{
			Color colorToGo;
			double colorValue = 0.25;
			switch (_tabbed.Children.IndexOf(_tabbed.CurrentPage))
			{
				case 0:
					colorToGo = new Color(colorValue, 0, 0);
					break;
				case 1:
					colorToGo = new Color(0, colorValue, 0);
					break;
				case 2:
				default:
					colorToGo = new Color(0, 0, colorValue);
					break;
			}
			var originColor = (Color)App.Current.Resources["BarColor"];

			var animation = new Animation(n =>
			{
				App.Current.Resources["BarColor"] = new Color((colorToGo.R - originColor.R) * n + originColor.R,
					(colorToGo.G - originColor.G) * n + originColor.G, (colorToGo.B - originColor.B) * n + originColor.B);
				BarColorChanged?.Invoke();
			}, 0, 1, Easing.CubicOut);

			animation.Commit(MainPage, "BarColorChange");
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
