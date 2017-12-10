using SQLite;
using System;

namespace TocoDo.Models
{
	public class TaskModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(512)]
		public string Title { get; set; }
		public bool Done { get; set; }
		[MaxLength(4000)]
		public string Description { get; set; }
		public DateTime? Deadline { get; set; }
	}
}