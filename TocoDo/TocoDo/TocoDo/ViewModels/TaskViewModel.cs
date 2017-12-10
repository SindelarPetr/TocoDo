﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.Models;
using TocoDo.Pages;
using TocoDo.Pages.Main;
using TocoDo.Resources;
using TocoDo.Services;
using Xamarin.Forms;

namespace TocoDo.ViewModels
{
	public class TaskViewModel : BaseViewModel
	{
		private readonly bool _updateOnPropertyChanged;

		#region IsCalendarVisible
		public static BindableProperty IsCalendarVisibleProperty = BindableProperty.Create(
			propertyName: "IsCalendarVisible",
			returnType: typeof(bool),
			declaringType: typeof(bool),
			defaultValue: false);

		public bool IsCalendarVisible
		{
			get => (bool)GetValue(IsCalendarVisibleProperty);
			set => SetValue(IsCalendarVisibleProperty, value);
		}
		#endregion

		#region Backing fields
		private DateTime? _deadline;
		private DateTime? _scheduleDate;
		private DateTime _creationTime;
		private string _description;
		private string _title;
		private DateTime? _reminder;
		#endregion

		#region Properties
		#region Done
		public static BindableProperty DoneProperty = BindableProperty.Create(
			propertyName: "Done",
			returnType: typeof(DateTime?),
			declaringType: typeof(DateTime?));

		public DateTime? Done
		{
			get => (DateTime?)GetValue(DoneProperty);
			set
			{
				SetValue(DoneProperty, value);
				OnPropertyChanged(".");
			}
		}
		#endregion

		public DateTime? Deadline
		{
			get => _deadline;
			set
			{
				SetValue(ref _deadline, value);

				OnPropertyChanged(nameof(HasAttribute));
			}
		}

		#region ScheduleDate
		public static BindableProperty ScheduleDateProperty = BindableProperty.Create(
			"ScheduleDate", typeof(DateTime?), typeof(DateTime?));

		public DateTime? ScheduleDate
		{
			get => (DateTime?)GetValue(ScheduleDateProperty);
			set
			{
				var oldValue = ScheduleDate;
				OnScheduleDateChanging?.Invoke(this, oldValue, value);
				SetValue(ScheduleDateProperty, value);
				OnScheduleDateChanged?.Invoke(this, oldValue, value);
			}
		}
		#endregion

		public DateTime CreateTime
		{
			get => _creationTime;
			set => SetValue(ref _creationTime, value);
		}

		public string Description
		{
			get => _description;
			set
			{
				SetValue(ref _description, value);
				OnPropertyChanged(nameof(HasAttribute));
			}
		}

		public string Title
		{
			get => _title;
			set => SetValue(ref _title, value);
		}

		public DateTime? Reminder
		{
			get => _reminder;
			set
			{
				SetValue(ref _reminder, value); 
				OnPropertyChanged(nameof(HasAttribute));
			}
		}

		public int Id { get; set; }

		public bool HasAttribute => Reminder != null || Deadline != null || !string.IsNullOrWhiteSpace(Description);
		#endregion

		#region Commands
		public ICommand RemoveCommand { get; }
		public ICommand UpdateCommand { get; }
		public ICommand CheckCommand { get; }
		public ICommand SelectDeadlineDateCommand { get; }
		public ICommand RemoveDeadlineCommand { get; }

		public ICommand SelectScheduleDateCommand { get; }
		public ICommand RemoveScheduleDateCommand { get; }

		public ICommand SelectReminderCommand { get; }
		public ICommand RemoveReminderCommand { get; }

		public ICommand EditDescriptionCommand { get; }
		#endregion

		#region Events
		public event Action<TaskViewModel, DateTime?, DateTime?> OnScheduleDateChanging;
		public event Action<TaskViewModel, DateTime?, DateTime?> OnScheduleDateChanged;
		#endregion

