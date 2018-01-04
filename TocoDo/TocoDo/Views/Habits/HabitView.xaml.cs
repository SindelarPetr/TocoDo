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
	}
}