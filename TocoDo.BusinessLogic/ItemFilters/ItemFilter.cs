using System;

namespace TocoDo.BusinessLogic.ItemFilters
{
    public class ItemFilter<T>
    {
	    private readonly Func<T, bool> _filterFunction;

	    public ItemFilter(Func<T, bool> filterFunction)
	    {
		    _filterFunction = filterFunction;
	    }

	    public bool Filter(T t)
	    {
		    return _filterFunction(t);
	    }
    }
}
