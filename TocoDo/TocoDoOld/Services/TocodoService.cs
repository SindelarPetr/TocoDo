using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TocoDo.Models;
using TocoDo.Resources;
using Xamarin.Forms.Internals;

namespace TocoDo.Services
{
	public static class TocodoService
	{

		private static List<ChallengeModel> _fakeChallenges = new List<ChallengeModel>
		{
			 new ChallengeModel
			{
				ChallengeType = ChallengeType.Daylong,
				DaysCount = 12,
			}
		};

		private static SQLiteAsyncConnection _connection;

		public static async void Init()
		{
			_connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.Current.LocalStorage.Path, AppStrings.DatabaseName));
			await _connection.CreateTableAsync<TaskModel>();
		}

		private static async Task<List<TaskModel>> FetchTasks()
		{
			//return _fakeTasks;
			throw new NotImplementedException();
		}

		private static async Task<List<ChallengeModel>> FetchChallenges()
		{
			throw new NotImplementedException();
		}

		private static async Task<List<TaskModel>> GetTasks()
		{
			return (await FetchTasks()).ToList();
		}



		public static async Task InsertTask(TaskModel task)
		{
			await _connection.InsertAsync(task);
		}

		public static async Task UpdateTask(TaskModel task)
		{
			await _connection.UpdateAsync(task);
		}

		public static async Task DeleteTask(TaskModel task)
		{
			await _connection.DeleteAsync(task);
		}

		public static async Task<List<ChallengeModel>> GetChallenges()
		{
			return (await FetchChallenges()).ToList();
		}

		public static async Task<List<TaskModel>> GetTodayTasks()
		{
			return (await GetTasks()).Where(t => t.Deadline != null && t.Deadline.Value.Date == DateTime.Today).ToList();
		}

		public static async Task<List<TaskModel>> GetSomedayTasks()
		{
			return (await GetTasks()).Where(t => t.Deadline == null).ToList();
		}

		public static async Task<Dictionary<DateTime, List<TaskModel>>> GetUpcommingTasks()
		{
			Dictionary<DateTime, List<TaskModel>> upcomming = new Dictionary<DateTime, List<TaskModel>>();

			(await GetTasks()).ForEach(t =>
			{
				if (t.Deadline != null && t.Deadline.Value.Date >= DateTime.Today + TimeSpan.FromDays(1))
				{
					if (upcomming.ContainsKey(t.Deadline.Value.Date))
					{
						upcomming[t.Deadline.Value.Date].Add(t);
					}
					else
					{
						upcomming[t.Deadline.Value.Date] = new List<TaskModel> { t };
					}
				}
			});

			return upcomming;
		}
	}
}
