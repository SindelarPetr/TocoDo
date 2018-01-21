using System;
using System.Diagnostics;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TocoDo.Pages.Main;
using TocoDo.Properties;
using Xamarin.Forms;

namespace TocoDo
{
	public partial class App : Application
	{
		public static Action BarColorChanged;
		private readonly TabbedPage _tabbed;

		public App()
		{
			MyLogger.WriteStartMethod();
			InitializeComponent();
			
			var main = new MainPage();
			MyLogger.WriteInMethod();

			_tabbed = main.TabbedPage;

			var navigationPage = new NavigationPage(main);

			navigationPage.SetDynamicResource(NavigationPage.BarBackgroundColorProperty, "BarColor");

			MainPage = navigationPage;

			MyLogger.WriteEndMethod();
		}

		protected override void OnStart()
		{
			try
			{
				AppCenter.Start(string.Format(AppStrings.AppCenterMessage, AppStrings.AppCenterUwpSecret,
						AppStrings.AppCenterAndroidSecret, AppStrings.AppCenterIosSecret), typeof(Analytics), typeof(Crashes));
			}
			catch (Exception e)
			{

				throw;
			}
		}
	}
}
