using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.UI.Models;
using TocoDo.UI.Properties;
using Xamarin.Forms;

namespace TocoDo.UI.DependencyInjection
{
	public class PersistanceProvider : IPersistance
	{
		private SQLiteAsyncConnection _connection;

		public async Task Init()
		{
			MyLogger.WriteStartMethod();
			MyLogger.WriteInMethod("Creating Database");
			var path    = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), AppStrings.DatabaseName);
			_connection = new SQLiteAsyncConnection(path);

			MyLogger.WriteInMethod("Creating tables");
			//await _connection.ExecuteAsync("DELETE FROM HabitModel");
			await _connection.CreateTablesAsync(CreateFlags.None, typeof(TaskModel), typeof(HabitModel));
			//for(int i = 0; i < 10; i++)
			//await _connection.InsertAsync(new HabitModel
			//                              {
			//	                              CreationDate = DateTime.Today,
			//	                              DaysToRepeat = 20,
			//	                              IsFinished   = true,
			//	                              StartDate    = DateTime.Today.AddDays(-50),
			//								  RepeatType = RepeatType.Days
			//                              });
			MyLogger.WriteEndMethod();
		}

		public async Task<List<ITaskModel>> GetTasks() => (await _connection.QueryAsync<TaskModel>(
			"SELECT * FROM TaskModel WHERE ScheduleDate IS NULL OR ScheduleDate >= '" + DateTime.Today.Ticks + "'")).OfType<ITaskModel>().ToList();

		public async Task<List<ITaskModel>> GetPastTasks() => (await _connection.QueryAsync<TaskModel>(
			$"SELECT * FROM TaskModel WHERE (ScheduleDate IS NOT NULL AND ScheduleDate < \'{DateTime.Today.Ticks}\') OR (Done IS NOT NULL AND Done < \'{DateTime.Today.Ticks}\')")).OfType<ITaskModel>().ToList();

		public async Task<List<IHabitModel>> GetHabits() => (await _connection.Table<HabitModel>().Where(h => !h.IsFinished).ToListAsync()).OfType<IHabitModel>().ToList();

		public async Task<List<IHabitModel>> GetPastHabits() =>
			(await _connection.Table<HabitModel>().Where(h => h.IsFinished).Take(10).ToListAsync())
			.OfType<IHabitModel>().ToList();

		public async Task InsertAsync(object item) => await _connection.InsertAsync(item);
		public async Task UpdateAsync(object item) => await _connection.UpdateAsync(item);
		public async Task DeleteAsync(object obj) => await _connection.DeleteAsync(obj);
	}
}