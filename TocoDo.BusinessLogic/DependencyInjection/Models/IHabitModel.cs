using System;

namespace TocoDo.BusinessLogic.DependencyInjection.Models
{
	/// <summary>
	///     Daylong - challenge is not a performable action - its something what lasts all the day
	///     Unit - challenge should be performed x times during the day
	/// </summary>
	public enum HabitType
	{
		Daylong,
		Unit
	}

	[Flags]
	public enum RepeatType
	{
		Mon    = 1,
		Tue    = 2,
		Wed    = 4,
		Thu    = 8,
		Fri    = 16,
		Sat    = 32,
		Sun    = 64,
		WorkWeek = 31,
		Weekend = 96,
		Days   = 128,
		Years  = 512
	}

	public interface IHabitModel
	{
		int       Id          { get; set; }
		HabitType HabitType   { get; set; }
		string    Title       { get; set; }
		string    Description { get; set; }

		bool IsRecommended { get; set; }

		/// <summary>
		///     Records in which days the user met his goals (in case of unit type challenge) or how many times violated the
		///     challenge (in case of daylong type of challenge). Its serialized as a JSON
		/// </summary>
		string Filling { get; set; }

		/// <summary>
		///     Indicates whether the Habit has its all iterations behind.
		/// </summary>
		bool IsFinished { get; set; }

		DateTime CreationDate { get; set; }

		#region Repeating

		/// <summary>
		///     Scheduling will work like this:
		///     There is a StartDate which is the first day to start calculating in which exact days the habit should be active.
		/// </summary>
		DateTime? StartDate { get; set; }

		/// <summary>
		///     Describes how is it with exact days for repeating. We can specify days of the week, or every week, month or years.
		/// </summary>
		RepeatType RepeatType { get; set; }

		/// <summary>
		///     Specifies how many days should be the task repeated.
		/// </summary>
		int DaysToRepeat { get; set; }

		/// <summary>
		///     How many times should be a unit task made per day
		/// </summary>
		int RepeatsADay { get; set; }

		#endregion
	}
}