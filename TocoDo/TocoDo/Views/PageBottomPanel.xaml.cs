using System.Windows.Input;
using TocoDo.BusinessLogic.Helpers.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageBottomPanel : ContentView
	{
		public static BindableProperty RemoveCommandProperty =
			BindableProperty.Create(nameof(RemoveCommand), typeof(IAsyncCommand), typeof(IAsyncCommand));

		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(string));

		public PageBottomPanel()
		{
			InitializeComponent();
		}

		public IAsyncCommand RemoveCommand
		{
			get => (IAsyncCommand) GetValue(RemoveCommandProperty);
			set => SetValue(RemoveCommandProperty, value);
		}

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}