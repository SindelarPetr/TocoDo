using System;
using System.Collections.Generic;
using System.Text;
using Plugin.CrossPlatformTintedImage.Abstractions;
using Xamarin.Forms;

namespace TocoDo.TriggerActions
{
    public class CheckboxCheckerCheckAction : TriggerAction<TintedImage>
    {
	    protected override void Invoke(TintedImage sender)
	    {
		    sender.RotationY = 90;
		    sender.RotateYTo(0);
		    sender.Scale = 2;
		    sender.ScaleTo(0.5);
		    sender.FadeTo(1);
	    }
    }
}
