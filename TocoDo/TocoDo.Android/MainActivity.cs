﻿using Android.App;
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
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
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

			//App.BarColorChanged += () =>
			//{
			var colorPrimary = (Color)App.Current.Resources["ColorPrimary"];
			var colorPrimaryDark = (Color)App.Current.Resources["ColorPrimaryDark"];

			//	var multiply = 0.7;
			//color = new Color(color.R * multiply, color.G * multiply, color.B * multiply);
			Window.SetStatusBarColor(colorPrimaryDark.ToAndroid());
			//Window.SetNavigationBarColor(colorPrimary.ToAndroid());
			//};

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

			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			toolbar.SetBackgroundColor(colorPrimary.ToAndroid());
			SetSupportActionBar(toolbar);
		}
	}
}