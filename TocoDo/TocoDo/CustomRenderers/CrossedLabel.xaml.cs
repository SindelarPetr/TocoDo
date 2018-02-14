using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.CustomRenderers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public class CrossedLabel : Label
	{
		public static BindableProperty IsStrikeThroughProperty = BindableProperty.Create(
			"IsStrikeThrough", typeof(bool), typeof(bool), false);

		public bool IsStrikeThrough
		{
			get => (bool)GetValue(IsStrikeThroughProperty);
			set => SetValue(IsStrikeThroughProperty, value);
		}
	}
}