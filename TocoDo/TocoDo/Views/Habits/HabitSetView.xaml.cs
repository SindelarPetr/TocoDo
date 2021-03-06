﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitSetView : ContentView
	{
		public HabitSetView()
		{
			InitializeComponent();
		}

		private void HabitsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (HabitViewModel task in e.NewItems) AddHabit(task);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (HabitViewModel task in e.OldItems) RemoveHabit(task);
					break;
			}
		}

		private void AddHabit(HabitViewModel habit)
		{
			var habitView = new HabitView(habit);
			MainLayout.Children.Add(habitView);
			if (habit.IsCreateMode)
				habitView.FocusEntry();
		}

		private void RemoveHabit(HabitViewModel habit)
		{
			var habitItem       = FindHabitItem(habit);
			habitItem.IsVisible = false;
		}

		private HabitView FindHabitItem(HabitViewModel id)
		{
			foreach (var child in MainLayout.Children)
			{
				var habit = child as HabitView;

				if (habit?.ViewModel == id)
					return habit;
			}

			return null;
		}

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
	}
}