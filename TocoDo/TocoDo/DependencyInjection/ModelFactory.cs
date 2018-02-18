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
				Id            = habit.ModelId,
				CreationDate  = habit.ModelCreationDate,
				RepeatType    = habit.ModelRepeatType,
				Description   = habit.ModelDescription,
				Filling       = JsonConvert.SerializeObject(new Dictionary<DateTime, int>(habit.ModelFilling)),
				HabitType     = habit.ModelHabitType,
				DaysToRepeat  = habit.ModelDaysToRepeat,
				StartDate     = habit.ModelStartDate,
				Title         = habit.ModelTitle,
				IsRecommended = habit.ModelIsRecommended,
				RepeatsADay   = habit.ModelMaxRepeatsADay
			};
		}

		public T CreateHabitModel<T>(HabitViewModel habit) where T : IHabitModel, new()
		{
			throw new NotImplementedException();
		}
	}
}