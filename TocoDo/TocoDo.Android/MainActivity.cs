using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CrossPlatformTintedImage.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace TocoDo.Droid
{
	[Activity(Label = "TocoDo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			TintedImageRenderer.Init();

			var res = TocoDo.Resources.AreYouSureDescardChanges;

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());

			//App.BarColorChanged += () =>
			//{
			//	var color = (Color)App.Current.Resources["BarColor"];
			//	var multiply = 0.7;
			//	color = new Color(color.R * multiply, color.G * multiply, color.B * multiply);
			//	Window.SetStatusBarColor(color.ToAndroid());
			//};

			//if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			//{
			//	Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
			//	Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			//	Window.SetStatusBarColor(Android.Graphics.Color.Blue);
			//}

			if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
			{
				Window.AddFlags(WindowManagerFlags.LayoutNoLimits);
				Window.AddFlags(WindowManagerFlags.LayoutInScreen);
				Window.DecorView.SetFitsSystemWindows(true);
				var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				statusBarHeightInfo?.SetValue(this, 0);
			}
		}
	}
}

