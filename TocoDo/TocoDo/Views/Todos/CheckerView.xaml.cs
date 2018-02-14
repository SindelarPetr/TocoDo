using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Todos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckerView : ContentView
	{
		#region Backing fiels
		public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(bool), false, BindingMode.TwoWay);
		public static readonly BindableProperty CheckedColorProperty = BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(Color), ((App)App.Current).ColorPrimary);
		public static readonly BindableProperty UncheckedColorProperty = BindableProperty.Create(nameof(UncheckedColor), typeof(Color), typeof(Color), ((App)App.Current).ColorPrimary);
		#endregion

		#region Properties
		public bool IsChecked
		{
			get => (bool)GetValue(IsCheckedProperty);
			set => SetValue(IsCheckedProperty, value);
		}
		public Color CheckedColor
		{
			get => (Color)GetValue(CheckedColorProperty);
			set => SetValue(CheckedColorProperty, value);
		}
		public Color UncheckedColor
		{
			get => (Color)GetValue(UncheckedColorProperty);
			set => SetValue(UncheckedColorProperty, value);
		}

		public ICommand CheckCommand { get; set; }
		#endregion

		public CheckerView()
		{
			InitializeComponent();
		}

		private void TapCheckBox_OnTapped(object sender, EventArgs e)
		{
			IsChecked = !IsChecked;
			CheckCommand?.Execute(IsChecked);
		}
	}
}