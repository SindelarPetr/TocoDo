using System;
using TocoDo.Pages.Main;
using TocoDo.Services;
using Xamarin.Forms;

namespace TocoDo
{
	public partial class App : Application
	{
		public static Action BarColorChanged;
		private readonly TabbedPage _tabbed;

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
			switch (_tabbed.Children.IndexOf(_tabbed.CurrentPage))
			{
				case 0:
					colorToGo = (Color)Current.Resources["TasksPageColor"];
					break;
				case 1:
					colorToGo = (Color)Current.Resources["TodayPageColor"];
					break;
				default:
					colorToGo = (Color)Current.Resources["ChallengesPageColor"];
					break;
			}
			var originColor = (Color)Current.Resources["BarColor"];

			var animation = new Animation(n =>
			{
				Current.Resources["BarColor"] = new Color((colorToGo.R - originColor.R) * n + originColor.R,
					(colorToGo.G - originColor.G) * n + originColor.G, (colorToGo.B - originColor.B) * n + originColor.B);
				BarColorChanged?.Invoke();
			}, 0, 1, Easing.CubicOut);

			animation.Commit(MainPage, "BarColorChange");
		}

		protected override async void OnStart()
		{
			//await StorageService.Init();
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
