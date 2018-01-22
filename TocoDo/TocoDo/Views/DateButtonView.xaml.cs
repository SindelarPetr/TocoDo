using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TocoDo.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DateButtonView : IconButton
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

		public DateButtonView()
		{
			InitializeComponent();
			PropertyChanged += OnPropertyChanged;
			SetText();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(SelectedDate) || propertyChangedEventArgs.PropertyName == nameof(FormattedText))
			{
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