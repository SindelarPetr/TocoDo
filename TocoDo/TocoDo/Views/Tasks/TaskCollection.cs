using System.Collections.ObjectModel;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;

namespace TocoDo.UI.Views.Todos
{
	public class TaskCollection : ItemCollection<ITaskViewModel, TaskView>
	{
		public TaskCollection() : base((vm, pram) => new TaskView(vm), nameof(ITaskViewModel.ScheduleDate))
		{
			MyLogger.WriteStartMethod();
			MyLogger.WriteEndMethod();
		}
	}
}