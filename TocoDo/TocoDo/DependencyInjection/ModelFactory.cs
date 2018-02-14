using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Models;

namespace TocoDo.UI.DependencyInjection
{
	public class ModelFactory : IModelFactory
	{
		public ITaskModel CreateTaskModel(TaskViewModel task)
		{
			return new TaskModel
			{
				CreateTime   = task.CreateTime,
				Deadline     = task.Deadline,
				Description  = task.Description,
				Done         = task.Done,
				Id           = task.Id,
				Title        = task.Title,
				Reminder     = task.Reminder,
				ScheduleDate = task.ScheduleDate
			};
		}

		public IHabitModel CreateHabitModel(HabitViewModel habit)
		{
			throw new NotImplementedException();
		}
	}
}
