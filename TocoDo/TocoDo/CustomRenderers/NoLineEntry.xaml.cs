using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.CustomRenderers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoLineEntry : Entry
	{
		public NoLineEntry()
		{
			InitializeComponent();
		}
	}
}