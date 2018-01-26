using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.ViewModels;

namespace TocoDo.Views.Todos
{
    public class TaskCollection : ItemCollection<TaskViewModel, TodoItemView>
    {
	    public TaskCollection() : base(vm => new TodoItemView(vm))
	    {

	    }
    }
}
