using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Todos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScheduledTasksView : TodoSetView
	{
		private readonly List<KeyValuePair<DateTime, ObservableCollection<TaskViewModel>>> _items =
			new List<KeyValuePair<DateTime, ObservableCollection<TaskViewModel>>>();

		private readonly Dictionary<ObservableCollection<TaskViewModel>, TodoSetView> _setsForItems =
			new Dictionary<ObservableCollection<TaskViewModel>, TodoSetView>();

		public ScheduledTasksView()
		{
			MyLogger.WriteStartMethod();
			MainLayout.Spacing = 5;
			MyLogger.WriteEndMethod();
		}

		protected override void AddTaskModel(TaskViewModel taskVm)
		{
			MyLogger.WriteStartMethod();
			if (taskVm.ScheduleDate == null)
			{
				MyLogger.WriteInMethod("Tried to add a task model with no schedule date to the collection of scheduled tasks");
				return;
			}

			DateTime scheduleDate = taskVm.ScheduleDate.Value.Date;

			var collection = FindCollection(scheduleDate);
			if (collection != null)
			{
				collection.Add(taskVm);
				return;
			}

			collection = new ObservableCollection<TaskViewModel>();
			var set    = new TodoSetView
			             {
				             TasksSource     = collection,
				             IsHeaderVisible = true,
				             HeaderText      = scheduleDate.ToString(
					             (scheduleDate < DateTime.Today + TimeSpan.FromDays(7) ? "ddd, " : "") +
					             "d'" + scheduleDate.GetDayExtension() + "' MMMM" +
					             ((DateTime.Today.Year != scheduleDate.Year) ? " YYYY" : "")),
				             HeaderBackgroundColor = Color.Transparent,
				             HeaderTextSize        = 13,
				             Padding               = new Thickness(0, 0, 0, 0)
			             };

			int index = InsertCollection(scheduleDate, collection);
			_setsForItems.Add(collection, set);

			collection.Add(taskVm);
			MainLayout.Children.Insert(index, set);
			MyLogger.WriteEndMethod();
		}

		private ObservableCollection<TaskViewModel> FindCollection(DateTime date)
		{
			MyLogger.WriteStartMethod();
			foreach (var keyValuePair in _items)
			{
				if (keyValuePair.Key == date)
					return keyValuePair.Value;

				if (keyValuePair.Key > date)
					return null;
			}

			MyLogger.WriteEndMethod();
			return null;
		}

		/// <summary>
		/// Inserts the given pair to the right order in the _items List.
		/// </summary>
		/// <param name="date">Key in the pair</param>
		/// <param name="collection">Value in the pair</param>
		/// <returns>The index on which the pair has been inserted</returns>
		private int InsertCollection(DateTime date, ObservableCollection<TaskViewModel> collection)
		{
			MyLogger.WriteStartMethod();
			for (var i = 0; i < _items.Count; i++)
			{
				var keyValuePair = _items[i];
				if (keyValuePair.Key > date)
				{
					_items.Insert(i, new KeyValuePair<DateTime, ObservableCollection<TaskViewModel>>(date, collection));
					return i;
				}
			}

			_items.Add(new KeyValuePair<DateTime, ObservableCollection<TaskViewModel>>(date, collection));
			return _items.Count - 1;
			MyLogger.WriteEndMethod();
		}

		protected override void RemoveTaskModel(TaskViewModel taskModel)
		{
			MyLogger.WriteStartMethod();
			if (taskModel.ScheduleDate == null)
			{
				MyLogger.WriteInMethod("Tried to remove a task model with no schedule date from the collection of scheduled tasks");
				return;
			}

			DateTime scheduleDate = taskModel.ScheduleDate.Value.Date;

			var collection = FindCollection(scheduleDate);

			if (collection == null)
			{
				return;
			}

			collection.Remove(taskModel);

			if (collection.Count == 0)
			{
				RemoveCollection(scheduleDate, collection);
			}
			MyLogger.WriteEndMethod();
		}

		private void RemoveCollection(DateTime scheduleDate, ObservableCollection<TaskViewModel> collection)
		{
			var set = _setsForItems[collection];
			_items.RemoveAll(p => p.Key == scheduleDate);
			_setsForItems.Remove(collection);
			MainLayout.Children.Remove(set);
		}
	}
}
