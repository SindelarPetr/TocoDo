using System;
using System.Collections.Generic;
using System.Text;

namespace TocoDo.BusinessLogic.Helpers
{
    public interface IDateTimeProvider
    {
		DateTime Now { get; }
    }
}
