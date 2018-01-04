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
	public enum Days { Mon = 1, Tue = 2, Wed = 4, Thu = 8, Fri = 16, Sat = 32, Sun = 64 }

	public enum RepeatType { Days, Weeks, Months, Years }

	public class HabitModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public HabitType HabitType { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime? StartDate { get; set; }
		public RepeatType RepeatType { get; set; }
		public short RepeatNumber { get; set; }
		public int DailyFillingCount { get; set; }
		public bool IsRecommended { get; set; }

		/// <summary>
		/// Records in which days the user meeted his goals (in case of unit type challenge) or how many times violated the challenge (in case of daylong type of challenge).
		/// </summary>
		public Dictionary<DateTime, int> Filling { get; set; }
	}
}