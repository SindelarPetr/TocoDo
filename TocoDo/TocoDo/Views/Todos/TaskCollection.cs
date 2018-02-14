using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;

namespace TocoDo.UI.Views.Todos
{
    public class TaskCollection : ItemCollection<TaskViewModel, TodoItemView>
    {
	    public TaskCollection() : base(vm => new TodoItemView(vm))
	    {

	    }
    }
}
