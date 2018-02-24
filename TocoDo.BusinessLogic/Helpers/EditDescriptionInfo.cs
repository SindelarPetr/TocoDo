using System;
using System.Collections.Generic;
using System.Text;

namespace TocoDo.BusinessLogic.Helpers
{
    public struct EditDescriptionInfo
    {
	    public string Title { get; }
		public string Description { get; }
		public Action<string> DescriptionSetter { get; }
	    public bool IsReadonly { get; }

	    public EditDescriptionInfo(string title, string description, Action<string> descriptionSetter, bool isReadonly)
	    {
		    Title = title;
		    Description = description;
		    DescriptionSetter = descriptionSetter;
		    IsReadonly = isReadonly;
	    }
    }
}
