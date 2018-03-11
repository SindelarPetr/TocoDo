using System;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Services;
using TocoDo.UI.Pages.Main;
using TocoDo.UI.Properties;
using Xamarin.Forms;

namespace TocoDo.UI
{
	public partial class App : Application
	{
		#region Static

		public static Action BarColorChanged;

		#endregion

		#region Properties

		public IPersistance       Persistance  => (IPersistance) Resources[AppStrings.Persistance];
		public IModelFactory      ModelFactory => (IModelFactory) Resources[AppStrings.ModelFactory];
		public IDateTimeProvider  DateProvider => (IDateTimeProvider) Resources[AppStrings.DateTimeProvider];
		public INavigationService Navigation   => (INavigationService) Resources[AppStrings.Navigation];
		public ITaskService       TaskService  => (ITaskService) Resources[AppStrings.TaskService];
		public IHabitService      HabitService => (IHabitService) Resources[AppStrings.HabitService];

		public Color ColorPrimary
		{
			get => (Color) Resources["ColorPrimary"];
			set => Resources["ColorPrimary"] = value;
		}

		public Color ColorPrimaryLight
		{
			get => (Color) Resources["ColorPrimaryLight"];
			set => Resources["ColorPrimaryLight"] = value;
		}

		public Color ColorPrimaryDark
		{
			get => (Color) Resources["ColorPrimaryDark"];
			set => Resources["ColorPrimaryDark"] = value;
		}

		#endregion

		public App()
		{
			MyLogger.WriteStartMethod();

			MyLogger.WriteLine("Before initialize component");
			InitializeComponent();
			MyLogger.WriteLine("After initialize component");

			var main = new MainPage();
			MyLogger.WriteInMethod();

			var navigationPage = new NavigationPage(main);
			MainPage           = navigationPage;

			MyLogger.WriteEndMethod();
		}


		protected override async void OnStart()
		{
			MyLogger.WriteStartMethod();
			try
			{
				//AppCenter.Start(string.Format(AppStrings.AppCenterMessage, AppStrings.AppCenterUwpSecret,
				//AppStrings.AppCenterAndroidSecret, AppStrings.AppCenterIosSecret), typeof(Analytics), typeof(Crashes));

				await Persistance.Init();
				await TaskService.LoadAsync();
				await HabitService.LoadAsync();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}

			MyLogger.WriteEndMethod();
		}
	}
}