using Android.Content;
using TocoDo.Droid.CustomRenderers;
using TocoDo.UI.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Xamarin.Forms.Button;

[assembly: ExportRenderer(typeof(NoBorderButton), typeof(NoBorderButtonRenderer))]
namespace TocoDo.Droid.CustomRenderers
{
	public class NoBorderButtonRenderer : ButtonRenderer
	{
		public NoBorderButtonRenderer(Context context) : base(context)
		{ }

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control == null) return;

			//Control.SetShadowLayer(0, 0, 0, Color.Transparent);
			//Control.SetFadingEdgeLength(0);
			//Control.SetBackground(Context.GetDrawable("NoBorderButton"));
		}
	}
}