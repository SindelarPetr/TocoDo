using System;
using SQLite;
using TocoDo.BusinessLogic.DependencyInjection.Models;

namespace TocoDo.UI.Models
{
	public class TaskModel : ITaskModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(512)]
		public string Title { get; set; }
		public DateTime? Done { get; set; }
		[MaxLength(4000)]
		public string Description { get; set; }

		public DateTime? ScheduleDate { get; set; }
		public DateTime? Deadline { get; set; }

		public DateTime CreateTime { get; set; }

		public DateTime? Reminder { get; set; }

		public TaskModel() { }

		public TaskModel(TaskModel taskToCopy)
		{
			CopyTask(taskToCopy);
		}

		/// <summary>
		/// Sets all properties of this task to the properties of the given task.
		/// </summary>
		/// <param name="task">The task whose properties will be copied.</param>
		public void CopyTask(TaskModel task)
		{
			Id = task.Id;
			Title = task.Title;
			Done = task.Done;
			Description = task.Description;
			Deadline = task.Deadline;
			CreateTime = task.CreateTime;
			Reminder = task.Reminder;
			ScheduleDate = task.ScheduleDate;
		}
	}
}