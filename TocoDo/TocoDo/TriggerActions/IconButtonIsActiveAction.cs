using System;
using Xamarin.Forms;

namespace TocoDo.UI.TriggerActions
{
    public class ShakeAction : TriggerAction<VisualElement>
    {
	    public double ShakeRange { get; set; }
		public int Duration { get; set; }
		public int OscilationsCount { get; set; }

	    public ShakeAction()
	    {
		    ShakeRange = 10;
		    Duration = 500;
		    OscilationsCount = 4;
	    }

	    protected override void Invoke(VisualElement sender)
	    {
		    sender.TranslateTo(20, sender.TranslationY, (uint)Duration,new Easing(t => (1 - t) * Math.Sin(OscilationsCount * Math.PI * t)));
	    }
    }
}
