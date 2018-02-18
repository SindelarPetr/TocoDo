using System;
using System.Collections.Generic;
using System.Text;

namespace TocoDo.BusinessLogic.Helpers
{
    public class RealDateTimeProvider : IDateTimeProvider
    {
	    public DateTime Now => DateTime.Now;
    }
}
