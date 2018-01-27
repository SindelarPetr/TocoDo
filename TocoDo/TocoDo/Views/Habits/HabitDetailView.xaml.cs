using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitDetailView : ContentView
	{
		public static BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(HabitViewModel), typeof(HabitViewModel));
		public HabitViewModel ViewModel
		{
			get => (HabitViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		public HabitDetailView ()
		{
			InitializeComponent ();
		}
	}
}