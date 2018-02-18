﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Services
{
    public class HabitService : IHabitService
	{
	    private readonly ObservableCollection<IHabitViewModel> _habits;
		public ReadOnlyObservableCollection<IHabitViewModel> AllHabits { get; }

		private IDateTimeProvider _dateTimeProvider;
		private HabitScheduleHelper _scheduleHelper;
		public IDateTimeProvider DateTimeProvider
		{
			get => _dateTimeProvider;
			set
			{
				_dateTimeProvider = value;
				_scheduleHelper   = new HabitScheduleHelper(_dateTimeProvider);
			}
		}
		public IModelFactory ModelFactory { get; set; }
		public INavigationService Navigation  { get; set; }
		public IPersistance       Persistance { get; set; }

	    public HabitService()
	    {
			_habits      = new ObservableCollection<IHabitViewModel>();
		    AllHabits = new ReadOnlyObservableCollection<IHabitViewModel>(_habits);
		}

	    public async Task LoadAsync()
	    {
		    var models    = await Persistance.GetHabits();
		    var habitList = models.Select(m => new HabitViewModel(this, Navigation, m));

		    var filtred                 = new List<HabitViewModel>();
		    var yesterdayFinishedHabits = new List<HabitViewModel>();
		    foreach (var habit in habitList)
		    {
			    if (_scheduleHelper.IsHabitFinished(habit))
			    {
				    yesterdayFinishedHabits.Add(habit);
				    continue;
			    }

			    filtred.Add(habit);
		    }

		    filtred.ForEach(_habits.Add);
		}

	    public void StartCreation()
	    {
		    _habits.Add(new HabitViewModel(this, Navigation));
	    }

	    public async Task ConfirmCreationAsync(IHabitViewModel habit)
	    {
		    await Persistance.InsertAsync(ModelFactory.CreateHabitModel(habit));
	    }

	    public void CancelCreation(IHabitViewModel habit)
	    {
		    _habits.Remove(habit);
	    }

	    public async Task UpdateAsync(IHabitViewModel habit)
	    {
		    await Persistance.UpdateAsync(ModelFactory.CreateHabitModel(habit));
	    }

	    public async Task DeleteAsync(IHabitViewModel habit)
	    {
		    _habits.Remove(habit);
		    await Persistance.DeleteAsync(ModelFactory.CreateHabitModel(habit));
	    }
	}
}
