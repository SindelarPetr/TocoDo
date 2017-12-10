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

			TabbedPage = (TabbedPage) Detail;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (!_isLoaded)
			{
				await StorageService.Init();
				_isLoaded = true;
			}
		}
	}
}
