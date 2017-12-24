
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.CustomRenderers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrossedLabel : Label
	{
		public static BindableProperty IsStrikeThroughProperty = BindableProperty.Create(
			"IsStrikeThrough", typeof(bool), typeof(bool), false);

		public bool IsStrikeThrough
		{
			get => (bool)GetValue(IsStrikeThroughProperty);
			set => SetValue(IsStrikeThroughProperty, value);
		}

		public CrossedLabel()
		{
			InitializeComponent();
		}
	}
}