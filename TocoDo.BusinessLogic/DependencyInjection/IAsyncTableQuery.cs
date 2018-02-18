using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TocoDo.BusinessLogic.DependencyInjection
{
	public interface IAsyncTableQuery<T>
	{
		IAsyncTableQuery<T> Where(Expression<Func<T, bool>>                       predExpr);
		IAsyncTableQuery<T> Skip(int                                              n);
		IAsyncTableQuery<T> Take(int                                              n);
		IAsyncTableQuery<T> OrderBy<TValue>(Expression<Func<T, TValue>>           orderExpr);
		IAsyncTableQuery<T> OrderByDescending<TValue>(Expression<Func<T, TValue>> orderExpr);
		IAsyncTableQuery<T> ThenBy<TValue>(Expression<Func<T, TValue>>            orderExpr);
		IAsyncTableQuery<T> ThenByDescending<TValue>(Expression<Func<T, TValue>>  orderExpr);

		Task<List<T>> ToListAsync();
		Task<int> CountAsync();
		Task<T> ElementAtAsync(int index);
		Task<T> FirstAsync();
		Task<T> FirstOrDefaultAsync();
	}
}