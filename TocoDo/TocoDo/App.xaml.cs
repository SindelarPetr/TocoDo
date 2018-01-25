using System;
using System.Diagnostics;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TocoDo.Pages.Main;
using TocoDo.Properties;
using Xamarin.Forms;

namespace TocoDo
{
	public partial class App : Application
	{
		public Color ColorPrimary
		{
			get => (Color)Resources["ColorPrimary"];
			set => Resources["ColorPrimary"] = value;
		}
		public Color ColorPrimaryLight
		{
			get => (Color)Resources["ColorPrimaryLight"];
			set => Resources["ColorPrimaryLight"] = value;
		}
		public Color ColorPrimaryDark
		{
			get => (Color)Resources["ColorPrimaryDark"];
			set => Resources["ColorPrimaryDark"] = value;
		}

		public static Action BarColorChanged;

		public App()
		{
			MyLogger.WriteStartMethod();
			InitializeComponent();

			var main = new MainPage();
			MyLogger.WriteInMethod();

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
