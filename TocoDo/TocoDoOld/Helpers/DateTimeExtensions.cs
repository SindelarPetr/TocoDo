using System;

namespace TocoDo.Helpers
{
	public static class DateTimeExtensions
	{
		public static string GetDayExtension(this DateTime date)
		{
			return (DateTime.Now.Day % 10 == 1 && DateTime.Now.Day != 11)
				? "st"
				: (DateTime.Now.Day % 10 == 2 && DateTime.Now.Day != 12)
					? "nd"
					: (DateTime.Now.Day % 10 == 3 && DateTime.Now.Day != 13)
						? "rd"
						: "th";
		}
	}
}
