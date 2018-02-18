using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.DependencyInjection.Models
{
	public interface IModelFactory
	{
		ITaskModel  CreateTaskModel(ITaskViewModel task);
		IHabitModel CreateHabitModel(IHabitViewModel habit);
	}
}