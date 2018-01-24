﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;

namespace TocoDo.Views.Global
{
	[ContentProperty("InnerContent")]
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FrameWrapper : Frame
	{
		#region Backing fields
		public static BindableProperty InnerContentProperty = BindableProperty.Create(nameof(InnerContent), typeof(View), typeof(View));
		public static BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(string), typeof(string), string.Empty);
		public static BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(Color), Color.Gray);
		#endregion

		#region Properties
		public View InnerContent
		{
			get => (View)GetValue(InnerContentProperty);
			set => SetValue(InnerContentProperty, value);
		}

		public string Header
		{
			get => (string)GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		public Color HeaderBackgroundColor
		{
			get => (Color) GetValue(HeaderBackgroundColorProperty);
			set => SetValue(HeaderBackgroundColorProperty, value);
		}
		#endregion

		public FrameWrapper()
		{
			InitializeComponent();

			PropertyChanged += OnPropertyChanged;
			PropertyChanging += OnPropertyChanging;
		}

		private void OnPropertyChanging(object sender, PropertyChangingEventArgs propertyChangingEventArgs)
		{
			if (propertyChangingEventArgs.PropertyName == nameof(InnerContent))
			{
				if (InnerContent != null)
					Grid.Children.Remove(InnerContent);
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(InnerContent))
			{
				Grid.Children.Add(InnerContent, 0, 2);
			}
		}
	}
}