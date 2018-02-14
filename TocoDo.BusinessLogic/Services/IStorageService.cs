using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
    public interface IStorageService
	{
		ReadOnlyObservableCollection<TaskViewModel> AllTasks { get; }
		ReadOnlyObservableCollection<HabitViewModel> AllHabits { get; }

		void StartCreatingHabit();
	    Task ConfirmCreationOfHabit(HabitViewModel habit);
	    void CancelCreationOfHabit(HabitViewModel habit);
	    Task UpdateHabit(HabitViewModel habit);
	    Task DeleteHabit(HabitViewModel habit);

		Task StartCreatingTask(DateTime? date);
		Task ConfirmCreationOfTask(TaskViewModel task);
		Task CancelCreationOfTask(TaskViewModel task);
		Task DeleteTask(TaskViewModel task);
		Task UpdateTask(TaskViewModel task);
	}
}
