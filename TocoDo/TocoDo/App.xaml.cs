using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Services;
using TocoDo.UI.DependencyInjection;
using TocoDo.UI.Pages.Main;
using TocoDo.UI.Properties;
using Xamarin.Forms;

namespace TocoDo.UI
{
	public partial class App : Application
	{
		public static Action BarColorChanged;

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

		public StorageService Storage { get; set; }
		public NavigationService Navigation { get; set; }

		public App()
		{
			MyLogger.WriteStartMethod();
			InitializeComponent();

			var main = new MainPage();
			MyLogger.WriteInMethod();

			var navigationPage = new NavigationPage(main);
			//navigationPage.SetDynamicResource(NavigationPage.BarBackgroundColorProperty, "BarColor");
			MainPage = navigationPage;

			MyLogger.WriteEndMethod();
		}

		protected override void OnStart()
		{
			try
			{
				AppCenter.Start(string.Format(AppStrings.AppCenterMessage, AppStrings.AppCenterUwpSecret,
						AppStrings.AppCenterAndroidSecret, AppStrings.AppCenterIosSecret), typeof(Analytics), typeof(Crashes));

				Navigation = new NavigationService();
				var persistance = new PersistanceProvider();
				var modelFactory = new IModelFactory
				Storage = new StorageService(persistance, Navigation, );
			}
			catch (Exception e)
			{

				//throw;
			}
		}
	}
}
