using System;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Extensions;

namespace TocoDo.BusinessLogic.ViewModels
{
	public interface IHabitViewModel : ICreateMode
	{
		string HabitDaysToRepeatWithRepeatType { get; }

		string     HabitTypeWithRepeats { get; }
		bool       IsStarted            { get; }
		DateTime   CreationDate         { get; }
		int        DaysToRepeat         { get; set; }
		string     Description          { get; set; }
		HabitType  HabitType            { get; set; }
		int        Id                   { get; }
		bool       IsFinished           { get; set; }
		bool       IsRecommended        { get; set; }
		int        MaxRepeatsADay       { get; set; }
		int        RepeatsToday         { get; set; }
		RepeatType RepeatType           { get; set; }
		DateTime?  StartDate            { get; set; }
		string     Title                { get; set; }

		ICommand FinishCreationCommand   { get; }
		ICommand EditCommand             { get; }
		ICommand EditTitleCommand        { get; }
		ICommand IncreaseTodayCommand    { get; }
		ICommand RemoveCommand           { get; }
		ICommand SelectRepeatCommand     { get; }
		ICommand SelectStartDateCommand  { get; }
		ICommand UnsetStartDateCommand   { get; }
		ICommand UpdateCommand           { get; }

		ObservableDictionary<DateTime, int> Filling { get; }

		void SetModelId(int id);
	}
}