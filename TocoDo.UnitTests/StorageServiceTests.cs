using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.UnitTests
{
	[TestClass]
	public class StorageServiceTests
	{
		[TestClass]
		public class HabitTests
		{
			private IPersistance _persistance;
			private IStorageService _storage;

			[TestInitialize]
			public void InitializeTest()
			{
				_persistance = Mock.Create<IPersistance>();
				_storage = new StorageService(_persistance);
			}

			[TestMethod]
			public void StartCreatingHabitDoesNotInsertAnythingToTheDatabase()
			{
				// Arrange
				Mock.Arrange(() => _persistance.InsertAsync(new object())).OccursNever();

				// Act
				_storage.StartCreatingHabit();

				// Assert
				Mock.Assert(_persistance);
			}

			[TestMethod]
			public void StartCreatingHabitAddsHabitToTheStorageListWithAllHabits()
			{
				// Arrange
				var expectedResult = 1;

				// Act
				_storage.StartCreatingHabit();

				// Assert
				Assert.AreEqual(expectedResult, _storage.AllHabits.Count);
			}

			
		}



	}
}
