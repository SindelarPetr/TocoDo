using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
	public interface IStorageService
	{
		ReadOnlyObservableCollection<IHabitViewModel> AllHabits  { get; }
		void StartCreatingHabit();
		Task ConfirmCreationOfHabit(IHabitViewModel habit);
		void CancelCreationOfHabit(IHabitViewModel  habit);
		Task UpdateHabit(IHabitViewModel            habit);
		Task DeleteHabit(IHabitViewModel            habit);

		ReadOnlyObservableCollection<ITaskViewModel> AllTasks { get; }
		void StartCreatingTask(DateTime?         date);
		Task ConfirmCreationOfTask(ITaskViewModel task);
		void CancelCreationOfTask(ITaskViewModel  task);
		Task DeleteTask(ITaskViewModel            task);
		Task UpdateTask(ITaskViewModel            task);
	}
}