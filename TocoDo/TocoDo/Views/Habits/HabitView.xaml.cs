using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitView : ContentView, IEntryFocusable<HabitViewModel>
	{
		public HabitView(HabitViewModel habit)
		{
			ViewModel = habit;
			InitializeComponent();
		}

		public HabitViewModel ViewModel
		{
			get => (HabitViewModel) BindingContext;
			set => BindingContext = value;
		}

		public void FocusEntry()
		{
			EntryEditTitle.Focus();
		}

		private void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			ViewModel.ConfirmCreationCommand?.Execute(null);
		}
	}
}