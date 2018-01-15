using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyHabitPage : ContentPage
	{
		public HabitViewModel Habit
		{
			get => (HabitViewModel)BindingContext;
			set => BindingContext = value;
		}

		public ModifyHabitPage(HabitViewModel habit)
		{
			Habit = habit;
			InitializeComponent();
		}

		private void EntryTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			var title = EntryTitle.Text.Trim();

			if (string.IsNullOrWhiteSpace(title))
				return;

			Habit.ModelTitle = title;
		}
	}
}