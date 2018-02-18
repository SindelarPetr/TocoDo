using Xamarin.Forms;

namespace TocoDo.UI.TriggerActions
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