using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitDetailView : ContentView
	{
		#region Static

		public static BindableProperty ViewModelProperty =
			BindableProperty.Create(nameof(ViewModel), typeof(HabitViewModel), typeof(HabitViewModel));

		#endregion

		#region Properties

		public HabitViewModel ViewModel
		{
			get => (HabitViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		#endregion

		public HabitDetailView()
		{
			InitializeComponent();
		}
	}
}