using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TocoDo.TriggerActions
{
    public class IsVisibleSettingAction : TriggerAction<VisualElement>
    {
	    public VisualElement Element { get; set; }

	    protected override void Invoke(VisualElement sender)
	    {
		    Element.IsVisible = true;
	    }
    }
}
