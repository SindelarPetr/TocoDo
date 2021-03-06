﻿using System;
using System.Windows.Input;
using TocoDo.BusinessLogic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[ContentProperty("InnerContent")]
	public partial class IconButton : ContentView
	{
		#region Static

		public static readonly BindableProperty ImageSourceProperty =
			BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(string));

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(string));

		public static readonly BindableProperty IsRemoveButtonVisibleProperty =
			BindableProperty.Create(nameof(IsRemoveButtonVisible), typeof(bool), typeof(bool), false);

		public static readonly BindableProperty HasRemoveButtonProperty =
			BindableProperty.Create(nameof(HasRemoveButton), typeof(bool), typeof(bool), true);

		public static readonly BindableProperty IsActiveProperty =
			BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(bool), false);

		public static readonly BindableProperty ColorProperty =
			BindableProperty.Create(nameof(Color), typeof(Color), typeof(Color), Color.Gray);

		public static readonly BindableProperty ClickCommandProperty 
			= BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(ICommand));
		#endregion

		#region Properties

		public string ImageSource
		{
			get => (string) GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public Color ActiveColor
		{
			get => ColorChangeAction.NewColor;
			set
			{
				ColorChangeAction.NewColor = value;

				if (IsActive && HasRemoveButton)
					ColorChangeAction.DoInvoke(this);
			}
		}

		public Color PassiveColor { get; set; }

		public Color Color
		{
			get => (Color) GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		public bool IsRemoveButtonVisible
		{
			get => (bool) GetValue(IsRemoveButtonVisibleProperty);
			set => SetValue(IsRemoveButtonVisibleProperty, value);
		}

		public bool HasRemoveButton
		{
			get => (bool) GetValue(HasRemoveButtonProperty);
			set => SetValue(HasRemoveButtonProperty, value);
		}

		public bool IsActive
		{
			get => (bool) GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}

		public View InnerContent
		{
			get => _innerContent.Content;
			set => _innerContent.Content = value;
		}

		public ICommand ClickCommand
		{
			get => (ICommand) GetValue(ClickCommandProperty);
			set => SetValue(ClickCommandProperty, value);
		}
		public ICommand RemoveCommand { get; set; }
		#endregion

		#region Fields

		public Action ScaleAnimation;

		#endregion

		public IconButton()
		{
			try
			{
				MyLogger.WriteStartMethod();
				Clicked += (a, b) => ClickCommand?.Execute(null);
				Removed += (a, b) => RemoveCommand?.Execute(null);
				MyLogger.WriteInMethod("Before InitializeComponent");
				InitializeComponent();
				MyLogger.WriteInMethod("After InitializeComponent");

				var maxScale   = 1.25;
				ScaleAnimation += () => this.ScaleTo(maxScale, 250, new Easing(t =>
				{
					var val = Math.Sin(t * Math.PI) * (maxScale - 1);
					MyLogger.WriteInMethod(val.ToString());
					return val;
				}));
				MyLogger.WriteEndMethod();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}
		}

		private void ClickRecognise(object sender, EventArgs e)
		{
			Clicked?.Invoke(this, e);
		}

		private void RemoveRecognise(object sender, EventArgs e)
		{
			Removed?.Invoke(this, e);
		}

		public void MakeUpdateAnimation()
		{
			ScaleAnimation();
		}

		public event EventHandler Clicked;
		public event EventHandler Removed;
	}
}