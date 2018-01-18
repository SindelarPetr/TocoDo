using SQLite;
using System;
using System.Collections.Generic;

namespace TocoDo.Models
{
	/// <summary>
	/// Daylong - challenge is not a performable action - its something what lasts all the day
	/// Unit - challenge should be performed x times during the day
	/// </summary>
	public enum HabitType { Daylong, Unit }

	[Flags]
	public enum RepeatType { Mon = 1, Tue = 2, Wed = 4, Thu = 8, Fri = 16, Sat = 32, Sun = 64, Days = 128, Months = 256, Years = 512 }

	public class HabitModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public HabitType HabitType { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		#region Repeating
		/// <summary>
		/// Scheduling will work like this:
		/// There is a StartDate which is the first day to start calculating in which exact days the habit should be active.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Describes how is it with exact days for repeating. We can specify days of the week, or every week, month or years.
		/// </summary>
		public RepeatType RepeatType { get; set; }

		/// <summary>
		/// Specifies how many times should be the task repeated.
		/// </summary>
		public short RepeatNumber { get; set; }
		#endregion

		public int DailyFillingCount { get; set; }
		public bool IsRecommended { get; set; }

		/// <summary>
		/// Records in which days the user meeted his goals (in case of unit type challenge) or how many times violated the challenge (in case of daylong type of challenge).
		/// </summary>
		public Dictionary<DateTime, int> Filling { get; set; }
	}
}