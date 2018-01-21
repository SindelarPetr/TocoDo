using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.Helpers;
using TocoDo.Models;
using TocoDo.Pages;
using TocoDo.Pages.Main;
using TocoDo.Properties;
using TocoDo.Services;

namespace TocoDo.ViewModels
{
	public class TaskViewModel : BaseViewModel
	{
		#region Backing fields
		private DateTime? _deadline;
		private DateTime _creationTime;
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
				SetValue(ref _done, value);
				OnPropertyChanged(".");
			}
		}
		public DateTime? Deadline
		{
			get => _deadline;
			set
			{
				SetValue(ref _deadline, value);

				OnPropertyChanged(nameof(HasAttribute));
			}
		}
		public DateTime? ScheduleDate
		{
			get => _scheduleDate;
			set => SetValue(ref _scheduleDate, value);
		}
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
				if(SetValue(ref _description, value));
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

		/// <summary>
		/// TODO: Get rid of this, Replace with trigger
		/// </summary>
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

		public TaskViewModel(TaskModel taskModel)
		{
			#region Copy taskModel properties
			Id = taskModel.Id;
			_done = taskModel.Done;
			_deadline = taskModel.Deadline;
			_title = taskModel.Title;
			CreateTime = taskModel.CreateTime;
			_description = taskModel.Description;
			_reminder = taskModel.Reminder;
			_scheduleDate = taskModel.ScheduleDate;
			#endregion
			
			#region Commands
			RemoveCommand = new MyCommand(async () => await RemoveTask());
			UpdateCommand = new MyCommand(async () => await Update());
			CheckCommand = new MyCommand(() =>
			{
				if (Done == null)
					Done = DateTime.Now;
				else
					Done = null;
			});

			// TODO: use triggers to remove some commands
			SelectDeadlineDateCommand = new MyCommand(async () => await SelectDeadlineDate());
			RemoveDeadlineCommand = new MyCommand(() => Deadline = null);

			EditDescriptionCommand = new MyCommand(EditDescription);

			SelectScheduleDateCommand = new MyCommand(async () => await SelectScheduleDate());
			RemoveScheduleDateCommand = new MyCommand(() => ScheduleDate = null);

			SelectReminderCommand = new MyCommand(async () => await SelectReminder());
			RemoveReminderCommand = new MyCommand(() => Reminder = null);
			#endregion
		}

		protected override async void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			await Update();
		}

		private async Task RemoveTask()
		{
			var result = await PageService.DisplayAlert(Resources.DeleteToDo, Resources.ConfirmDelete, Resources.Yes,
				Resources.Cancel);

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
			string[] buttons = { Resources.Today, Resources.Tomorrow, Resources.TheDayAfterTomorrow, Resources.PickADate };

			string result = await PageService.DisplayActionSheet(actionSheetHeader, Resources.Cancel, null, buttons);


			DateTime selectedDate;
			if (result == Resources.Today)
			{
				selectedDate = DateTime.Today;
			}
			else if (result == Resources.Tomorrow)
			{
				selectedDate = DateTime.Today + TimeSpan.FromDays(1);
			}
			else if (result == Resources.TheDayAfterTomorrow)
			{
				selectedDate = DateTime.Today + TimeSpan.FromDays(2);
			}
			else if (result == Resources.PickADate)
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
			await SelectDate(d => Deadline = d, Resources.ChooseDeadlineDate);
		}

		private async Task SelectScheduleDate()
		{
			await SelectDate(d => ScheduleDate = d, Resources.ChooseScheduleDate);
		}

		private async Task SelectReminder()
		{
			await SelectDate(SelectReminderContinueWithTime, Resources.ChooseReminder);
		}

		private void SelectReminderContinueWithTime(DateTime? date)
		{
			Reminder = date;

			if (Reminder == null) return;

			SelectTimeByPicker(t => Reminder = Reminder.Value.Date + t);
		}

		private async void EditDescription()
		{
			Debug.Write("------------- Edit description called.");
			await PageService.PushModalAsync(new EditDescriptionPage(Title, Description, d => Description = d));
			Debug.Write("------------- Stopped calling edit description.");
		}
	}
}
