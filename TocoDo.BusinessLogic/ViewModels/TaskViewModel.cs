﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Properties;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class TaskViewModel : BaseViewModel, ITaskViewModel
	{
		#region Services

		private readonly INavigationService _navigation;
		private readonly ITaskService _taskService;

		#endregion

		#region Backing fields

		private DateTime? _deadline;
		private string _description;
		private string _title;
		private DateTime? _reminder;
		private DateTime? _done;
		private DateTime? _scheduleDate;

		#endregion

		#region Properties

		public int Id { get; set; }

		public DateTime? Done
		{
			get => _done;
			set
			{
				// Change the done time only if the property has no value...
				if (_done == null || value == null)
					SetValue(ref _done, value);
			}
		}

		public DateTime? Deadline
		{
			get => _deadline;
			set => SetValue(ref _deadline, value);
		}

		public DateTime? ScheduleDate
		{
			get => _scheduleDate;
			set => SetValue(ref _scheduleDate, value);
		}

		public DateTime CreateTime { get; }

		public string Description
		{
			get => _description;
			set => SetValue(ref _description, value);
		}

		public string Title
		{
			get => _title;
			set => SetValue(ref _title, value);
		}

		public DateTime? Reminder
		{
			get => _reminder;
			set => SetValue(ref _reminder, value);
		}

		public bool IsCreateMode { get; set; }

		#endregion

		#region Commands

		public ICommand FinishCreationCommand { get; }
		public ICommand EditCommand           { get; }
		public ICommand EditTitleCommand      { get; }
		public ICommand RemoveCommand         { get; }
		public ICommand UpdateCommand         { get; }

		#endregion

		public TaskViewModel(ITaskService taskService, INavigationService navigation) : this()
		{
			_navigation  = navigation;
			_taskService = taskService;

			IsCreateMode = true;
		}

		public TaskViewModel(ITaskService taskService, INavigationService navigation, ITaskModel taskModel) : this()
		{
			_taskService = taskService;
			_navigation  = navigation;

			#region Copy taskModel properties

			Id            = taskModel.Id;
			_done         = taskModel.Done;
			_deadline     = taskModel.Deadline;
			_title        = taskModel.Title;
			CreateTime    = taskModel.CreateTime;
			_description  = taskModel.Description;
			_reminder     = taskModel.Reminder;
			_scheduleDate = taskModel.ScheduleDate;

			#endregion
		}

		private TaskViewModel()
		{
			FinishCreationCommand = new Command(ConfirmCreation);
			EditTitleCommand      = new Command(EditTitle);
			EditCommand           = new Command(async () => await Edit());
			RemoveCommand         = new Command(async () => await RemoveTask());
			UpdateCommand         = new Command(async () => await Update());
		}

		#region Private methods

		private void EditTitle(object o)
		{
			if (o is string title && !string.IsNullOrWhiteSpace(title))
			{
				Title = title.Trim();
			}
		}

		private void ConfirmCreation(object o)
		{
			if (o == null || o is string && string.IsNullOrWhiteSpace((string) o))
			{
				_taskService.CancelCreation(this);
				return;
			}

			if (o is string title)
			{
				Title = title.Trim();
				_taskService.ConfirmCreationAsync(this);
			}
		}

		private async Task Edit()
		{
			await _navigation.PushAsync(PageType.ModifyTaskPage, this);
		}

		protected override async void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (!IsCreateMode)
			{
				await Update();
				MyLogger.WriteInMethod($"Saved task, changed property: {propertyName}");
			}
		}

		private async Task RemoveTask()
		{
			var result = await _navigation.DisplayAlert(Resources.DeleteToDo, Resources.ConfirmDelete, Resources.Yes,
			                                            Resources.Cancel);

			if (!result) return;

			await _taskService.DeleteAsync(this);

			await _navigation.PopAsync();
		}

		private async Task Update()
		{
			await _taskService.UpdateAsync(this);
		}

		private async Task SelectDate(Action<DateTime?> pickedAction, string actionSheetHeader)
		{
			string[] buttons = {Resources.Today, Resources.Tomorrow, Resources.TheDayAfterTomorrow, Resources.PickADate};

			var result = await _navigation.DisplayActionSheet(actionSheetHeader, Resources.Cancel, null, buttons);


			DateTime selectedDate;
			if (result == Resources.Today)
			{
				selectedDate = DateTime.Today;
			}
			else if (result == Resources.Tomorrow)
			{
				selectedDate = DateTime.Today + TimeSpan.FromDays(1);
			}
			else if (result == Resources.PickADate)
			{
				//SelectDateByPicker(d => pickedAction(d));
				return;
			}
			else
			{
				return;
			}

			pickedAction(selectedDate);
		}

		#endregion
	}
}