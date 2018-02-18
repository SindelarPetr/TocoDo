using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using Xamarin.Forms;

namespace TocoDo.BusinessLogic.DependencyInjection
{
	public interface IPersistance
	{
		Task Init();
		Task<List<ITaskModel>> GetTasks();
		Task<List<IHabitModel>> GetHabits();

		Task InsertAsync(object item);
		Task UpdateAsync(object obj);
		Task DeleteAsync(object obj);
	}
}