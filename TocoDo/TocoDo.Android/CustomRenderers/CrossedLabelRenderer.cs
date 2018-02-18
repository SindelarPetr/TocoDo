using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using TocoDo.Droid.CustomRenderers;
using TocoDo.UI.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CrossedLabel), typeof(CrossedLabelRenderer))]

namespace TocoDo.Droid.CustomRenderers
{
	public class CrossedLabelRenderer : LabelRenderer
	{
		public CrossedLabelRenderer(Context con) : base(con)
		{
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			StrikeThrough();
		}

		//protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		//{
		//	base.OnElementChanged(e);
		//}

		private void StrikeThrough()
		{
			var crossedLabel = (CrossedLabel) Element;
			if (Control == null || crossedLabel == null) return;


			if (crossedLabel.IsStrikeThrough)
				Control.PaintFlags = Control.PaintFlags | PaintFlags.StrikeThruText;
			else
				Control.PaintFlags &= ~PaintFlags.StrikeThruText;
		}
	}
}