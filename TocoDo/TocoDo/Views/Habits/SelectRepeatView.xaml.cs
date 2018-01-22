using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectRepeatView : ContentView
	{
		private SelectRepeatView Vm
		{
			get => (SelectRepeatView) BindingContext;
			set => BindingContext = value;
		}

		public SelectRepeatView()
		{
			//Vm = new SelectRepeatView();
			InitializeComponent();
		}

		
	}
}