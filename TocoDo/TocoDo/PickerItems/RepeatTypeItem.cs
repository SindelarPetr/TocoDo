using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.Models;

namespace TocoDo.PickerItems
{
    public class RepeatTypeItem
    {
	    public RepeatType RepeatType { get; set; }
	    public string Text { get; }

	    public RepeatTypeItem(RepeatType repeatType, string text)
	    {
		    RepeatType = repeatType;
		    Text = text;
	    }
    }
}
