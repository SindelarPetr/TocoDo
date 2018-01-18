using System;
using System.Collections.Generic;
using System.Text;

namespace TocoDo.Helpers
{
    public static class FakeIdGenerator
    {
	    private static int _id = 5;
	    public static int GetId() => _id++;
    }
}
