using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitView : ContentView, IEntryFocusable<HabitViewModel>
	{
		public HabitViewModel ViewModel
		{
			get => (HabitViewModel)BindingContext;
			set => BindingContext = value;
		}

		public HabitView(HabitViewModel habit)
		{
			ViewModel = habit;
			InitializeComponent();
		}

		private void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			ViewModel.ConfirmCreationCommand?.Execute(null);
		}

		public void FocusEntry()
		{
			EntryEditTitle.Focus();
		}
	}
}