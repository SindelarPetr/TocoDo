using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Models;

namespace TocoDo.UI.DependencyInjection
{
	public class ModelFactory : IModelFactory
	{
		public ITaskModel CreateTaskModel(ITaskViewModel task)
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

		public IHabitModel CreateHabitModel(IHabitViewModel habit) 
		{
			return new HabitModel
			{
				Id            = habit.Id,
				CreationDate  = habit.CreationDate,
				RepeatType    = habit.RepeatType,
				Description   = habit.Description,
				Filling       = JsonConvert.SerializeObject(habit.Filling),
				HabitType     = habit.HabitType,
				DaysToRepeat  = habit.DaysToRepeat,
				StartDate     = habit.StartDate,
				Title         = habit.Title,
				IsRecommended = habit.IsRecommended,
				RepeatsADay   = habit.MaxRepeatsADay
			};
		}
	}
}