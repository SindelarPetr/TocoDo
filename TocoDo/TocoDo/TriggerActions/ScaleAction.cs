﻿using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.Views;
using Xamarin.Forms;

namespace TocoDo.TriggerActions
{
    public class ScaleAction : TriggerAction<IconButton>
    {
		public double MaxScale { get; set; }
		public double Duration { get; set; }

	    public ScaleAction()
	    {
		    MaxScale = 1.5;
		    Duration = 250;
	    }

	    protected override void Invoke(IconButton sender)
	    {
		    sender.ScaleTo(MaxScale, (uint)Duration, new Easing(t => 1 + Math.Sin(t * Math.PI) * MaxScale));
	    }
    }
}