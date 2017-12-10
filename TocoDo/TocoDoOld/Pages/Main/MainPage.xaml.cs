using Xamarin.Forms;

namespace TocoDo.Pages.Main
{
	public partial class MainPage : MasterDetailPage
	{
		public TabbedPage TabbedPage { get; set; }
		public MainPage()
		{
			InitializeComponent();

			TabbedPage = (TabbedPage)Detail;
		}
	}
}
