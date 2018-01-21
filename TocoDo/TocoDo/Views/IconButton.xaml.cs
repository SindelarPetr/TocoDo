using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IconButton : ContentView
	{
		#region ImageSource
		public static BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(string), typeof(string));

		public string ImageSource
		{
			get => (string)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}
		#endregion

		#region Text
		public static BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(string));

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
		#endregion

		#region Color
		public static BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Color), Color.Gray);

		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}
		#endregion

		#region IsRemoveButtonVisible
		public static BindableProperty IsRemoveButtonVisibleProperty = BindableProperty.Create("IsRemoveButtonVisible", typeof(bool), typeof(bool), false);

		public bool IsRemoveButtonVisible
		{
			get => (bool)GetValue(IsRemoveButtonVisibleProperty);
			set => SetValue(IsRemoveButtonVisibleProperty, value);
		}
		#endregion

		#region Click command
		public static BindableProperty ClickCommandProperty = BindableProperty.Create("ClickCommand", typeof(ICommand), typeof(ICommand));

		public ICommand ClickCommand
		{
			get => (ICommand)GetValue(ClickCommandProperty);
			set => SetValue(ClickCommandProperty, value);
		}
		#endregion

		#region Remove command
		public static BindableProperty RemoveCommandProperty = BindableProperty.Create("RemoveCommand", typeof(ICommand), typeof(ICommand));

		public ICommand RemoveCommand
		{
			get => (ICommand)GetValue(RemoveCommandProperty);
			set => SetValue(RemoveCommandProperty, value);
		}
		#endregion

		public event EventHandler Clicked = (a, b) => {};
		public event EventHandler Removing = (a, b) => { };

		public IconButton()
		{
			InitializeComponent();
		}

		private void RemoveTapRecogniser_OnTapped(object sender, EventArgs e)
		{
			Removing?.Invoke(this, e);
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			Clicked?.Invoke(this, e);
		}
	}
}