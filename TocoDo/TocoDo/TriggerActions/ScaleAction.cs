using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.UI.TypeConverters;
using Xamarin.Forms;

namespace TocoDo.UI.TriggerActions
{
    public class ScaleAction : TriggerAction<VisualElement>
    {
	    public int Duration { get; set; }
		public double TargetScale { get; set; }
	    [TypeConverter(typeof(EasingConverter))]
		public Easing Easing { get; set; }

	    public ScaleAction()
	    {
		    Duration = 250;
		    TargetScale = 1.5;
			Easing = Easing.CubicOut;
	    }


	    protected override void Invoke(VisualElement sender)
	    {
		    sender.ScaleTo(TargetScale, (uint)Duration, Easing);
	    }
    }
}
