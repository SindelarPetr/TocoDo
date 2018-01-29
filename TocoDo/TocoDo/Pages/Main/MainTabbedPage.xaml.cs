
using TocoDo.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainTabbedPage : TabbedPage
	{
		public static MainTabbedPage Instance { get; set; }

		private SemaphorDisabler _swipePagingDisabler;

		private bool _isLoaded;

		public MainTabbedPage()
		{
			InitializeComponent();
			
			Instance = this;

			if (Children.Count >= 2)
				CurrentPage = Children[1];
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (!_isLoaded)
			{
				_swipePagingDisabler = new SemaphorDisabler(() => Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.EnableSwipePaging(
					  On<Xamarin.Forms.PlatformConfiguration.Android>()),
					  () => Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.DisableSwipePaging(
						  On<Xamarin.Forms.PlatformConfiguration.Android>()));
				_isLoaded = true;
			}
		}

		public void DisableSwipePagging()
		{
			_swipePagingDisabler.Disable();
		}

		public void EnableSwipePagging()
		{
			_swipePagingDisabler.Enable();
		}
	}
}