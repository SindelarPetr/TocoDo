
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Plugin.CrossPlatformTintedImage.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace TocoDo.Droid
{
	[Activity(Label = "Toco Do", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
			AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentOnUnhandledException;

			global::Xamarin.Forms.Forms.Init(this, bundle);
			OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

			TintedImageRenderer.Init();


			LoadApplication(new App());

			App.BarColorChanged += () =>
			{
				var color = (Color)App.Current.Resources["BarColor"];
				var multiply = 0.7;
				color = new Color(color.R * multiply, color.G * multiply, color.B * multiply);
				Window.SetStatusBarColor(color.ToAndroid());
			};
		}

		private void AndroidEnvironmentOnUnhandledException(object sender, RaiseThrowableEventArgs e)
		{
			Console.WriteLine("Catched an exception: " + e.Exception.Message);
		}

		private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{

			Console.WriteLine("Catched an exception: " + e.Exception);
		}

		private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine("Catched an exception: " + e);
		}
	}
}

