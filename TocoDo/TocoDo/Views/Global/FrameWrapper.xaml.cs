using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views.Global
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FrameWrapper : ContentView
	{
		public FrameWrapper ()
		{
			InitializeComponent ();
		}
	}
}