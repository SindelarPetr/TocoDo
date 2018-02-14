using System;

namespace TocoDo.BusinessLogic.DependencyInjection.Models
{
	public interface ITaskModel
	{
		int Id { get; set; }
		string Title { get; set; }
		DateTime? Done { get; set; }
		string Description { get; set; }

		DateTime? ScheduleDate { get; set; }
		DateTime? Deadline { get; set; }

		DateTime CreateTime { get; set; }

		DateTime? Reminder { get; set; }
	}
}