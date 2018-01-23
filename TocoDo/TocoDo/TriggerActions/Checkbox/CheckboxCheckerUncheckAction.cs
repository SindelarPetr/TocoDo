using System;
using System.Collections.Generic;
using System.Text;
using Plugin.CrossPlatformTintedImage.Abstractions;
using Xamarin.Forms;

namespace TocoDo.TriggerActions.Checkbox
{
    public class CheckboxCheckerUncheckAction : TriggerAction<TintedImage>
    {
	    protected override void Invoke(TintedImage sender)
	    {
		    sender.FadeTo(0);
	    }
    }
}
