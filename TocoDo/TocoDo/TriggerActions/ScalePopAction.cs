using System;
using TocoDo.UI.Views;
using Xamarin.Forms;

namespace TocoDo.UI.TriggerActions
{
	public class ScalePopAction : TriggerAction<VisualElement>
	{
		public ScalePopAction()
		{
			MaxScale = 1.5;
			Duration = 250;
		}

		public double MaxScale { get; set; }
		public double Duration { get; set; }

		protected override void Invoke(VisualElement sender)
		{
			sender.ScaleTo(MaxScale, (uint) Duration, new Easing(t => 1 + Math.Sin(t * Math.PI) * MaxScale));
		}
	}
}