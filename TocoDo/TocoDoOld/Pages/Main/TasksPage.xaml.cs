using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TocoDo.Models;
using TocoDo.Services;
using TocoDo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TasksPage : ContentPage
	{
		public TasksPage()
		{
			InitializeComponent();

			LoadTasks();
		}

		public async void LoadTasks()
		{
			await CreateTasksUi();
		}

		private async Task CreateTasksUi()
		{
			await CreateSomedayTasks();

			await CreateUpcommingTasks();
		}

		private async Task CreateUpcommingTasks()
		{
			var taskGroups = await TocodoService.GetUpcommingTasks();

			bool isFirst = true;
			taskGroups.ForEach(p =>
			{
				CreateHeader(p.Key, isFirst);
				isFirst = false;
				p.Value.ForEach(t => LayoutUpcomming.Children.Add(new TodoItemView(t)));
			});
		}

		private async Task CreateSomedayTasks()
		{
			var tasks = await TocodoService.GetSomedayTasks();

			tasks.ForEach(t => LayoutSomeday.Children.Add(new TodoItemView(t)));
		}


		private void CreateHeader(DateTime date, bool isFirst)
		{
			string text = date.ToString("D");

			var style = Resources["TaskTitleInFrame"];

			Label label = new Label
			{
				Text = text,
				Style = (Style)style,
			};

			if (isFirst)
				label.Margin = new Thickness(0);

			LayoutUpcomming.Children.Add(label);
		}
	}
}