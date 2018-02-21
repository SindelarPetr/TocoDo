using System;
using System.Windows.Input;

namespace TocoDo.BusinessLogic.ViewModels
{
	public interface ITaskViewModel : ICreateMode
	{
		int       Id           { get; set; }
		DateTime? Reminder     { get; set; }
		DateTime? ScheduleDate { get; set; }
		string    Title        { get; set; }
		DateTime  CreateTime   { get; }
		DateTime? Deadline     { get; set; }
		string    Description  { get; set; }
		DateTime? Done         { get; set; }

		ICommand EditCommand            { get; }
		ICommand EditTitleCommand       { get; }
		ICommand FinishCreationCommand { get; }
		ICommand RemoveCommand          { get; }
		ICommand UpdateCommand          { get; }
	}
}