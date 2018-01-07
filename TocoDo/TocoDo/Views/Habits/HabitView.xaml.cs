using System.Diagnostics;
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

		public HabitView(HabitViewModel habit)
		{
			HabitViewModel = habit;
			InitializeComponent();
		}

		protected override void OnParentSet()
		{
			Debug.WriteLine("------------ OnParentSet of a habit called.");
			base.OnParentSet();

			//if (HabitViewModel.IsEditTitleMode)
			//	EntryEditTitle.Focus();
			Debug.WriteLine("------------ Finished calling of OnParentSet of a habit.");
		}

		private async void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			Debug.WriteLine("---------- Onunfocused of EditTitle called");
			var title = ((Entry)e.VisualElement).Text;
			Debug.WriteLine("-------------- Got title of the Entry: " + title);
			await HabitViewModel.InsertToStorage(title);
			Debug.WriteLine("---------- Finished calling of EditTitle");
		}

		public void FocusEditTitleEntry()
		{
			EntryEditTitle.Focus();
		}
	}
}