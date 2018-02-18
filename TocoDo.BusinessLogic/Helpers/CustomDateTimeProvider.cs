using System;
using System.Collections.Generic;
using System.Text;

namespace TocoDo.BusinessLogic.Helpers
{
    public class CustomDateTimeProvider : IDateTimeProvider
    {
	    public DateTime Now { get; set; }

	    public CustomDateTimeProvider( DateTime dateTime )
	    {
		    Now = dateTime;
	    }
    }
}
