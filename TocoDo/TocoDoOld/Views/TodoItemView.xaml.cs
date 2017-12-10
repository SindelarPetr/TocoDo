
using System;
using TocoDo.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoItemView : ContentView
	{
		#region Text
		public static readonly BindableProperty TextProperty = BindableProperty.Create(
			propertyName: "Text",
			returnType: typeof(string),
			declaringType: typeof(string),
			defaultValue: "Beautiful checkbox");
		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
		#endregion

		#region IsChecked
		public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
	propertyName: "IsChecked",
	returnType: typeof(bool),
	declaringType: typeof(bool),
	defaultValue: false);
		public bool IsChecked
		{
			get => (bool)GetValue(IsCheckedProperty);
			set
			{
				SetValue(IsCheckedProperty, value);

				if (value) AnimateCheck();
				else AnimateUncheck();
			}
		}

		#endregion

		public TodoItemView()
		{
			BindingContext = new TaskModel
			{
				Title = "This is placeholder title",
				Deadline = DateTime.Now,
				Description = "This is placeholder description.",
			};

			InitializeComponent();
		}
		public TodoItemView(TaskModel model)
		{
			BindingContext = model;

			InitializeComponent();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			IsChecked = !IsChecked;
		}

		private void AnimateCheck()
		{
			ImageUnchecked.FadeTo(0);
			ImageChecked.FadeTo(1);
			LabelText.FadeTo(0.4);
			//BackgroundColor = Color.MediumSeaGreen;
			//LabelCrossed.FadeTo(1);
			//CrossBox.FadeTo(1);
		}

		private void AnimateUncheck()
		{
			ImageUnchecked.FadeTo(1);
			ImageChecked.FadeTo(0);
			LabelText.FadeTo(1);
			//BackgroundColor = Color.Transparent;
			//LabelCrossed.FadeTo(0);
			//CrossBox.FadeTo(0);
		}

		private void PinchGestureRecognizer_OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
		{

		}
	}
}