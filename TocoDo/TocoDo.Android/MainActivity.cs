using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Plugin.CrossPlatformTintedImage.Android;
using Rg.Plugins.Popup;
using TocoDo.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace TocoDo.Droid
{
	[Activity(Label       = "TocoDo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize)]
	public class MainActivity : FormsAppCompatActivity
	{
		private Toolbar _toolbar;

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource   = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Popup.Init(this, bundle);
			TintedImageRenderer.Init();
			Forms.Init(this, bundle);

			var app = new App();
			LoadApplication(app);
			
			var colorPrimaryDark = (Color)App.Current.Resources["ColorPrimaryDark"];
			
			Window.SetStatusBarColor(colorPrimaryDark.ToAndroid());

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				//Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
				//Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
				//Window.SetStatusBarColor(Android.Graphics.Color.Blue);
			}

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				//Window.AddFlags(WindowManagerFlags.LayoutNoLimits);
				//Window.AddFlags(WindowManagerFlags.LayoutInScreen);
				//Window.DecorView.SetFitsSystemWindows(true);
				//var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

				//statusBarHeightInfo?.SetValue(this, 0);

				//Toolbar myToolbar = (Toolbar)FindViewById(ToolbarResource);
			}

			_toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(_toolbar);
		}

		protected override void OnResume()
		{
			base.OnResume();

			var colorPrimary = (Color)App.Current.Resources["ColorPrimary"];

			_toolbar.SetBackgroundColor(colorPrimary.ToAndroid());
		}


		
	}
}