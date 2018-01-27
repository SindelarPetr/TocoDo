using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TocoDo.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DescriptionFrameView : ContentView
	{
		#region Backing fields
		public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(string));
		public static BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(string), null, BindingMode.TwoWay);
		#endregion

		#region Properties
		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}
		public string Description
		{
			get => (string)GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		public bool IsReadonly { get; set; }

		#endregion

		public DescriptionFrameView ()
		{
			InitializeComponent ();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			var editDescriptionPage = new EditDescriptionPage(Title, Description, d => Description = d, IsReadonly);

			Navigation.PushModalAsync(editDescriptionPage);
		}
	}
}