using System.Threading.Tasks;

namespace TocoDo.BusinessLogic.DependencyInjection
{
    public interface IPersistance
    {
	    Task InitAsync();
	    Task InsertAsync<T>(T obj);
	    Task UpdateAsync<T>(T obj);
	    Task DeleteAsync<T>(T obj);
    }
}
