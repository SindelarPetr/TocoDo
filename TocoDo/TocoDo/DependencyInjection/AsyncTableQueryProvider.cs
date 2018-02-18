using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;
using TocoDo.BusinessLogic.DependencyInjection;

namespace TocoDo.UI.DependencyInjection
{
	public class AsyncTableQueryProvider<T> : IAsyncTableQuery<T>
		where T : class, new()
	{
		private AsyncTableQuery<T> _wrappedQuery;

		public AsyncTableQueryProvider(AsyncTableQuery<T> queryToWrap) => _wrappedQuery = queryToWrap;

		public IAsyncTableQuery<T> Where(Expression<Func<T, bool>> predExpr) => new AsyncTableQueryProvider<T>(_wrappedQuery.Where(predExpr));
		public IAsyncTableQuery<T> Skip(int n) => new AsyncTableQueryProvider<T>(_wrappedQuery.Skip(n));
		public IAsyncTableQuery<T> Take(int n) => new AsyncTableQueryProvider<T>(_wrappedQuery.Take(n));


		public IAsyncTableQuery<T> OrderBy<TValue>(Expression<Func<T, TValue>> orderExpr) => new AsyncTableQueryProvider<T>(_wrappedQuery.OrderBy(orderExpr));
		public IAsyncTableQuery<T> OrderByDescending<TValue>(Expression<Func<T, TValue>> orderExpr) => new AsyncTableQueryProvider<T>(_wrappedQuery.OrderByDescending(orderExpr));
		public IAsyncTableQuery<T> ThenBy<TValue>(Expression<Func<T, TValue>> orderExpr) => new AsyncTableQueryProvider<T>(_wrappedQuery.ThenBy(orderExpr));
		public IAsyncTableQuery<T> ThenByDescending<TValue>(Expression<Func<T, TValue>> orderExpr) => new AsyncTableQueryProvider<T>(_wrappedQuery.ThenByDescending(orderExpr));
		public async Task<List<T>> ToListAsync() => await _wrappedQuery.ToListAsync();
		public async Task<int> CountAsync() => await _wrappedQuery.CountAsync();
		public async Task<T> ElementAtAsync(int index) => await _wrappedQuery.ElementAtAsync(index);
		public async Task<T> FirstAsync() => await _wrappedQuery.FirstAsync();
		public async Task<T> FirstOrDefaultAsync() => await _wrappedQuery.FirstOrDefaultAsync();
	}
}
