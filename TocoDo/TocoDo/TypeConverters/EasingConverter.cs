using System;
using System.Reflection;
using Xamarin.Forms;

namespace TocoDo.UI.TypeConverters
{
    public class EasingConverter : TypeConverter
    {
	    public override bool CanConvertFrom(Type sourceType)
	    {
		    if (sourceType == null)
			    throw new ArgumentNullException("EasingConverter.CanConvertFrom: sourceType");

		    return sourceType == typeof(string);
	    }

	    public override object ConvertFromInvariantString(string value)
	    {
		    if (value == null || !(value is string))
			    return null;

			string name = value.Trim();

		    if (name.StartsWith("Easing"))
			    name = name.Substring(7);

		    FieldInfo field = typeof(Easing).GetRuntimeField(name);

		    if (field != null && field.IsStatic)
		    {
			    return (Easing) field.GetValue(null);
		    }

		    throw new InvalidOperationException("Cannot convert \"" + value + "\" into Xamarin.Forms.Easing");
	    }
    }
}