		public TaskViewModel(TaskModel taskModel)
		{
			#region Copy taskModel properties
			Id = taskModel.Id;
			Done = taskModel.Done;
			Deadline = taskModel.Deadline;
			Title = taskModel.Title;
			CreateTime = taskModel.CreateTime;
			Description = taskModel.Description;
			Reminder = taskModel.Reminder;
			ScheduleDate = taskModel.ScheduleDate;
			#endregion

			#region Commands
			RemoveCommand = new Command(async () => await RemoveTask());
			UpdateCommand = new Command(async () => await Update());
			CheckCommand = new Command(() =>
			{
				if (Done == null)
					Done = DateTime.Now;
				else
					Done = null;
			});

			SelectDeadlineDateCommand = new Command(async () => await SelectDeadlineDate());
			RemoveDeadlineCommand = new Command(() => Deadline = null);

			EditDescriptionCommand = new Command(EditDescription);

			SelectScheduleDateCommand = new Command(async () => await SelectScheduleDate());
			RemoveScheduleDateCommand = new Command(() => ScheduleDate = null);

			SelectReminderCommand = new Command(async () => await SelectReminder());
			RemoveReminderCommand = new Command(() => Reminder = null);
			#endregion

			_updateOnPropertyChanged = true;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (_updateOnPropertyChanged) Update();
		}

		private async Task RemoveTask()
		{
			var result = await PageService.DisplayAlert(AppResource.DeleteToDo, AppResource.ConfirmDelete, AppResource.Yes,
				AppResource.Cancel);

			if (!result) return;

			await StorageService.DeleteTask(this);

			await PageService.PopAsync();
		}

		public TaskModel GetTaskModel()
		{
			return new TaskModel
			{
				CreateTime = CreateTime,
				Deadline = Deadline,
				Description = Description,
				Done = Done,
				Id = Id,
				Title = Title,
				Reminder = Reminder,
				ScheduleDate = ScheduleDate
			};
		}

		private async Task Update()
		{
			await StorageService.UpdateTask(this);
		}

		private void SelectDateByPicker(Action<DateTime> pickedAction)
		{
			TodayPage.Instance.ShowGlobalDatePicker(Deadline ?? DateTime.Today, pickedAction);
		}

		private async Task SelectDate(Action<DateTime?> pickedAction, string actionSheetHeader)
		{
			string[] buttons = { AppResource.Today, AppResource.Tomorrow, AppResource.TheDayAfterTomorrow, AppResource.PickADate };

			string result = await PageService.DisplayActionSheet(actionSheetHeader, AppResource.Cancel, null, buttons);


			DateTime selectedDate;
			if (result == AppResource.Today)
			{
				selectedDate = DateTime.Today;
			}
			else if (result == AppResource.Tomorrow)
			{
				selectedDate = DateTime.Today + TimeSpan.FromDays(1);
			}
			else if (result == AppResource.TheDayAfterTomorrow)
			{
				selectedDate = DateTime.Today + TimeSpan.FromDays(2);
			}
			else if (result == AppResource.PickADate)
			{
				SelectDateByPicker(d => pickedAction(d));
				return;
			}
			else
				return;

			pickedAction(selectedDate);
		}

		private void SelectTimeByPicker(Action<TimeSpan> pickAction)
		{
			TodayPage.Instance.ShowGlobalTimePicker(pickAction);
		}

		private async Task SelectDeadlineDate()
		{
			await SelectDate(d => Deadline = d, AppResource.ChooseDeadlineDate);
		}

		private async Task SelectScheduleDate()
		{
			await SelectDate(d => ScheduleDate = d, AppResource.ChooseScheduleDate);
		}

		private async Task SelectReminder()
		{
			await SelectDate(SelectReminderContinueWithTime, AppResource.ChooseReminder);
		}

		private void SelectReminderContinueWithTime(DateTime? date)
		{
			Reminder = date;

			if (Reminder == null) return;

			SelectTimeByPicker(t => Reminder = Reminder.Value.Date + t);
		}

		private async void EditDescription()
		{
			await PageService.PushModalAsync(new EditDescriptionPage(this));
		}
	}
}
