using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Input;
using TocoDo.Models;
using TocoDo.Services;

namespace TocoDo.ViewModels
{
	public class HabitViewModel : BaseViewModel
	{
		private bool _updateOnPropertyChange;

		#region Backing fields

		private bool _modelIsRecommended;
		private int _modelDailyFillingCount;
		private HabitType _modelHabitType;
		private short _modelRepeatNumber;
		private DateTime? _modelStartDate;
		private string _modelTitle;
		private RepeatType _modelRepeatType;
		private string _modelDescription;

		#endregion

		#region Properties
		public int ModelId { get; }

		public bool ModelIsRecommended
		{
			get => _modelIsRecommended;
			set => SetValue(ref _modelIsRecommended, value);
		}

		public int ModelDailyFillingCount
		{
			get => _modelDailyFillingCount;
			set => SetValue(ref _modelDailyFillingCount, value);
		}

		public ObservableDictionary<DateTime, int> ModelFilling { get; }

		public HabitType ModelHabitType
		{
			get => _modelHabitType;
			set => SetValue(ref _modelHabitType, value);
		}

		public short ModelRepeatNumber
		{
			get => _modelRepeatNumber;
			set => SetValue(ref _modelRepeatNumber, value);
		}

		public DateTime? ModelStartDate
		{
			get => _modelStartDate;
			set => SetValue(ref _modelStartDate, value);
		}

		public string ModelTitle
		{
			get => _modelTitle;
			set => SetValue(ref _modelTitle, value);
		}

		public RepeatType ModelRepeatType
		{
			get => _modelRepeatType;
			set => SetValue(ref _modelRepeatType, value);
		}

		public string ModelDescription
		{
			get => _modelDescription;
			set => SetValue(ref _modelDescription, value);
		}
		#endregion

		#region Commands
		public ICommand UpdateCommand { get; }
		#endregion

		public HabitViewModel(HabitModel model)
		{
			ModelId = model.Id;
			_modelRepeatType = model.RepeatType;
			_modelDescription = model.Description;
			ModelFilling = new ObservableDictionary<DateTime, int>(model.Filling);
			_modelHabitType = model.HabitType;
			_modelRepeatNumber = model.RepeatNumber;
			_modelStartDate = model.StartDate;
			_modelTitle = model.Title;
			_modelDailyFillingCount = model.DailyFillingCount;
			_modelIsRecommended = model.IsRecommended;
			_updateOnPropertyChange = true;
		}

		public HabitModel GetHabitModel()
		{
			return new HabitModel
			{
				Id = ModelId,
				RepeatType = ModelRepeatType,
				Description = ModelDescription,
				Filling = new Dictionary<DateTime, int>(ModelFilling),
				HabitType = ModelHabitType,
				RepeatNumber = ModelRepeatNumber,
				StartDate = ModelStartDate,
				Title = ModelTitle,
				DailyFillingCount = ModelDailyFillingCount,
				IsRecommended = ModelIsRecommended
			};
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (_updateOnPropertyChange)
				StorageService.UpdateHabit(this);
		}
	}
}