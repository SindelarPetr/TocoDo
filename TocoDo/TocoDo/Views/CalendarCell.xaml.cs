using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarCell : ContentView
	{
		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(string));

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public CalendarCell ()
		{
			InitializeComponent ();
		}
	}
}