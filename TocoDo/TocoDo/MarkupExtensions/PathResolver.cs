using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.MarkupExtensions
{
	[ContentProperty("Source")]
	public class PathResolver : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Device.RuntimePlatform == Device.UWP)
				return "Content/" + Source;

			return Source;
		}
	}
}
