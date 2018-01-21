using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TocoDo.Controls
{
    class NullableDatePicker : DatePicker
    {
			private string _format = null;
			public static readonly BindableProperty NullableDateProperty = BindableProperty.Create(nameof(NullableDate), typeof(DateTime?), typeof(DateTime?));

			public DateTime? NullableDate
			{
				get { return (DateTime?)GetValue(NullableDateProperty); }
				set { SetValue(NullableDateProperty, value); UpdateDate(); }
			}

			private void UpdateDate()
			{
				if (NullableDate.HasValue) { if (null != _format) Format = _format; Date = NullableDate.Value; }
				else { _format = Format; Format = "pick ..."; }
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
