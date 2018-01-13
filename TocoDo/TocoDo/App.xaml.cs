using System;
using System.Diagnostics;
using TocoDo.Pages.Main;
using Xamarin.Forms;

namespace TocoDo
{
	public partial class App : Application
	{
		public static Action BarColorChanged;
		private readonly TabbedPage _tabbed;

		public App()
		{
			Debug.WriteLine("--- Called App constructor");
			InitializeComponent();

			Debug.WriteLine("----- Creating MainPage");
			var main = new MainPage();
			Debug.WriteLine("----- Finished creating MainPage");

			_tabbed = main.TabbedPage;

			var navigationPage = new NavigationPage(main);

			navigationPage.SetDynamicResource(NavigationPage.BarBackgroundColorProperty, "BarColor");

			Debug.WriteLine("----- Setting MainPage");
			MainPage = navigationPage;

			Debug.WriteLine("----- Finished setting MainPage");
			
			Debug.WriteLine("--- Finished calling of App constructor");
		}


	}
}
