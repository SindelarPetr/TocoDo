using System;
using System.ComponentModel;
using TocoDo.UI.Controls;
using TocoDo.UI.Converters;
using Xamarin.Forms;

namespace TocoDo.UI.Views
{
	public class DateButtonView : IconButton
	{
		#region Static

		public static BindableProperty FormattedTextProperty =
			BindableProperty.Create(nameof(FormattedText), typeof(string), typeof(string), "{0}");

		public static BindableProperty DateFormatProperty =
			BindableProperty.Create(nameof(DateFormat), typeof(string), typeof(string), "D");

		public static BindableProperty SelectedDateProperty =
			BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(DateTime?));

		#endregion

		#region Properties

		public string FormattedText
		{
			get => (string) GetValue(FormattedTextProperty);
			set => SetValue(FormattedTextProperty, value);
		}

		public string DateFormat
		{
			get => (string) GetValue(DateFormatProperty);
			set => SetValue(DateFormatProperty, value);
		}

		public DateTime? SelectedDate
		{
			get => (DateTime?) GetValue(SelectedDateProperty);
			set => SetValue(SelectedDateProperty, value);
		}

		#endregion

		#region Fields

		private readonly NullableDatePicker _nullablePicker;

		#endregion

		public DateButtonView()
		{
			Clicked         += OnClicked;
			Removed         += OnRemoved;
			_nullablePicker =  new NullableDatePicker();
			_nullablePicker.SetBinding(NullableDatePicker.NullableDateProperty, new Binding("SelectedDate", BindingMode.TwoWay));
			_nullablePicker.DateSelected += DatePicker_OnDateSelected;
			InnerContent                 =  _nullablePicker;

			PropertyChanged += OnPropertyChanged;
			SetText();
		}

		private void OnRemoved(object sender, EventArgs eventArgs)
		{
			SelectedDate = null;
		}

		private void OnClicked(object sender, EventArgs eventArgs)
		{
			// TODO: Show the action sheet with Today, Tomorrow, In one week, Pick a date

			_nullablePicker.Focus();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(SelectedDate) ||
			    propertyChangedEventArgs.PropertyName == nameof(FormattedText))
			{
				if (SelectedDate != null)
					MakeUpdateAnimation();
				IsActive = SelectedDate != null;
				SetText();
			}
		}

		private void SetText()
		{
			Text = string.Format(FormattedText, DateToTextConverter.Convert(SelectedDate));
		}

		private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
		{
			SelectedDate = e.NewDate;
		}
	}
}