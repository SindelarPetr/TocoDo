
using System;
using TocoDo.Droid.Services;
using TocoDo.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PathService))]
namespace TocoDo.Droid.Services
{
	public class PathService : IPathService
	{
		public string GetPath()
		{
			return Environment.CurrentDirectory;
		}
	}
}