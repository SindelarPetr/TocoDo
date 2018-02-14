using Plugin.CrossPlatformTintedImage.Abstractions;
using Xamarin.Forms;

namespace TocoDo.UI.TriggerActions.Checkbox
{
    public class CheckboxCheckerUncheckAction : TriggerAction<TintedImage>
    {
	    protected override void Invoke(TintedImage sender)
	    {
		    sender.FadeTo(0);
	    }
    }
}
