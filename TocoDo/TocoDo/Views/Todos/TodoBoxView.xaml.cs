﻿using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckerView : ContentView
	{
		#region Backing fiels
		public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(bool), false, BindingMode.TwoWay);
		public static readonly BindableProperty IsBackgroundVisibleProperty = BindableProperty.Create(nameof(IsBackgroundVisible), typeof(bool), typeof(bool), false);
		public static readonly BindableProperty BackgroundImageColorProperty = BindableProperty.Create(nameof(BackgroundImageColor), typeof(Color), typeof(Color), Color.White);
		public static readonly BindableProperty CheckedColorProperty = BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(Color), Color.White);
		public static readonly BindableProperty UncheckedColorProperty = BindableProperty.Create(nameof(UncheckedColor), typeof(Color), typeof(Color), Color.White);
		#endregion

		#region Properties
		public bool IsChecked
		{
			get => (bool)GetValue(IsCheckedProperty);
			set => SetValue(IsCheckedProperty, value);
		}
		public bool IsBackgroundVisible
		{
			get => (bool)GetValue(IsBackgroundVisibleProperty);
			set => SetValue(IsBackgroundVisibleProperty, value);
		}
		public Color BackgroundImageColor
		{
			get => (Color)GetValue(BackgroundImageColorProperty);
			set => SetValue(BackgroundImageColorProperty, value);
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