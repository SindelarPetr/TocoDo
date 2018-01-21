using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace TocoDo
{
    public static class MyLogger
    {
	    private const int INDENT_MIN = 5;
	    private const char INDENT_CHAR = '=';
	    private const char INDENT_END = '>';
	    private const string WRITE_START = "Started {0} - {1} on line {2}";
	    private const string WRITE_END = "Ended {0} - {1} on line {2}";

	    private static int _indent = INDENT_MIN;

	    private static int Indent
		{
			get => _indent;
			set => _indent = Math.Max(value, INDENT_MIN);
		}

		[Conditional("DEBUG")]
		public static void WriteStartMethod(string message = null,[CallerMemberName] string callerName = null, [CallerFilePath] string callerPath = null, [CallerLineNumber] int callerLineNumber = 0)
	    {
		    _indent++;
			ApplyIndent();
			Write(string.Format(WRITE_START, callerName, Path.GetFileName(callerPath), callerLineNumber));

		    if (message != null)
				WriteLine(": " + message);

	    }

	    [Conditional("DEBUG")]
	    public static void WriteInMethod(string message = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerPath = null, [CallerLineNumber] int callerLineNumber = 0)
	    {
		    ApplyIndent();

			if(message != null)
				Write(": " + message);
		}

		[Conditional("DEBUG")]
	    public static void WriteEndMethod(string message = null,[CallerMemberName] string callerName = null, [CallerFilePath] string callerPath = null, [CallerLineNumber] int callerLineNumber = 0)
		{
			ApplyIndent();
			Write(string.Format(WRITE_START, callerName, Path.GetFileName(callerPath), callerLineNumber));

			if (message != null)
				Write(": " + message);
			_indent--;
		}

	    [Conditional("DEBUG")]
	    private static void ApplyIndent()
	    {
			Write("".PadLeft(_indent, INDENT_CHAR) + INDENT_END);
	    }


	    [Conditional("DEBUG")]
	    public static void Write(string message)
	    {
			Debug.Write(message);
	    }

	    [Conditional("DEBUG")]
	    public static void WriteLine(string message)
	    {
			Debug.WriteLine(message);
	    }
		
	}
}
