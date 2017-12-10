using System;
using System.Collections.Generic;

namespace TocoDo.Models
{
	public enum ChallengeType { Daylong, Unit }

	public class ChallengeModel
	{
		public ChallengeType ChallengeType { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public int DaysCount { get; set; }

		/// <summary>
		/// Records in which days the user meeted his goals (in case of unit type challenge) or how many times violated the challenge (in case of daylong type of challenge).
		/// </summary>
		public Dictionary<DateTime, int> Filling { get; set; }
	}
}
