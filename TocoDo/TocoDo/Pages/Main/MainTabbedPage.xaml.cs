using TocoDo.BusinessLogic.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainTabbedPage : TabbedPage
	{
		private bool _isLoaded;

		private SemaphorDisabler _swipePagingDisabler;

		public MainTabbedPage()
		{
			InitializeComponent();

			Instance = this;

			if (Children.Count >= 2)
				CurrentPage = Children[1];
		}

		public static MainTabbedPage Instance { get; set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (!_isLoaded)
			{
				_swipePagingDisabler = new SemaphorDisabler(() =>
						Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.EnableSwipePaging(
							On<Android>()),
					() => Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.DisableSwipePaging(
						On<Android>()));
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