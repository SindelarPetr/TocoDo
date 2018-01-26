using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.ViewModels;
using TocoDo.Views.Todos;

namespace TocoDo.Views.Habits
{
    public class ActualHabitCollection : ItemCollection<HabitViewModel, ActualHabitView>
    {
	    public ActualHabitCollection() : base(vm => new ActualHabitView(vm)) {}
    }
}
