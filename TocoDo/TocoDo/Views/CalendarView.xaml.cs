﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using NetBox.Extensions;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PropertyChangingEventArgs = System.ComponentModel.PropertyChangingEventArgs;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarView : ContentView
	{
		#region Backing fields
		private static readonly BindablePropertyKey HighlightedDatePropertyKey = BindableProperty.CreateReadOnly(nameof(HighlightedDate), typeof(DateTime), typeof(DateTime), DateTime.Today);

		public static BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(DateTime?));
		public static BindableProperty HighlightedDateProperty = HighlightedDatePropertyKey.BindableProperty;
		public static BindableProperty HabitsSourceProperty = BindableProperty.Create(nameof(HabitsSource), typeof(ReadOnlyObservableCollection<IHabitViewModel>), typeof(ReadOnlyObservableCollection<IHabitViewModel>));
		public static BindableProperty TasksSourceProperty = BindableProperty.Create(nameof(TasksSource), typeof(ReadOnlyObservableCollection<ITaskViewModel>), typeof(ReadOnlyObservableCollection<ITaskViewModel>));

		private CalendarCell _selectedCell;
		private ICommand _moveNextCommand;
		private ICommand _movePrevCommand;
		private DateTime _firstDayDate;
		private ObservableCollection<ITaskViewModel> _selectedDayTasks;
		private ObservableCollection<IHabitViewModel> _selectedDayHabits;
		#endregion

		#region Properties

		public ReadOnlyObservableCollection<ITaskViewModel> SelectedDayTasks { get; }
		public ReadOnlyObservableCollection<IHabitViewModel> SelectedDayHabits { get; }

		public DateTime HighlightedDate
		{
			get => (DateTime)GetValue(HighlightedDateProperty);
			set => SetValue(HighlightedDatePropertyKey, value);
		}
		public DateTime? SelectedDate
		{
			get => (DateTime?)GetValue(SelectedDateProperty);
			set => SetValue(SelectedDateProperty, value?.Date);
		}

		private DateTime LastDayDate => _firstDayDate.AddDays(3 * 7);

		#region Sources
		public ReadOnlyObservableCollection<IHabitViewModel> HabitsSource
		{
			get => (ReadOnlyObservableCollection<IHabitViewModel>)GetValue(HabitsSourceProperty);
			set => SetValue(HabitsSourceProperty, value);
		}
		public ReadOnlyObservableCollection<ITaskViewModel> TasksSource
		{
			get => (ReadOnlyObservableCollection<ITaskViewModel>)GetValue(TasksSourceProperty);
			set => SetValue(TasksSourceProperty, value);
		}
		#endregion


		#region Commands
		public ICommand MoveNextCommand => _moveNextCommand ?? (_moveNextCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(() => SetupCalendarGrid(_firstDayDate.AddDays(3 * 7))));
		public ICommand MovePrevCommand => _movePrevCommand ?? (_movePrevCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(() => SetupCalendarGrid(_firstDayDate.AddDays(-3 * 7))));
		#endregion
		#endregion

		public CalendarView()
		{
			MyLogger.WriteStartMethod();

			MyLogger.WriteInMethod("Before initialize component.");
			InitializeComponent();

			MyLogger.WriteInMethod("Initializing Tasks and Habits in The SelectedDate collections");
			_selectedDayHabits = new ObservableCollection<IHabitViewModel>();
			SelectedDayHabits = new ReadOnlyObservableCollection<IHabitViewModel>(_selectedDayHabits);

			_selectedDayTasks = new ObservableCollection<ITaskViewModel>();
			SelectedDayTasks = new ReadOnlyObservableCollection<ITaskViewModel>(_selectedDayTasks);

			MyLogger.WriteInMethod("Before setup of calendar.");
			SetupCalendarGrid(DateTime.Today).ContinueWith(r => SelectedDate = DateTime.Today);

			MyLogger.WriteEndMethod();
		}

		#region Property change
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);

			switch (propertyName)
			{
				case nameof(HabitsSource): HabitsSourceChanging(); break;
				case nameof(TasksSource): TasksSourceChanging(); break;
			}
		}
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			switch (propertyName)
			{
				case nameof(SelectedDate): SelectedDateChanged(); break;
				case nameof(HighlightedDate): HighlightedMonthChanged(); break;
				case nameof(HabitsSource): HabitsSourceChanged(); break;
				case nameof(TasksSource): TasksSourceChanged(); break;
			}

		}
		#endregion

		private void SelectedDateChanged()
		{
			// Unselect previously selected cell
			if (_selectedCell != null)
			{
				_selectedCell.IsSelected = false;
				_selectedCell = null;
			}

			if (SelectedDate == null)
			{
				RefreshSelectedDayCollections();
				return;
			}

			// Find the cell with the SelectedDate, select it and save it to _selectedDate
			// The cell can be in just in the column which corresonds to its day in week
			int column = SelectedDate.Value.ZeroMondayBasedDay();

			// Iterate the rows in the column and find the cell with the right date
			int childrenCount = CalendarGrid.Children.Count;
			for (int i = column; i < childrenCount; i += 7)
			{
				if (CalendarGrid.Children[i] is CalendarCell cell && cell.Date == SelectedDate)
				{
					// FOUND THE RIGHT CELL -> lets select it
					cell.IsSelected = true;
					_selectedCell = cell;

					// if the newly selected cell is in different month than which is highlighted, then highlight the new month
					HighlightedDate = SelectedDate.Value;
					RefreshSelectedDayCollections();
					return;
				}
			}
		}
		private void HighlightedMonthChanged()
		{
			foreach (var child in CalendarGrid.Children)
			{
				if (!(child is CalendarCell cell))
					continue;

				cell.IsSideMonth = cell.Date.Month != HighlightedDate.Month;
			}
		}

		#region Manage SelectedDay Collections
		private void RefreshSelectedDayCollections()
		{
			RefreshSelectedDayTasks();
			RefreshSelectedDayHabits();
		}

		private void RefreshSelectedDayTasks()
		{
			//if (_selectedDayTasks == null)
			//	return;

			_selectedDayTasks.Clear();
			if(SelectedDate != null)
			foreach (var task in TasksSource)
				if (task.ScheduleDate != null && task.ScheduleDate.Value == SelectedDate)
					_selectedDayTasks.Add(task);

		}

		private void RefreshSelectedDayHabits()
		{
			//if (_selectedDayHabits == null)
			//	return;

			_selectedDayHabits.Clear();
			if(SelectedDate != null)
			foreach (var habit in HabitsSource)
			{
				if (habit.StartDate != null && habit.IsActive(habit.StartDate.Value))
					_selectedDayHabits.Add(habit);
			}
		}

		private void RefreshTaskInSelectedTasks(ITaskViewModel task)
		{
			//if(_selectedDayTasks == null)
			//	return;

			if (SelectedDate == null)
			{
				_selectedDayTasks.Remove(task);
				return;
			}

			// if the task is active but it is not in the collection -> add it
			if (task.ScheduleDate != null && task.ScheduleDate == SelectedDate && !_selectedDayTasks.Contains(task))
				_selectedDayTasks.Add(task);
			// if the task is not active but it is in the collection -> remove it
			else if (task.ScheduleDate == null || task.ScheduleDate != SelectedDate)
				_selectedDayTasks.Remove(task);
		}
		#endregion

		#region Setup
		private async Task SetupCalendarGrid(DateTime date)
		{
			// Create 3 week calendar
			MyLogger.WriteStartMethod();

			CalendarGrid.Scale = 0.75;

			// Clean the calendar
			SelectedDate = null;

			int rows = 3;
			int daysInWeek = 7;

			DateTime firstDay = _firstDayDate = date.AddDays(-date.ZeroMondayBasedDay());

			for (int i = 0; i < rows * daysInWeek; i++)
			{
				DateTime currentDate = firstDay.AddDays(i);
				int row = i / daysInWeek;
				int column = currentDate.ZeroMondayBasedDay();

				CreateCell(currentDate, column, row, date.Month != currentDate.Month && _selectedCell != null);
			}

			HighlightedDate = ((CalendarCell)CalendarGrid.Children[CalendarGrid.Children.Count / 2]).Date;

			MyLogger.WriteInMethod("Before Fill Tasks Busyness");
			FillTasksBusyness();
			MyLogger.WriteInMethod("Before Fill Habits Busyness");
			FillHabitsBusyness();


			await CalendarGrid.ScaleTo(1, 250, Easing.CubicOut);
			MyLogger.WriteEndMethod();
		}

		private void FillHabitsBusyness()
		{
			MyLogger.WriteStartMethod();
			if (HabitsSource != null)
				foreach (var habit in HabitsSource)
				{
					IncreaseBusyness(habit);
				}
			MyLogger.WriteEndMethod();
		}

		private void CreateCell(DateTime date, int column, int row, bool isSide)
		{
			var today = DateTime.Today;
			if (CalendarGrid.Children.Count != 7 * 3)
			{
				MyLogger.WriteInMethod("Creating a new cell");
				var cell = new CalendarCell(date)
				{
					TappedCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(c => { SelectedDate = ((CalendarCell)c).Date; }),
					IsSideMonth = isSide
				};

				MyLogger.WriteInMethod("Before adding the cell to the children");
				CalendarGrid.Children.Add(cell, column, row);
				cell.IsToday = date == today;
				MyLogger.WriteInMethod("After adding the cell to the children");
			}
			else
			{
				MyLogger.WriteInMethod("Reusing a cell");
				// Reuse of the previous cell
				if (CalendarGrid.Children[row * 7 + column] is CalendarCell cell)
				{
					cell.Date = date;
					cell.Busyness = 0;
					cell.IsSideMonth = isSide;
					cell.IsToday = date == today;
					cell.IsSelected = false;
				}
			}
		}
		#endregion

		#region Habits
		private void HabitsSourceChanging()
		{
			if (HabitsSource != null)
				((INotifyCollectionChanged)HabitsSource).CollectionChanged -= OnHabitsSourceCollectionChanged;
		}
		private void HabitsSourceChanged()
		{
			if (HabitsSource != null)
			{
				((INotifyCollectionChanged)HabitsSource).CollectionChanged += OnHabitsSourceCollectionChanged;

				FillHabitsBusyness();
			}
		}
		private void OnHabitsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			switch (notifyCollectionChangedEventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (IHabitViewModel habit in notifyCollectionChangedEventArgs.NewItems)
					{
						IncreaseBusyness(habit);
						habit.PropertyChanging += HabitOnPropertyChanging;
						habit.PropertyChanged += HabitOnPropertyChanged;
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (IHabitViewModel habit in notifyCollectionChangedEventArgs.OldItems)
					{
						DecreaseBusyness(habit);
						habit.PropertyChanging -= HabitOnPropertyChanging;
						habit.PropertyChanged -= HabitOnPropertyChanged;
					}
					break;
			}
		}

		private void HabitOnPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == nameof(IHabitViewModel.RepeatType) || e.PropertyName == nameof(IHabitViewModel.StartDate) || e.PropertyName == nameof(IHabitViewModel.DaysToRepeat))
				DecreaseBusyness((IHabitViewModel)sender);
		}

		private void HabitOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(IHabitViewModel.RepeatType) || e.PropertyName == nameof(IHabitViewModel.StartDate) ||
				e.PropertyName == nameof(IHabitViewModel.DaysToRepeat))
			{
				IncreaseBusyness((IHabitViewModel)sender);
			}
		}

		#endregion

		#region Tasks
		private void TasksSourceChanging()
		{
			if (TasksSource != null)
				((INotifyCollectionChanged)TasksSource).CollectionChanged -= OnTasksSourceCollectionChanged;
		}
		private void TasksSourceChanged()
		{
			if (TasksSource != null)
			{
				((INotifyCollectionChanged)TasksSource).CollectionChanged += OnTasksSourceCollectionChanged;

				FillTasksBusyness();
			}
		}
		private void OnTasksSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			switch (notifyCollectionChangedEventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (ITaskViewModel task in notifyCollectionChangedEventArgs.NewItems)
					{
						IncreaseBusyness(task.ScheduleDate);
						task.PropertyChanging += TaskOnPropertyChanging;
						task.PropertyChanged += TaskOnPropertyChanged;

						RefreshTaskInSelectedTasks(task);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (ITaskViewModel task in notifyCollectionChangedEventArgs.OldItems)
					{
						DecreaseBusyness(task.ScheduleDate);
						task.PropertyChanging -= TaskOnPropertyChanging;
						task.PropertyChanged -= TaskOnPropertyChanged;

						if (_selectedDayTasks.Contains(task))
							_selectedDayTasks.Remove(task);
					}
					break;
			}
		}
		private void TaskOnPropertyChanging(object sender, PropertyChangingEventArgs propertyChangingEventArgs)
		{
			if (propertyChangingEventArgs.PropertyName == nameof(ITaskViewModel.ScheduleDate) && sender is ITaskViewModel task)
			{
				DecreaseBusyness(task.ScheduleDate);
			}
		}
		private void TaskOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(ITaskViewModel.ScheduleDate) && sender is ITaskViewModel task)
			{
				IncreaseBusyness(task.ScheduleDate);
				RefreshTaskInSelectedTasks(task);
			}
		}
		#endregion

		private CalendarCell FindCell(DateTime? nullableDate)
		{
			if (nullableDate == null)
				return null;

			DateTime date = nullableDate.Value.Date;

			// Calculate row of the schedule date
			var row = (date.Date - _firstDayDate.Date).TotalDays / 7;
			if (row < 0 || row >= 3)
				return null;

			var column = date.ZeroMondayBasedDay();
			if (CalendarGrid.Children[column + (int)row * 7] is CalendarCell cell)
			{
				return cell;
			}

			return null;
		}
		private void DecreaseBusyness(IHabitViewModel habit)
		{
			if (habit.StartDate == null) return;

			// Iterates all cells and increases their busyness by one if the habit is active on the cells date
			foreach (var child in CalendarGrid.Children)
			{
				if (child is CalendarCell cell && habit.IsActive(cell.Date))
					cell.Busyness--;
			}
		}
		private void DecreaseBusyness(DateTime? date)
		{
			var cell = FindCell(date);

			if (cell != null)
				cell.Busyness--;
		}

		private void IncreaseBusyness(IHabitViewModel habit)
		{
			MyLogger.WriteStartMethod();
			if (habit.StartDate == null)
			{
				MyLogger.WriteEndMethod("Ending soon because the habit has no StartDate");
				return;
			}

			// Iterates all cells and increases their busyness by one if the habit is active on the cells date
			foreach (var child in CalendarGrid.Children)
			{
				if (child is CalendarCell cell && habit.IsActive(cell.Date))
				{
					MyLogger.WriteInMethod("Increasing Busyness in a cell");
					cell.Busyness++;
				}
			}

			MyLogger.WriteEndMethod();
		}
		private void IncreaseBusyness(DateTime? date)
		{
			var cell = FindCell(date);

			if (cell != null)
				cell.Busyness++;
		}

		private void FillTasksBusyness()
		{
			if (TasksSource != null)
				foreach (var taskViewModel in TasksSource)
				{
					IncreaseBusyness(taskViewModel.ScheduleDate);
				}
		}

	}
}