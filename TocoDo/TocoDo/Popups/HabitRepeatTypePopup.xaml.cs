using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using TocoDo.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitRepeatTypePopup : PopupPage
	{
		public static BindableProperty SelectedRepeatTypeProperty = BindableProperty.Create(nameof(SelectedRepeatType), typeof(RepeatType), typeof(RepeatType), RepeatType.Days);

		public RepeatType SelectedRepeatType
		{
			get => (RepeatType) GetValue(SelectedRepeatTypeProperty);
			set => SetValue(SelectedRepeatTypeProperty, value);
		}

		public HabitRepeatTypePopup ()
		{
			InitializeComponent ();
		}
	}
}