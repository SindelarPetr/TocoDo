
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.CustomRenderers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoBorderButton : Button
	{
		public NoBorderButton()
		{
			InitializeComponent();
		}
	}
}