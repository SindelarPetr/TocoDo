using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageBottomPanel : ContentView
	{
		public static BindableProperty RemoveCommandProperty =
			BindableProperty.Create(nameof(RemoveCommand), typeof(ICommand), typeof(ICommand));

		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(string));

		public PageBottomPanel()
		{
			InitializeComponent();
		}

		public ICommand RemoveCommand
		{
			get => (ICommand) GetValue(RemoveCommandProperty);
			set => SetValue(RemoveCommandProperty, value);
		}

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}