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
		//public BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(DateTime?));
		public BindableProperty FormattedTextProperty = BindableProperty.Create(nameof(FormattedText), typeof(string), typeof(string), "{0}");
		public BindableProperty DateFormatProperty = BindableProperty.Create(nameof(DateFormat), typeof(string), typeof(string), "D");
		#endregion

		#region Properties
		//public DateTime? SelectedDate
		//{
	//		get => (DateTime?)GetValue(SelectedDateProperty);
	//		set => SetValue(SelectedDateProperty, value);
		//}
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
		#endregion

		public DateButtonView ()
		{
			InitializeComponent ();
			PropertyChanged += OnPropertyChanged;
			Removed += OnRemoving;
			SetText();
		}

		private void OnRemoving(object sender, EventArgs eventArgs)
		{
			//SelectedDate = null;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(Value) || propertyChangedEventArgs.PropertyName == nameof(FormattedText))
			{
				SetText();
			}
		}

		private void SetText()
		{
			Text = String.Format(FormattedText, DateToTextConverter.Convert((DateTime?)Value));
		}

		private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
		{
			Value = e.NewDate;
		}
	}
}