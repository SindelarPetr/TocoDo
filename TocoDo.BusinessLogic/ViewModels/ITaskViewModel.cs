using System;
using System.Windows.Input;
using TocoDo.BusinessLogic.Helpers.Commands;

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

		IAsyncCommand EditCommand            { get; }
		ICommand EditTitleCommand       { get; }
		ICommand FinishCreationCommand { get; }
		IAsyncCommand RemoveCommand          { get; }
		IAsyncCommand UpdateCommand          { get; }
	}
}