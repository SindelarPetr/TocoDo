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
		Task<List<ITaskModel>> GetPastTasks();
		Task<List<IHabitModel>> GetHabits();
		Task<List<IHabitModel>> GetPastHabits();

		Task InsertAsync(object item);
		Task UpdateAsync(object obj);
		Task DeleteAsync(object obj);
	}
}