using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TocoDo.Converters.Habits;
using TocoDo.Models;
using Xamarin.Forms;

namespace TocoDo.Views.Habits
{
    public class RepeatTypeButton : IconButton
    {
		#region Backing fields
		public static BindableProperty RepeatTypeProperty = BindableProperty.Create(nameof(RepeatType), typeof(RepeatType), typeof(RepeatType), RepeatType.Days, BindingMode.TwoWay);
		public static BindableProperty DaysToRepeatProperty = BindableProperty.Create(nameof(DaysToRepeat), typeof(int), typeof(int), 20, BindingMode.TwoWay);
		public static BindableProperty FormattedTextProperty = BindableProperty.Create(nameof(FormattedText), typeof(string), typeof(string), "{0}");
		#endregion

		#region Properties
		public RepeatType RepeatType
		{
			get => (RepeatType)GetValue(RepeatTypeProperty);
			set => SetValue(RepeatTypeProperty, value);
		}
		public int DaysToRepeat
		{
			get => (int)GetValue(DaysToRepeatProperty);
			set => SetValue(DaysToRepeatProperty, value);
		}
	    public string FormattedText
	    {
		    get => (string) GetValue(FormattedTextProperty);
		    set => SetValue(FormattedTextProperty, value);
	    }
		#endregion

		public RepeatTypeButton()
	    {
		    PropertyChanged += OnPropertyChanged;
	    }

	    private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
	    {
		    if (propertyChangedEventArgs.PropertyName == nameof(RepeatType) ||
		        propertyChangedEventArgs.PropertyName == nameof(DaysToRepeat) ||
		        propertyChangedEventArgs.PropertyName == nameof(FormattedText))
		    {
			    UpdateText();
				//MakeUpdateAnimation();
		    }
	    }

	    private void UpdateText()
	    {
		    string repeatText = HabitDateConverter.GetRepeatText(RepeatType, DaysToRepeat);

		    Text = string.Format(FormattedText, repeatText);
	    }
    }
}
