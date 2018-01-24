using Android.Content;
using TocoDo.CustomRenderers;
using TocoDo.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Xamarin.Forms.Button;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(FloatingAddButton), typeof(FloatingAddButtonRenderer))]
namespace TocoDo.Droid.CustomRenderers
{
	public class FloatingAddButtonRenderer : ButtonRenderer
	{
		public FloatingAddButtonRenderer(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control == null) return;

			var draw = Context.GetDrawable("CircleShape");
			Control.SetBackground(draw);
			Control.Elevation = 10;
			Control.TranslationZ = 15;
			Control.SetShadowLayer(10, 0, 0,Color.Black);
		}
	}
}