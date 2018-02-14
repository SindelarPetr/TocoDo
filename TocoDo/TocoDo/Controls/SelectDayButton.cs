using System;
using System.ComponentModel;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using Xamarin.Forms;

namespace TocoDo.UI.Controls
{
    public class SelectDayButton : Button
    {
		public static BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(bool), false, BindingMode.TwoWay);

	    public bool IsToggled
	    {
		    get => (bool)GetValue(IsToggledProperty);
		    set => SetValue(IsToggledProperty, value);
	    }

	    public Color ToggledTextColor { get; set; }
	    public Color ToggledBackgroundColor { get; set; }
	    public Color NotToggledTextColor { get; set; }
	    public Color NotToggledBackgroundColor { get; set; }

		public RepeatType RepeatType { get; set; }

		public ICommand ToggleCommand { get; set; }

	    public SelectDayButton()
	    {
			Clicked += SelectDayButton_Clicked;

		    ToggledTextColor = Color.White;
		    ToggledBackgroundColor = ((App)App.Current).ColorPrimary;
		    TextColor = NotToggledTextColor = Color.Black;
		    BackgroundColor = NotToggledBackgroundColor = Color.White;

			PropertyChanged += OnPropertyChanged;
	    }

	    private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
	    {
		    if (propertyChangedEventArgs.PropertyName == nameof(IsToggled))
		    {
			    if (IsToggled)
			    {
					BackgroundColor = ToggledBackgroundColor;
				    TextColor = ToggledTextColor;
			    }
			    else
			    {
					BackgroundColor = NotToggledBackgroundColor;
				    TextColor = NotToggledTextColor;
				}

				ToggleCommand?.Execute(this);
		    }
	    }

	    private void SelectDayButton_Clicked(object sender, EventArgs e)
		{
			IsToggled = !IsToggled;
		}
	}
}
