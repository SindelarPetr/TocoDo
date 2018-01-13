using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.CustomRenderers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FloatingAddButton : Button
	{
		public FloatingAddButton ()
		{
			InitializeComponent ();
		}
	}
}