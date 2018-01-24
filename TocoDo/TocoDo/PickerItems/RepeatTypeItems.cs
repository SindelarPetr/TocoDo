using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.Models;
using TocoDo.Properties;

namespace TocoDo.PickerItems
{
    public class RepeatTypeItems : List<RepeatTypeItem>
    {
	    public RepeatTypeItem WeeksItem { get; }
	    public RepeatTypeItems()
	    {
			WeeksItem = new RepeatTypeItem(RepeatType.Mon | RepeatType.Tue | RepeatType.Wed, Resources.Week);
		    Add(new RepeatTypeItem(RepeatType.Days, Resources.Day));
		    Add(new RepeatTypeItem(RepeatType.Months, Resources.Month));
		    Add(new RepeatTypeItem(RepeatType.Years, Resources.Year));
	    }
    }
}
