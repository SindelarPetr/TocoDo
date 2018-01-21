using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.TypeConverters;
using TocoDo.Views;
using Xamarin.Forms;

namespace TocoDo.TriggerActions
{
    public class IconButtonColorChangeAction : TriggerAction<IconButton>
    {
		public Color NewColor { get; set; }

		[TypeConverter(typeof(EasingConverter))]
		public Easing Easing { get; set; }

	    public IconButtonColorChangeAction()
	    {
		    NewColor = Color.Gray;
	    }

	    protected override void Invoke(IconButton sender)
	    {
		    var startColor = sender.Color;
		    var endColor = NewColor;
		    var anim = new Animation(t => sender.Color = GetDifference(startColor, endColor, t), 0, 1, Easing);
			anim.Commit(sender, "Name", 16, 500);
	    }

	    private Color GetDifference(Color start, Color end, double t)
	    {
			return new Color(start.R + (end.R - start.R) * t, start.G + (end.G - start.G) * t, start.B + (end.B - start.B) * t);
	    }
    }
}
