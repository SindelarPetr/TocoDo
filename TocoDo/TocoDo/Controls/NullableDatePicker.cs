using System;
using Xamarin.Forms;

namespace TocoDo.UI.Controls
{
	internal class NullableDatePicker : DatePicker
	{
		public static readonly BindableProperty NullableDateProperty =
			BindableProperty.Create(nameof(NullableDate), typeof(DateTime?), typeof(DateTime?));

		private string _format;

		public DateTime? NullableDate
		{
			get => (DateTime?) GetValue(NullableDateProperty);
			set
			{
				if (NullableDate == value)
					return;

				SetValue(NullableDateProperty, value);
				UpdateDate();
			}
		}

		private void UpdateDate()
		{
			if (NullableDate.HasValue)
			{
				if (null != _format) Format = _format;
				Date                        = NullableDate.Value;
			}
			else
			{
				_format = Format;
				Format  = "pick ...";
			}
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			UpdateDate();
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == "Date") NullableDate = Date;
		}
	}
}