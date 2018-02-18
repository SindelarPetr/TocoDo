using System;
using SQLite;
using TocoDo.BusinessLogic.DependencyInjection.Models;

namespace TocoDo.UI.Models
{
	public class HabitModel : IHabitModel
	{
		[PrimaryKey] [AutoIncrement] public int Id { get; set; }

		public HabitType HabitType   { get; set; }
		public string    Title       { get; set; }
		public string    Description { get; set; }

		public bool     IsRecommended { get; set; }
		public string   Filling       { get; set; }
		public bool     IsFinished    { get; set; }
		public DateTime CreationDate  { get; set; }

		#region Repeating

		public DateTime?  StartDate    { get; set; }
		public RepeatType RepeatType   { get; set; }
		public int        DaysToRepeat { get; set; }
		public int        RepeatsADay  { get; set; }

		#endregion
	}
}