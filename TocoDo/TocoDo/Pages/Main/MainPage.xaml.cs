using System.Diagnostics;
using Rg.Plugins.Popup.Extensions;
using TocoDo.Popups;
using TocoDo.Services;
using Xamarin.Forms;

namespace TocoDo.Pages.Main
{
	public partial class MainPage : MasterDetailPage
	{
		public TabbedPage TabbedPage { get; set; }
		private bool _isLoaded;

		public MainPage()
		{
			InitializeComponent();

			TabbedPage = ((TabbedPage)Detail);
		}

		protected override async void OnAppearing()
		{
			Debug.Write("------------------------ Called OnAppearing of MainPage");
			base.OnAppearing();

			if (!_isLoaded)
			{
				Debug.Write("------------------------ Isnt loaded yet - loading");
				await StorageService.Init();
				_isLoaded = true;
				Debug.Write("------------------------ finished loading");
			}
			Debug.Write("------------------------ Finished call of OnAppearing of MainPage");
		}
	}
}
