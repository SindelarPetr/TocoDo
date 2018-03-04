using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.UnitTests.Services
{
	[TestClass]
	public class HabitScheduleHelperTests
	{
		private IDateTimeProvider _dateProvider;
		private IHabitViewModel _habit;
		private HabitScheduleHelper _scheduleHelper;

		[TestInitialize]
		public void TestInit()
		{
			_dateProvider   = Mock.Create<IDateTimeProvider>();
			_scheduleHelper = new HabitScheduleHelper(_dateProvider);
			_habit          = Mock.Create<IHabitViewModel>();
		}

		private void ArrangeNowDateToDateProvider(DateTime date)
		{
			Mock.Arrange(() => _dateProvider.Now).Returns(date);
		}

		private void ArrangeHabit(DateTime? startDate, RepeatType repeatType, int daysToRepeat)
		{
			Mock.Arrange(() => _habit.StartDate).Returns(startDate);
			Mock.Arrange(() => _habit.RepeatType).Returns(repeatType);
			Mock.Arrange(() => _habit.DaysToRepeat).Returns(daysToRepeat);
		}

		#region HabitLength
		[TestMethod]
		public void IsHabitActiveReturnsTrueWhenTheRepeatTypeIsDaysAndAskedDateIsTheSameAsStartDate()
		{
			// Arrange
			var date = new DateTime(2010, 10, 10);
			ArrangeNowDateToDateProvider(date);
			var daysToRepeat = 10;
			var repeatType = RepeatType.Days;
			ArrangeHabit(date, repeatType, daysToRepeat);

			// Act
			var result = _scheduleHelper.IsHabitActive(_habit, date);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void HabitLengthThrowsArgumentExceptionIfTheGivenHabitHasNullStartDate()
		{
			// Arrange
			ArrangeHabit(null, RepeatType.Days, 10);


			// Act
			void Action()
			{
				_scheduleHelper.HabitLength(_habit);
			}

			// Assert
			Assert.ThrowsException<ArgumentException>(() => Action());
		}

		[TestMethod]
		public void HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsDays()
		{
			// Arrange
			var startDate = new DateTime(2010, 10, 2);
			var repeatType = RepeatType.Days;
			var daysToRepeat = 1000;
			var expectedResult = TimeSpan.FromDays(daysToRepeat);
			ArrangeHabit(startDate, repeatType, daysToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		//[TestMethod]
		//public void HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsMonths()
		//{
		//	// Arrange
		//	var startDate = new DateTime(2010, 10, 2);
		//	var repeatType = RepeatType.Months;
		//	var monthsToRepeat = 200;
		//	var expectedResult = startDate.AddMonths(monthsToRepeat) - startDate;
		//	ArrangeHabit(startDate, repeatType, monthsToRepeat);

		//	// Act
		//	var actualResult = _scheduleHelper.HabitLength(_habit);

		//	// Assert
		//	Assert.AreEqual(expectedResult, actualResult);
		//}

		[TestMethod]
		public void HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsYears()
		{
			// Arrange
			var startDate = new DateTime(2010, 10, 2);
			var repeatType = RepeatType.Years;
			var yearsToRepeat = 200;
			var expectedResult = startDate.AddYears(yearsToRepeat) - startDate;
			ArrangeHabit(startDate, repeatType, yearsToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsTueSatThereIsJustOneWeekToRepeat()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 27); // Its monday
			var repeatType = RepeatType.Tue | RepeatType.Sat;
			var weeksToRepeat = 1;
			var expectedResult = TimeSpan.FromDays(5);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsMonSunThereIsJustOneWeekToRepeat()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 26); // Its monday
			var repeatType = RepeatType.Mon | RepeatType.Sun;
			var weeksToRepeat = 1;
			var expectedResult = TimeSpan.FromDays(7);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsThuAndThereIsJustOneWeekToRepeat()
		{
			// Arrange
			var startDate = new DateTime(2018, 3, 1); // Its monday
			var repeatType = RepeatType.Thu;
			var weeksToRepeat = 1;
			var expectedResult = TimeSpan.FromDays(1);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void
			HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsTueThuSatThereIsJustOneWeekToRepeatAndTheStartDateIsOnThu()
		{
			// Arrange
			var startDate = new DateTime(2018, 3, 1); // Its Thursday
			var repeatType = RepeatType.Tue | RepeatType.Thu | RepeatType.Sat;
			var weeksToRepeat = 1;
			var expectedResult = TimeSpan.FromDays(3);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void
			HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsTueThuSatThereAreTwoWeeksToRepeatAndTheStartDateIsOnThu()
		{
			// Arrange
			var startDate = new DateTime(2018, 3, 1); // Its Thursday
			var repeatType = RepeatType.Tue | RepeatType.Thu | RepeatType.Sat;
			var weeksToRepeat = 2;
			var expectedResult = TimeSpan.FromDays(10);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void
			HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsMonWedThuSatThereAreALotOfWeeksToRepeatAndTheStartDateIsOnThu()
		{
			// Arrange
			var startDate = new DateTime(2018, 3, 1); // Its Thursday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Thu | RepeatType.Sat;
			var weeksToRepeat = 10;
			var expectedResult = TimeSpan.FromDays(66);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void
			HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsMonTueWedThuFriSatSunThereAreALotOfWeeksToRepeatAndTheStartDateIsOnFri()
		{
			// Arrange
			var startDate = new DateTime(2018, 3, 2); // Its Friday
			var repeatType = RepeatType.Mon | RepeatType.Tue | RepeatType.Wed | RepeatType.Thu | RepeatType.Fri |
							 RepeatType.Sat | RepeatType.Sun;
			var weeksToRepeat = 100;
			var expectedResult = TimeSpan.FromDays(3 + (weeksToRepeat - 1) * 7);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.HabitLength(_habit);

			// Assert
			Assert.AreEqual(expectedResult, actualResult);
		} 
		#endregion

		#region IsHabitFinished
		[TestMethod]
		public void IsHabitFinishedReturnsTrueIfAHabitWithRepeatTypeDaysEndsBeforeCurrentDate()
		{
			// Arrange
			var startDate = new DateTime(2018, 3, 2); // Its Friday
			var repeatType = RepeatType.Days;
			var dyasToRepeat = 10;
			var currentDate = new DateTime(2018, 3, 12);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitFinished(_habit, currentDate);

			// Assert
			Assert.IsTrue(actualResult);
		}
		[TestMethod]
		public void IsHabitFinishedReturnsFalseIfAHabitWithRepeatTypeDaysEndsOnTheCurrentDate()
		{
			// Arrange
			var startDate = new DateTime(2018, 3, 2); // Its Friday
			var repeatType = RepeatType.Days;
			var dyasToRepeat = 10;
			var currentDate = new DateTime(2018, 3, 11);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitFinished(_habit, currentDate);

			// Assert
			Assert.IsFalse(actualResult);
		}
		[TestMethod]
		public void IsHabitFinishedReturnsFalseIfAHabitWithRepeatTypeDaysStartsAfterTheCurrentDate()
		{
			// Arrange
			var startDate = new DateTime(2018, 5, 2); // Its Friday
			var repeatType = RepeatType.Days;
			var dyasToRepeat = 10;
			var currentDate = new DateTime(2018, 3, 11);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitFinished(_habit, currentDate);

			// Assert
			Assert.IsFalse(actualResult);
		}
		[TestMethod]
		public void IsHabitFinishedReturnsTrueIfAHabitWithRepeatTypeWeeksTueThuEndsRightBeforeTheCurrentDate()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 28); // Its Wednesday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var dyasToRepeat = 1;
			var currentDate = new DateTime(2018, 3, 3);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitFinished(_habit, currentDate);

			// Assert
			Assert.IsTrue(actualResult);
		}
		[TestMethod]
		public void IsHabitFinishedReturnsFalseIfAHabitWithRepeatTypeWeeksTueThuEndsOnTheCurrentDate()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 28); // Its Wednesday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var dyasToRepeat = 1;
			var currentDate = new DateTime(2018, 3, 2);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitFinished(_habit, currentDate);

			// Assert
			Assert.IsFalse(actualResult);
		}
		[TestMethod]
		public void IsHabitFinishedReturnsTrueIfAHabitWithRepeatTypeWeeksTueThuEndsRightBeforeTheCurrentDateAndThereAreALotOfWeeksToRepeat()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 28); // Its Wednesday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var dyasToRepeat = 10;
			var currentDate = new DateTime(2018, 5, 5);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitFinished(_habit, currentDate);

			// Assert
			Assert.IsTrue(actualResult);
		}
		[TestMethod]
		public void IsHabitFinishedReturnsFalseIfAHabitWithRepeatTypeWeeksMonWedFriEndsOnTheCurrentDateAndThereAreALotOfWeeksToRepeat()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 28); // Its Wednesday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var dyasToRepeat = 10;
			var currentDate = new DateTime(2018, 5, 4);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitFinished(_habit, currentDate);

			// Assert
			Assert.IsFalse(actualResult);
		}
		#endregion

		// IsHabitActive
		#region IsHabitActive
		[TestMethod]
		public void IsHabitActiveReturnsTrueIfAHabitWithRepeatTypeWeeksWedHasOneWeekToRepeatAndCurrentDateIsAlsoAtThatDay()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 28); // Its Wednesday
			var repeatType = RepeatType.Wed;
			var dyasToRepeat = 1;
			var currentDate = new DateTime(2018, 2, 28);
			ArrangeHabit(startDate, repeatType, dyasToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitActive(_habit, currentDate);

			// Assert
			Assert.IsTrue(actualResult);
		}
		[TestMethod]
		public void IsHabitActiveReturnsTrueIfAHabitWithRepeatTypeMonWedFriHasOneWeekToRepeatAndCurrentDateIsOnWednesdayInTheOneWeek()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 26); // Its Wednesday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var weeksToRepeat = 1;
			var currentDate = new DateTime(2018, 2, 28);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitActive(_habit, currentDate);

			// Assert
			Assert.IsTrue(actualResult);
		}
		[TestMethod]
		public void IsHabitActiveReturnsFalseIfAHabitWithRepeatTypeMonWedFriHasOneWeekToRepeatAndCurrentDateIsOnThursdayInTheOneWeek()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 26); // Its Monday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var weeksToRepeat = 1;
			var currentDate = new DateTime(2018, 2, 27);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitActive(_habit, currentDate);

			// Assert
			Assert.IsFalse(actualResult);
		}
		[TestMethod]
		public void IsHabitActiveReturnsTrueIfAHabitWithRepeatTypeMonWedFriHasThreeWeeksToRepeatAndCurrentDateIsOnWednesdayInTheLastWeek()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 26); // Its Monday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var weeksToRepeat = 3;
			var currentDate = new DateTime(2018, 3, 14);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitActive(_habit, currentDate);

			// Assert
			Assert.IsTrue(actualResult);
		}
		[TestMethod]
		public void IsHabitActiveReturnsFalseIfAHabitWithRepeatTypeMonWedFriHasThreeWeeksToRepeatAndCurrentDateIsOnThursdayInTheLastWeek()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 26); // Its Monday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var weeksToRepeat = 3;
			var currentDate = new DateTime(2018, 3, 15);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitActive(_habit, currentDate);

			// Assert
			Assert.IsFalse(actualResult);
		}
		[TestMethod]
		public void IsHabitActiveReturnsTrueIfAHabitWithRepeatTypeMonWedFriHasThreeWeeksToRepeatAndCurrentDateIsOnFridayInTheLastWeek()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 26); // Its Monday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var weeksToRepeat = 3;
			var currentDate = new DateTime(2018, 3, 16);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitActive(_habit, currentDate);

			// Assert
			Assert.IsTrue(actualResult);
		}
		[TestMethod]
		public void IsHabitActiveReturnsFalseIfAHabitWithRepeatTypeMonWedFriHasThreeWeeksToRepeatAndCurrentDateIsOnSaturdayInTheLastWeek()
		{
			// Arrange
			var startDate = new DateTime(2018, 2, 26); // Its Monday
			var repeatType = RepeatType.Mon | RepeatType.Wed | RepeatType.Fri;
			var weeksToRepeat = 3;
			var currentDate = new DateTime(2018, 3, 17);
			ArrangeHabit(startDate, repeatType, weeksToRepeat);

			// Act
			var actualResult = _scheduleHelper.IsHabitActive(_habit, currentDate);

			// Assert
			Assert.IsFalse(actualResult);
		}
		#endregion



		//[TestMethod]
		//public void HabitLengthReturnsTheRightAmountOfDaysWhenRepeatTypeIsThuThereAreTwoWeeksToRepeat()
		//{
		//	// Arrange
		//	var startDate = new DateTime(2018, 2, 27); // Its monday
		//	var repeatType = RepeatType.Thu;
		//	var weeksToRepeat = 2;
		//	var expectedResult = TimeSpan.FromDays(2);
		//	ArrangeHabit(startDate, repeatType, weeksToRepeat);

		//	// Act
		//	var actualResult = _scheduleHelper.HabitLength(_habit);

		//	// Assert
		//	Assert.AreEqual(expectedResult, actualResult);
		//}
	}
}