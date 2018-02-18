using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarCell : ContentView
	{
		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(string));

		public CalendarCell()
		{
			InitializeComponent();
		}

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}