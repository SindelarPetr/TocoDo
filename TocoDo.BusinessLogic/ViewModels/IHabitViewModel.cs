using System;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Extensions;

namespace TocoDo.BusinessLogic.ViewModels
{
	public interface IHabitViewModel : ICreateMode
	{
		ICommand   ConfirmCreationCommand          { get; }
		ICommand   EditCommand                     { get; }
		ICommand   EditTitleCommand                { get; }
		string     HabitDaysToRepeatWithRepeatType { get; }
		string     HabitTypeWithRepeats            { get; }
		ICommand   IncreaseTodayCommand            { get; }
		bool       IsStarted                       { get; }
		DateTime   ModelCreationDate               { get; }
		int        ModelDaysToRepeat               { get; set; }
		string     ModelDescription                { get; set; }
		HabitType  ModelHabitType                  { get; set; }
		int        ModelId                         { get; }
		bool       ModelIsFinished                 { get; set; }
		bool       ModelIsRecommended              { get; set; }
		int        ModelMaxRepeatsADay             { get; set; }
		int        ModelRepeatsToday               { get; set; }
		RepeatType ModelRepeatType                 { get; set; }
		DateTime?  ModelStartDate                  { get; set; }
		string     ModelTitle                      { get; set; }
		ICommand   RemoveCommand                   { get; }
		ICommand   SelectRepeatCommand             { get; }
		ICommand   SelectStartDateCommand          { get; }
		ICommand   UnsetStartDateCommand           { get; }
		ICommand   UpdateCommand                   { get; }

		ObservableDictionary<DateTime, int> ModelFilling { get; }

		void SetModelId(int id);
	}
}