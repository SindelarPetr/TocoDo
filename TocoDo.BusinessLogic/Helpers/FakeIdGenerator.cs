namespace TocoDo.BusinessLogic.Helpers
{
	public static class FakeIdGenerator
	{
		private static int _id = 5;

		public static int GetId()
		{
			return _id++;
		}
	}
}