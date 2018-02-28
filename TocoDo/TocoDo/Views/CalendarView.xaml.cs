using System;
using System.Windows.Input;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Helpers.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarView : ContentView
	{
		#region Backing fields
		public static BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime), typeof(DateTime), DateTime.Now);

		private static readonly BindablePropertyKey HighlightedMonthPropertyKey = BindableProperty.CreateReadOnly(nameof(HighlightedMonth), typeof(int), typeof(int), -1);
		public static BindableProperty HighlightedMonthProperty = HighlightedMonthPropertyKey.BindableProperty;

		private CalendarCell _selectedCell;
		private ICommand _moveNextCommand;
		private ICommand _movePrevCommand;
		private DateTime _firstDayDate;

		#endregion

		#region Properties

		public int HighlightedMonth
		{
			get => (int) GetValue(HighlightedMonthProperty);
			set => SetValue(HighlightedMonthPropertyKey, value);
		}

		public DateTime SelectedDate
		{
			get => (DateTime) GetValue(SelectedDateProperty);
			set => SetValue(SelectedDateProperty, value.Date);
		}


		public ICommand MoveNextCommand => _moveNextCommand ?? (_moveNextCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(() => SetupCalendarGrid(_firstDayDate.AddDays(3 * 7))));
		public ICommand MovePrevCommand => _movePrevCommand ?? (_movePrevCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(() => SetupCalendarGrid(_firstDayDate.AddDays(3 * 7))));

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

			if (propertyName == nameof(SelectedDate) && _selectedCell != null)
			{
				_selectedCell.IsSelected = false;
				_selectedCell = null;
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == nameof(SelectedDate))
			{
				// Unselect previously selected cell
				if (_selectedCell != null)
				{
					_selectedCell.IsSelected = false;
					_selectedCell            = null;
				}

				// Find the cell with the SelectedDate, select it and save it to _selectedDate
				// The cell can be in just in the column which corresonds to its day in week
				int column = SelectedDate.ZeroMondayBasedDay();

				// Iterate the rows in the column and find the cell with the right date
				int childrenCount = CalendarGrid.Children.Count;
				for (int i = column; i < childrenCount; i += 7)
				{
					if (CalendarGrid.Children[i] is CalendarCell cell && cell.Date == SelectedDate)
					{
						// Found the right cell -> lets select it
						cell.IsSelected = true;
						_selectedCell   = cell;

						// if the newly selected cell is in different month than which is highlighted, then highlight the new month
						HighlightedMonth = SelectedDate.Month;
						return;
					}
				}
			} else if (propertyName == nameof(HighlightedMonth))
			{
				foreach (var child in CalendarGrid.Children)
				{
					if(!(child is CalendarCell cell))
						continue;

					cell.IsSideMonth = cell.Date.Month != HighlightedMonth;
				}
			}

		}
		#endregion

		private void SetupCalendarGrid(DateTime date)
		{
			// Create 3 week calendar
			MyLogger.WriteStartMethod();

			// Clean the calendar
			CalendarGrid.ColumnDefinitions.Clear();
			CalendarGrid.Children.Clear();
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

				CreateCell(currentDate, column, row, date.Month != currentDate.Month);
			}


			MyLogger.WriteEndMethod();


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
		}

		private void CreateCell(DateTime date, int column, int row, bool isSide)
		{
			var today = DateTime.Today;
			var cell = new CalendarCell
			           {
				           Date          = date,
				           TappedCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(c =>
				           {
					           SelectedDate = ((CalendarCell) c).Date;
				           }),
						   IsSideMonth   = isSide,
						   IsToday = date == today,
			           };

			MyLogger.WriteInMethod("Before adding the cell to the children");
			CalendarGrid.Children.Add(cell, column, row);
			MyLogger.WriteInMethod("After adding the cell to the children");
		}
	}
}