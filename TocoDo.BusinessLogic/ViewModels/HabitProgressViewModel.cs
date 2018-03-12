using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class HabitProgressViewModel : BaseViewModel
	{
		#region Properties


		public DateTime CreationDate { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public int DaysToRepeat { get; set; }

		public RepeatType RepeatType { get; set; }
		public HabitType  HabitType  { get; set; }

		public DateTime? StartDate { get; set; }

		public int MaxRepeatsADay { get; set; }

		public HabitViewModel                             Habit   { get; }
		public IReadOnlyList<KeyValuePair<DateTime, int>> Filling { get; }

		public IAsyncCommand DeleteCommand => Habit.DeleteCommand;

		public IAsyncCommand OnChangeUpdateCommand => _onChangeUpdateCommand ?? (_onChangeUpdateCommand = new AwaitableCommand(OnChangeUpdate));
		#endregion

		#region Fields

		private readonly IHabitService _habitService;
		
		private IAsyncCommand _onChangeUpdateCommand;

		#endregion

		public HabitProgressViewModel(IHabitService habitService, HabitViewModel habit)
		{
			_habitService  = habitService;
			Habit          = habit;
			MaxRepeatsADay = Habit.MaxRepeatsADay;
			StartDate      = Habit.StartDate;
			RepeatType     = Habit.RepeatType;
			DaysToRepeat   = Habit.DaysToRepeat;
			HabitType      = Habit.HabitType;
			Description    = Habit.Description;
			Title          = Habit.Title;
			CreationDate   = Habit.CreationDate;

			var list = Habit.Filling.ToList();
			list.Sort((p1, p2) => p1.Key > p2.Key ? 1 :
				          p1.Key == p2.Key           ? 0 : -1);
			Filling = list;
		}

		private async Task OnChangeUpdate()
		{
			if (Habit.Description != Description)
			{
				Habit.Description = Description;
				await Habit.UpdateCommand.ExecuteAsync(null);
			}
		}
	}
}