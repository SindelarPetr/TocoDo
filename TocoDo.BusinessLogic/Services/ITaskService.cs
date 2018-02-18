using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
	public interface ITaskService
	{
		ReadOnlyObservableCollection<ITaskViewModel> AllTasks { get; }
		IDateTimeProvider DateTimeProvider { get; set; }
		IModelFactory ModelFactory { get; set; }
		INavigationService Navigation { get; set; }
		IPersistance Persistance { get; set; }

		Task LoadAsync();
		void StartCreation(DateTime? date);
		Task ConfirmCreationAsync(ITaskViewModel task);
		void CancelCreation(ITaskViewModel task);
		Task DeleteAsync(ITaskViewModel task);
		Task UpdateAsync(ITaskViewModel task);
	}
}