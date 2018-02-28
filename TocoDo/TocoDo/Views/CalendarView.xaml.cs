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
		#endregion

		#region Properties

		private CalendarCell _selectedCell;
		public DateTime SelectedDate
		{
			get => (DateTime) GetValue(SelectedDateProperty);
			set => SetValue(SelectedDateProperty, value);
		}

		public ICommand CellTappedCommand { get; set; } 
		#endregion

		public CalendarView()
		{
			MyLogger.WriteStartMethod();
			MyLogger.WriteInMethod("Before initialize component.");
			InitializeComponent();
			MyLogger.WriteInMethod("Before setup of calendar.");
			SetupCalendarGrid(DateTime.Now.Month + 2, DateTime.Now.Year);
			SelectedDate = DateTime.Today;
			MyLogger.WriteEndMethod();
		}

		#region Property change
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);

			if (propertyName == nameof(SelectedDate) && _selectedCell != null)
				_selectedCell.IsSelected = false;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			//if(propertyName == nameof(SelectedDate))
		}
		#endregion

		private void SetupCalendarGrid(int month, int year)
		{
			MyLogger.WriteStartMethod();
			// Clean the calendar
			CalendarGrid.ColumnDefinitions.Clear();
			CalendarGrid.Children.Clear();
			MyLogger.WriteInMethod("After clearing the calendar grid.");

			// Count rows
			// get days in the month and roundUp((that + startday num) / 7) is the number of rows
			int daysInMonth = DateTime.DaysInMonth(year, month);
			var startDate = new DateTime(year, month, 1);
			int startDay = startDate.ZeroMondayBasedDay();
			int rows = (int)Math.Ceiling((daysInMonth + startDay) / 7.0);
			
			// Create row definitions
			MyLogger.WriteInMethod($"Before creating the { rows } rows");
			for (int i = 0; i < rows; i++)
				CalendarGrid.RowDefinitions.Add(new RowDefinition{Height = GridLength.Star});

			// TODO:Create previous month cells
			// 
			int prevMonthYear = startDate.Year - (month == 1 ? 1 : 0);
			int prevMonth = (prevMonthYear < startDate.Year) ? 12 : month - 1;
			int prevMonthDays = DateTime.DaysInMonth(prevMonthYear, prevMonth);

			for (int i = 0; i < startDay; i++)
			{
				var dateOfTheDay = new DateTime(prevMonthYear, prevMonth, prevMonthDays - i);
				var column = dateOfTheDay.ZeroMondayBasedDay();
				var row = 0;

				CreateCell(dateOfTheDay, column, row, true);
			}

			// Create current month cells
			MyLogger.WriteInMethod("Before creating days in the current month");
			DateTime date = startDate;
			for (int i = 0; i < daysInMonth; i++)
			{
				DateTime currentDate = date.Add(TimeSpan.FromDays(i));
				int row = (i + startDay) / 7;
				int column = currentDate.ZeroMondayBasedDay();

				CreateCell(currentDate, column, row, false);
			}
			MyLogger.WriteInMethod("After creating days in the current month");

			// TODO: Create upcomming month cells
			int nextMonthYear;
			int nextMonth;

			if (startDate.Month == 12)
			{
				nextMonth = 1;
				nextMonthYear = startDate.Year + 1;
			}
			else
			{
				nextMonth = startDate.Month + 1;
				nextMonthYear = year;
			}

			DateTime nextMonthFirstDay = new DateTime(nextMonthYear, nextMonth, 1);
			for (int i = 0; i < 7 - nextMonthFirstDay.ZeroMondayBasedDay(); i++)
			{
				DateTime nextMonthdate = nextMonthFirstDay.AddDays(i);
				CreateCell(nextMonthdate, nextMonthdate.ZeroMondayBasedDay(), rows - 1, true);
			}

			MyLogger.WriteEndMethod();
		}

		private void CreateCell(DateTime date, int column, int row, bool isSide)
		{
			var cell = new CalendarCell
			           {
				           Date          = date,
				           TappedCommand = new TocoDo.BusinessLogic.Helpers.Commands.Command(c => ((CalendarCell)c).IsSelected = true),
						   IsSideMonth   = isSide
			           };

			MyLogger.WriteInMethod("Before adding the cell to the children");
			CalendarGrid.Children.Add(cell, column, row);
			MyLogger.WriteInMethod("After adding the cell to the children");
		}
	}
}