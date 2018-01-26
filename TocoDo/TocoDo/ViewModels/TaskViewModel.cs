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
using TocoDo.Views.Habits;

namespace TocoDo.ViewModels
{
	public class TaskViewModel : BaseViewModel, ICreateMode
	{
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
				if(_done == null || value == null)
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
		public DateTime CreateTime
		{
			get;
		}
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
		public ICommand RemoveCommand { get; set; }
		public ICommand UpdateCommand { get; set; }
		public ICommand EditDescriptionCommand { get; set; }
		#endregion

		public TaskViewModel()
		{
			IsCreateMode = true;
			InitCommands();
		}

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

			InitCommands();
		}

		private void InitCommands()
		{
			RemoveCommand = new MyCommand(async () => await RemoveTask());
			UpdateCommand = new MyCommand(async () => await Update());

			EditDescriptionCommand = new MyCommand(EditDescription);
		}

		protected override async void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if(!IsCreateMode)
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

		// Todo: Get rid of this
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

		public async void InsertToStorage(string title)
		{
			_title = title;
			OnPropertyChanged(nameof(Title));
			IsCreateMode = false;
			await StorageService.InsertTask(this, false);
		}
	}
}
