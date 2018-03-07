using System;
using System.ComponentModel;
using TocoDo.BusinessLogic.DependencyInjection;
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

		public static BindableProperty CanSelectTodayProperty =
			BindableProperty.Create(nameof(CanSelectToday), typeof(bool), typeof(bool), true);
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

		public bool CanSelectToday
		{
			get => (bool) GetValue(CanSelectTodayProperty);
			set => SetValue(CanSelectTodayProperty, value);
		}

		#endregion

		#region Fields

		private readonly NullableDatePicker _nullablePicker;

		#endregion

		public DateButtonView()
		{
			Clicked         += OnClicked;
			Removed         += OnRemoved;
			_nullablePicker =  new NullableDatePicker
			                   {
				                   MinimumDate = DateTime.Today
			                   };
			//_nullablePicker.SetBinding(NullableDatePicker.NullableDateProperty, new Binding("SelectedDate", BindingMode.TwoWay));
			_nullablePicker.NullableDate = SelectedDate;
			_nullablePicker.DateSelected += DatePicker_OnDateSelected;
			InnerContent                 =  _nullablePicker;

			PropertyChanged += OnPropertyChanged;
			FormattedText = TocoDo.BusinessLogic.Properties.Resources.StartDateText;
		}

		private void OnRemoved(object sender, EventArgs eventArgs)
		{
			SelectedDate = null;
		}

		private async void OnClicked(object sender, EventArgs eventArgs)
		{
			var result = await ((App) Application.Current).Navigation.DisplayDatePickingActionSheet(CanSelectToday);
			switch (result)
			{
				case DatePickingActionSheetResult.Canceled:
					return;
				case DatePickingActionSheetResult.Today:
					SelectedDate = DateTime.Today;
					break;
				case DatePickingActionSheetResult.Tomorrow:
					SelectedDate = DateTime.Today.AddDays(1);
					break;
				case DatePickingActionSheetResult.InOneWeek:
					SelectedDate = DateTime.Today.AddDays(7);
					break;
				case DatePickingActionSheetResult.PickADate:
					_nullablePicker.Focus();
					break;
				default:
					throw new ArgumentOutOfRangeException("The given DatePickingActionSheetResult was not recognised.");
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(SelectedDate) ||
			    propertyChangedEventArgs.PropertyName == nameof(FormattedText))
			{
				if (SelectedDate != null)
					MakeUpdateAnimation();
				IsActive = SelectedDate != null;

				if (propertyChangedEventArgs.PropertyName == nameof(SelectedDate))
					_nullablePicker.NullableDate = SelectedDate;

				SetText();
			}
			else if (propertyChangedEventArgs.PropertyName == nameof(CanSelectToday))
			{
				_nullablePicker.MinimumDate = CanSelectToday ? DateTime.Today : DateTime.Today.AddDays(1);
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