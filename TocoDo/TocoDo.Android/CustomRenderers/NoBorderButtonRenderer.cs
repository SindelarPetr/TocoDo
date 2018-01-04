using Android.Content;
using TocoDo.CustomRenderers;
using TocoDo.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Xamarin.Forms.Button;
using Color = Android.Graphics.Color;

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

			Control?.SetShadowLayer(0, 0, 0, Color.Transparent);
			Control?.SetFadingEdgeLength(0);
			Control?.SetBackground(Resources.GetDrawable("NoBorderButton"));
		}
	}
}