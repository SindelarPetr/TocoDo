using System;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using TocoDo.BusinessLogic.DependencyInjection;

namespace TocoDo.UI.DependencyInjection
{
    public class PersistanceProvider : IPersistance
    {
	    private SQLiteAsyncConnection _connection;

	    public async Task InitAsync()
	    {
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MySQLite.db3");
		    _connection = new SQLiteAsyncConnection(path);
		}

		public async Task InsertAsync<T>(T obj)
		{
			await _connection.InsertAsync(obj);
		}

	    public async Task UpdateAsync<T>(T obj)
	    {
		    await _connection.UpdateAsync(obj);
	    }

	    public async Task DeleteAsync<T>(T obj)
	    {
		    await _connection.DeleteAsync(obj);
	    }
    }
}
