using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarCell : ContentView
	{
		#region Backing fields
		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(string));
		public static BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(ICommand));
		#endregion

		#region Properties
		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public ICommand TappedCommand
		{
			get => (ICommand) GetValue(TappedCommandProperty);
			set => SetValue(TappedCommandProperty, value);
		}
		#endregion

		public CalendarCell()
		{
			InitializeComponent();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			if (TappedCommand.CanExecute(this))
			{
				TappedCommand.Execute(this);
			}
		}
	}
}