using System;
using System.Windows.Input;

namespace TocoDo.BusinessLogic.ViewModels
{
	public interface ITaskViewModel : ICreateMode
	{
		DateTime CreateTime { get; }
		DateTime? Deadline { get; set; }
		string Description { get; set; }
		DateTime? Done { get; set; }
		ICommand EditCommand { get; set; }
		ICommand EditDescriptionCommand { get; set; }
		ICommand EditTitleCommand { get; set; }
		int Id { get; set; }
		DateTime? Reminder { get; set; }
		ICommand RemoveCommand { get; set; }
		DateTime? ScheduleDate { get; set; }
		string Title { get; set; }
		ICommand UpdateCommand { get; set; }
	}
}