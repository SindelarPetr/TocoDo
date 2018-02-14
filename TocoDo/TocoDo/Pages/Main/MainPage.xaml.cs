using TocoDo.BusinessLogic;
using Xamarin.Forms;

namespace TocoDo.UI.Pages.Main
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			MyLogger.WriteStartMethod();
			try
			{
				InitializeComponent();
			}
			catch (System.Exception e)
			{
				MyLogger.WriteException(e);
			}
			MyLogger.WriteEndMethod();
		}
	}
}
