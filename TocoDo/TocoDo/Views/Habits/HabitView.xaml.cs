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

		private async void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			Debug.WriteLine("---------- Onunfocused of EditTitle called");
			var title = ((Entry)e.VisualElement).Text;
			Debug.WriteLine("-------------- Got title of the Entry: " + title);
			await HabitViewModel.InsertToStorage(title);
			Debug.WriteLine("---------- Finished calling of EditTitle");
		}
	}
}