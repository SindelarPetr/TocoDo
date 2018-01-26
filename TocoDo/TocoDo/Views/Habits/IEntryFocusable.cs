using System;
using System.Collections.Generic;
using System.Text;

namespace TocoDo.Views.Habits
{
    public interface IEntryFocusable<TViewModel>
    {
	    void FocusEntry();
		TViewModel ViewModel { get; set; }

    }
}
