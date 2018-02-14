using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.DependencyInjection.Models
{
    public interface IModelFactory
    {
	    ITaskModel CreateTaskModel(TaskViewModel task);
	    IHabitModel CreateHabitModel(HabitViewModel habit);
    }
}
