using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TocoDo.TriggerActions
{
    public class FocusAction : TriggerAction<VisualElement>
    {
		public VisualElement ElementToFocus { get; set; }

	    protected override void Invoke(VisualElement sender)
	    {
		    ElementToFocus.Focus();
	    }
    }
}
