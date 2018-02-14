using Plugin.CrossPlatformTintedImage.Abstractions;
using Xamarin.Forms;

namespace TocoDo.UI.TriggerActions.Checkbox
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
