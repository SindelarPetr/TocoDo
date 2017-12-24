using System;

namespace TocoDo.Helpers
{
	public static class DateTimeExtensions
	{
		public static string GetDayExtension(this DateTime date)
		{
			var ext = (date.Day % 10 == 1 && date.Day != 11)
				? "st"
				: (date.Day % 10 == 2 && date.Day != 12)
					? "nd"
					: (date.Day % 10 == 3 && date.Day != 13)
						? "rd"
						: "th";

			return ext;
		}
	}
}
