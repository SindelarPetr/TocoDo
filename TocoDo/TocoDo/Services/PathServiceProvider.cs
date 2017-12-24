using Xamarin.Forms;

namespace TocoDo.Services
{
	public static class PathServiceProvider
	{
		public static string GetPath()
		{
			return DependencyService.Get<IPathService>().GetPath();
		}
	}
}
