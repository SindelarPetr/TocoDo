﻿using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoBoxView : ContentView
	{
		#region IsChecked
		public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
			propertyName: "IsChecked",
			returnType: typeof(bool),
			declaringType: typeof(bool),
			defaultValue: false,
			defaultBindingMode: BindingMode.TwoWay,
			propertyChanged: (bindable, value, newValue) =>
			{ Debug.WriteLine($"Setting IsChecked from {(bool)value} to: {(bool)newValue}"); });


		public bool IsChecked
		{
			get => (bool)GetValue(IsCheckedProperty);
			set => SetValue(IsCheckedProperty, value);
		}
		#endregion

		#region IsBackgroundVisible
		public static readonly BindableProperty IsBackgroundVisibleProperty = BindableProperty.Create(
			propertyName: "IsBackgroundVisible",
			returnType: typeof(bool),
			declaringType: typeof(bool),
			defaultValue: false);

		public bool IsBackgroundVisible
		{
			get => (bool)GetValue(IsBackgroundVisibleProperty);
			set => SetValue(IsBackgroundVisibleProperty, value);
		}
		#endregion

		#region BackgroundImageColor
		public static readonly BindableProperty BackgroundImageColorProperty = BindableProperty.Create(
			propertyName: "BackgroundImageColor",
			returnType: typeof(Color),
			declaringType: typeof(Color),
			defaultValue: Color.White);

		public Color BackgroundImageColor
		{
			get => (Color)GetValue(BackgroundImageColorProperty);
			set => SetValue(BackgroundImageColorProperty, value);
		}
		#endregion

		#region CheckedColor
		public static readonly BindableProperty CheckedColorProperty = BindableProperty.Create(
			propertyName: "CheckedColor",
			returnType: typeof(Color),
			declaringType: typeof(Color),
			defaultValue: Color.LimeGreen);

		public Color CheckedColor
		{
			get => (Color)GetValue(CheckedColorProperty);
			set => SetValue(CheckedColorProperty, value);
		}
		#endregion

		#region UncheckedColor
		public static readonly BindableProperty UncheckedColorProperty = BindableProperty.Create(
			propertyName: "UncheckedColor",
			returnType: typeof(Color),
			declaringType: typeof(Color),
			defaultValue: Color.White);

		public Color UncheckedColor
		{
			get => (Color)GetValue(UncheckedColorProperty);
			set => SetValue(UncheckedColorProperty, value);
		}
		#endregion

		#region CheckCommand
		public static readonly BindableProperty CheckCommandProperty = BindableProperty.Create(
			propertyName: "CheckCommand",
			returnType: typeof(ICommand),
			declaringType: typeof(ICommand),
			defaultValue: null);

		public ICommand CheckCommand
		{
			get => (ICommand)GetValue(CheckCommandProperty);
			set => SetValue(CheckCommandProperty, value);
		}
		#endregion

		public TodoBoxView()
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