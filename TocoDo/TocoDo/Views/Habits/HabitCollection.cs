using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.ViewModels;
using TocoDo.Views.Todos;

namespace TocoDo.Views.Habits
{
    public class HabitCollection : ItemCollection<HabitViewModel, HabitView>
    {
	    public HabitCollection() : base(vm => new HabitView(vm)) { }
    }
}
