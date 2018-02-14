using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.DependencyInjection
{
	public enum PageType {
		EditDescriptionPage,
		ModifyHabitPage,
		HabitProgressPage,
		ModifyTaskPage
	}
	public enum PopupType { }

    public interface INavigationService
    {
	    Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
	    Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons);

		Task PushAsync(PageType page, object param = null);
	    Task PopAsync();

	    Task PushModalAsync(PageType page, object param = null);
	    Task PopModalAsync();

	    Task PushPopupAsync(PopupType page, object param = null);
	    Task PopPopupAsync();
    }
}
