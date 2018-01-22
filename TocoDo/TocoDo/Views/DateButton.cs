using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TocoDo.Controls;
using TocoDo.Converters;
using Xamarin.Forms;

namespace TocoDo.Views
{
	public class DateButtonView : IconButton
	{
		#region Backing fields
		public static BindableProperty FormattedTextProperty = BindableProperty.Create(nameof(FormattedText), typeof(string), typeof(string), "{0}");
		public static BindableProperty DateFormatProperty = BindableProperty.Create(nameof(DateFormat), typeof(string), typeof(string), "D");
		public static BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(DateTime?));
		#endregion

		#region Properties
		public string FormattedText
		{
			get => (string)GetValue(FormattedTextProperty);
			set => SetValue(FormattedTextProperty, value);
		}
		public string DateFormat
		{
			get => (string)GetValue(DateFormatProperty);
			set => SetValue(DateFormatProperty, value);
		}
		public DateTime? SelectedDate
		{
			get => (DateTime?)GetValue(SelectedDateProperty);
			set => SetValue(SelectedDateProperty, value);
		}
		#endregion

		private NullableDatePicker _nullablePicker;

		public DateButtonView()
		{
			Clicked += OnClicked;
			Removed += OnRemoved;
			_nullablePicker = new NullableDatePicker();
			_nullablePicker.SetBinding(NullableDatePicker.NullableDateProperty, new Binding("SelectedDate", BindingMode.TwoWay));
			_nullablePicker.DateSelected += DatePicker_OnDateSelected;
			//SetBinding(IsActiveProperty, new Binding("SelectedDate", BindingMode.OneWay, new IsNotNullConverter()));
			InnerContent = _nullablePicker;

			PropertyChanged += OnPropertyChanged;
			SetText();
		}

		private void OnRemoved(object sender, EventArgs eventArgs)
		{
			SelectedDate = null;
		}

		private void OnClicked(object sender, EventArgs eventArgs)
		{
			_nullablePicker.Focus();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(SelectedDate) || propertyChangedEventArgs.PropertyName == nameof(FormattedText))
			{
				if(SelectedDate != null)
					MakeUpdateAnimation();
				IsActive = SelectedDate != null;
				SetText();
			}
		}

		private void SetText()
		{
			Text = String.Format(FormattedText, DateToTextConverter.Convert(SelectedDate));
		}

		private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
		{
			SelectedDate = e.NewDate;
		}
	}
}
