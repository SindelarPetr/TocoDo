using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TocoDo.Models;
using TocoDo.Services;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitView : ContentView, IEntryFocusable<HabitViewModel>
	{
		public HabitViewModel ViewModel
		{
			get => (HabitViewModel)BindingContext;
			set => BindingContext = value;
		}

		[Obsolete("Creates example HabitView")]
		public HabitView()
		{
			ViewModel = StorageService.GetExampleHabitViewModel();

			InitializeComponent();
		}

		public HabitView(HabitViewModel habit)
		{
			ViewModel = habit;
			InitializeComponent();
		}

		private void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			var title = ((Entry)e.VisualElement).Text;
			// If user left the entry blank, then remove the habit from collection
			if (string.IsNullOrWhiteSpace(title))
			{
				StorageService.RemoveHabitFromTheList(ViewModel);
				return;
			}
			
			ViewModel.InsertToStorage(title);
		}


		public void FocusEntry()
		{
			EntryEditTitle.Focus();
		}
	}
}