using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMasterPage : ContentPage
	{
		public MasterViewModel ViewModel { get; set; }
		public MainMasterPage()
		{
			ViewModel = new MasterViewModel(((App)App.Current).Navigation);
			InitializeComponent();
		}
	}
}