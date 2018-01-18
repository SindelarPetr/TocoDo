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
	public partial class HabitView : ContentView
	{
		public HabitViewModel HabitViewModel
		{
			get => (HabitViewModel)BindingContext;
			set => BindingContext = value;
		}

		[Obsolete("Creates example HabitView")]
		public HabitView()
		{
			HabitViewModel = StorageService.GetExampleHabitViewModel();

			InitializeComponent();
		}

		public HabitView(HabitViewModel habit)
		{
			HabitViewModel = habit;
			InitializeComponent();
		}

		private void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			Debug.WriteLine("---------- Onunfocused of EditTitle called");

			Debug.WriteLine("---------- Getting the title of entry element in EditTitle_OnUnfocused");
			var title = ((Entry)e.VisualElement).Text;
			// If user left the entry blank, then remove the habit from collection
			if (string.IsNullOrWhiteSpace(title))
			{
				StorageService.RemoveHabitFromTheList(HabitViewModel);
				return;
			}

			
			Debug.WriteLine("-------------- Got title of the Entry: " + title);
			HabitViewModel.InsertToStorage(title).Wait();
			Debug.WriteLine("---------- Finished calling of EditTitle");
		}

		public void FocusEditTitleEntry()
		{
			EntryEditTitle.Focus();
		}
	}
}