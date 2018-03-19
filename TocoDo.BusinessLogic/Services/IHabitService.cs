using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
	public interface IHabitService
	{
		ReadOnlyObservableCollection<IHabitViewModel> AllHabits { get; }
		IDateTimeProvider DateTimeProvider { get; set; }
		IModelFactory ModelFactory { get; set; }
		INavigationService Navigation { get; set; }
		IPersistance Persistance { get; set; }

		void StartCreation();
		Task ConfirmCreationAsync(IHabitViewModel habit);
		void CancelCreation(IHabitViewModel habit);
		Task DeleteAsync(IHabitViewModel habit);
		Task LoadAsync();
		Task UpdateAsync(IHabitViewModel habit);
		Task<List<HabitViewModel>> LoadPastHabitsAsync();
	}
}