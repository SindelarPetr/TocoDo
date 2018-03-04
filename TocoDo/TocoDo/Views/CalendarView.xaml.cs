using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
		private static readonly BindablePropertyKey HighlightedMonthPropertyKey = BindableProperty.CreateReadOnly(nameof(HighlightedMonth), typeof(int), typeof(int), -1);

		public static BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(DateTime?));
		public static BindableProperty HighlightedMonthProperty = HighlightedMonthPropertyKey.BindableProperty;
		public static BindableProperty HabitsSourceProperty = BindableProperty.Create(nameof(HabitsSource), typeof(ReadOnlyObservableCollection<IHabitViewModel>), typeof(ReadOnlyObservableCollection<IHabitViewModel>));
		public static BindableProperty TasksSourceProperty = BindableProperty.Create(nameof(TasksSource), typeof(ReadOnlyObservableCollection<ITaskViewModel>), typeof(ReadOnlyObservableCollection<ITaskViewModel>));

		private CalendarCell _selectedCell;
		private ICommand _moveNextCommand;
		private ICommand _movePrevCommand;
		private DateTime _firstDayDate;

		#endregion

		#region Properties
		public int HighlightedMonth
		{
			get => (int)GetValue(HighlightedMonthProperty);
			set => SetValue(HighlightedMonthPropertyKey, value);
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

			MyLogger.WriteInMethod("Before setup of calendar.");
			SetupCalendarGrid(DateTime.Today);
			SelectedDate = DateTime.Today;

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
				case nameof(HighlightedMonth): HighlightedMonthChanged(); break;
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
				return;

			// Find the cell with the SelectedDate, select it and save it to _selectedDate
			// The cell can be in just in the column which corresonds to its day in week
			int column = SelectedDate.Value.ZeroMondayBasedDay();

			// Iterate the rows in the column and find the cell with the right date
			int childrenCount = CalendarGrid.Children.Count;
			for (int i = column; i < childrenCount; i += 7)
			{
				if (CalendarGrid.Children[i] is CalendarCell cell && cell.Date == SelectedDate)
				{
					// Found the right cell -> lets select it
					cell.IsSelected = true;
					_selectedCell = cell;

					// if the newly selected cell is in different month than which is highlighted, then highlight the new month
					HighlightedMonth = SelectedDate.Value.Month;
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

				cell.IsSideMonth = cell.Date.Month != HighlightedMonth;
			}
		}

		#region Setup
		private void SetupCalendarGrid(DateTime date)
		{
			// Create 3 week calendar
			MyLogger.WriteStartMethod();

			// Clean the calendar
			_selectedCell = null;
			CalendarGrid.RowDefinitions.Clear();
			CalendarGrid.Children.Clear();
			HighlightedMonth = -1;
			MyLogger.WriteInMethod("After clearing the calendar grid.");

			int rows = 3;
			int daysInWeek = 7;

			// Create row definitions
			for (int i = 0; i < rows; i++)
				CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

			DateTime firstDay = _firstDayDate = date.AddDays(-date.ZeroMondayBasedDay());

			for (int i = 0; i < rows * daysInWeek; i++)
			{
				DateTime currentDate = firstDay.AddDays(i);
				int row = i / daysInWeek;
				int column = currentDate.ZeroMondayBasedDay();

				CreateCell(currentDate, column, row, date.Month != currentDate.Month && _selectedCell != null);
			}

			MyLogger.WriteInMethod("Before Fill Tasks Busyness");
			FillTasksBusyness();
			MyLogger.WriteInMethod("Before Fill Habits Busyness");
			FillHabitsBusyness();

			MyLogger.WriteEndMethod();


			#region Full month calendar
			// Making full month
			//MyLogger.WriteStartMethod();
			//// Clean the calendar
			//CalendarGrid.ColumnDefinitions.Clear();
			//CalendarGrid.Children.Clear();
			//MyLogger.WriteInMethod("After clearing the calendar grid.");

			//// Count rows
			//// get days in the month and roundUp((that + startday num) / 7) is the number of rows
			//int daysInMonth = DateTime.DaysInMonth(year, month);
			//var startDate = new DateTime(year, month, 1);
			//int startDay = startDate.ZeroMondayBasedDay();
			//int rows = 3;//(int)Math.Ceiling((daysInMonth + startDay) / 7.0);

			//// Create row definitions
			//MyLogger.WriteInMethod($"Before creating the { rows } rows");
			//for (int i = 0; i < rows; i++)
			//	CalendarGrid.RowDefinitions.Add(new RowDefinition{Height = GridLength.Star});

			//// TODO:Create previous month cells
			////
			//int prevMonthYear = startDate.Year - (month == 1 ? 1 : 0);
			//int prevMonth = (prevMonthYear < startDate.Year) ? 12 : month - 1;
			//int prevMonthDays = DateTime.DaysInMonth(prevMonthYear, prevMonth);

			//for (int i = 0; i < startDay; i++)
			//{
			//	var dateOfTheDay = new DateTime(prevMonthYear, prevMonth, prevMonthDays - i);
			//	var column = dateOfTheDay.ZeroMondayBasedDay();
			//	var row = 0;

			//	CreateCell(dateOfTheDay, column, row, true);
			//}

			//// Create current month cells
			//MyLogger.WriteInMethod("Before creating days in the current month");
			//DateTime date = startDate;
			//for (int i = 0; i < daysInMonth; i++)
			//{
			//	DateTime currentDate = date.Add(TimeSpan.FromDays(i));
			//	int row = (i + startDay) / 7;
			//	int column = currentDate.ZeroMondayBasedDay();

			//	CreateCell(currentDate, column, row, false);
			//}
			//MyLogger.WriteInMethod("After creating days in the current month");

			//// TODO: Create upcomming month cells
			//int nextMonthYear;
			//int nextMonth;

			//if (startDate.Month == 12)
			//{
			//	nextMonth = 1;
			//	nextMonthYear = startDate.Year + 1;
			//}
			//else
			//{
			//	nextMonth = startDate.Month + 1;
			//	nextMonthYear = year;
			//}

			//DateTime nextMonthFirstDay = new DateTime(nextMonthYear, nextMonth, 1);
			//for (int i = 0; i < 7 - nextMonthFirstDay.ZeroMondayBasedDay(); i++)
			//{
			//	DateTime nextMonthdate = nextMonthFirstDay.AddDays(i);
			//	CreateCell(nextMonthdate, nextMonthdate.ZeroMondayBasedDay(), rows - 1, true);
			//}

			//MyLogger.WriteEndMethod(); 
			#endregion
		}

		private void FillHabitsBusyness()
		{
			if(HabitsSource != null)
			foreach (var habit in HabitsSource)
			{
				IncreaseBusyness(habit);
			}
		}

		private void CreateCell(DateTime date, int column, int row, bool isSide)
		{
			var today = DateTime.Today;
			var cell = new CalendarCell(date)
			{
				TappedCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(c =>
				{
					SelectedDate = ((CalendarCell)c).Date;
				}),
				IsSideMonth = isSide,
				IsToday = date == today,
			};

			MyLogger.WriteInMethod("Before adding the cell to the children");
			CalendarGrid.Children.Add(cell, column, row);
			MyLogger.WriteInMethod("After adding the cell to the children");
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
						habit.PropertyChanged  += HabitOnPropertyChanged;
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (IHabitViewModel habit in notifyCollectionChangedEventArgs.OldItems)
					{
						DecreaseBusyness(habit);
						habit.PropertyChanging -= HabitOnPropertyChanging;
						habit.PropertyChanged  -= HabitOnPropertyChanged;
					}
					break;
			}
		}

		private void HabitOnPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if(e.PropertyName == nameof(IHabitViewModel.RepeatType) || e.PropertyName == nameof(IHabitViewModel.StartDate) || e.PropertyName == nameof(IHabitViewModel.DaysToRepeat))
				DecreaseBusyness((IHabitViewModel)sender);
		}

		private void HabitOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(IHabitViewModel.RepeatType) || e.PropertyName == nameof(IHabitViewModel.StartDate) || e.PropertyName == nameof(IHabitViewModel.DaysToRepeat))
				IncreaseBusyness((IHabitViewModel)sender);
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
				((INotifyCollectionChanged) TasksSource).CollectionChanged += OnTasksSourceCollectionChanged;

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
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (ITaskViewModel task in notifyCollectionChangedEventArgs.OldItems)
					{
						DecreaseBusyness(task.ScheduleDate);
						task.PropertyChanging -= TaskOnPropertyChanging;
						task.PropertyChanged -= TaskOnPropertyChanged;
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
			if(habit.StartDate == null) return;

			// Iterates all cells and increases their busyness by one if the habit is active on the cells date
			foreach (var child in CalendarGrid.Children)
			{
				if (child is CalendarCell cell && habit.IsActive(cell.Date))
					cell.Busyness++;
			}
		}
		private void IncreaseBusyness(DateTime? date)
		{
			var cell = FindCell(date);

			if (cell != null)
				cell.Busyness++;
		}

		private void FillTasksBusyness()
		{
			if(TasksSource != null)
			foreach (var taskViewModel in TasksSource)
			{
				IncreaseBusyness(taskViewModel.ScheduleDate);
			}
		}

	}
}