using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views.ModifyTaskViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickDeadlineView : ContentView
	{
		public PickDeadlineView ()
		{
			InitializeComponent ();
		}
	}
}