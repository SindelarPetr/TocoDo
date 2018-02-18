using System.Collections.ObjectModel;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;

namespace TocoDo.UI.Views.Todos
{
	public class TaskCollection : ItemCollection<ITaskViewModel, TodoItemView>
	{
		public TaskCollection() : base(vm => new TodoItemView(vm))
		{
			MyLogger.WriteStartMethod();
			MyLogger.WriteEndMethod();
		}
	}
}