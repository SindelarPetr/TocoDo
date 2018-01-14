using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitSetView : ContentView
	{
		#region HabitsSource
		private ObservableCollection<HabitViewModel> _habitSetView;
		public ObservableCollection<HabitViewModel> HabitsSource
		{
			get => _habitSetView;
			set
			{
				UnbindSource();
				_habitSetView = value;
				BindSource();
			}
		}

		private void BindSource()
		{
			HabitsSource.CollectionChanged += HabitsSourceOnCollectionChanged;
		}

		private void UnbindSource()
		{
			if (HabitsSource != null)
				HabitsSource.CollectionChanged -= HabitsSourceOnCollectionChanged;
		}

		#endregion

		public HabitSetView()
		{
			InitializeComponent();
		}

		private void HabitsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (HabitViewModel task in e.NewItems)
					{
						AddHabit(task);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (HabitViewModel task in e.OldItems)
					{
						RemoveHabit(task);
					}
					break;

			}
		}

		private void AddHabit(HabitViewModel habit)
		{
			var habitView = new HabitView(habit);
			MainLayout.Children.Add(habitView);
			if (habit.IsEditTitleMode)
				habitView.FocusEditTitleEntry();
		}

		private void RemoveHabit(HabitViewModel habit)
		{
			HabitView habitItem = FindHabitItem(habit.ModelId);

			habitItem?.FadeTo(0).ContinueWith(r =>
			{
				Debug.WriteLine("------------ Before removing the habitItem from layout");
				MainLayout.Children.Remove(habitItem);
				Debug.WriteLine("------------ After removing the habitItem from layout");
			});
		}

		private HabitView FindHabitItem(int id)
		{
			foreach (var child in MainLayout.Children)
			{
				var habit = child as HabitView;

				if (habit?.HabitViewModel.ModelId == id)
					return habit;
			}

			return null;
		}
	}
}