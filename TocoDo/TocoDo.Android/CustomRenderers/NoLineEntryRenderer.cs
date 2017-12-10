using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TocoDo.CustomRenderers;
using TocoDo.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(NoLineEntry), typeof(NoLineEntryRenderer))]
namespace TocoDo.Droid.CustomRenderers
{
	public class NoLineEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if(Control == null) return;

			var shape = new ShapeDrawable(new RectShape());
			shape.Paint.Alpha = 0;
			shape.Paint.SetStyle(Paint.Style.Stroke);
			Control.SetBackground(shape);
		}
	}
}