﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.MarkupExtensions
{
	[ContentProperty("ResourceId")]
	public class EmbeddedImage : IMarkupExtension
	{
		public string ResourceId { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (string.IsNullOrWhiteSpace(ResourceId)) return null;

			return ImageSource.FromResource(ResourceId);
		}
	}
}