using Xamarin.Forms;

namespace TocoDo.UI.TriggerActions
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
