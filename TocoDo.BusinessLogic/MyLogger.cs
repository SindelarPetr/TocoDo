using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TocoDo.BusinessLogic
{
	public static class MyLogger
	{
		private const int    INDENT_CHARS_PER_LEVEL = 3;
		private const int    INDENT_MIN             = 0;
		private const char   INDENT_CHAR            = '=';
		private const char   INDENT_END             = '>';
		private const string WRITE_START            = "Started {0} {1} on line {2}";
		private const string WRITE_IN               = "In {0} {1} on line {2}";
		private const string WRITE_END              = "Ended {0} {1} on line {2}";

		private static int _indent = INDENT_MIN;

		private static int Indent
		{
			get => _indent;
			set => _indent = Math.Max(value, INDENT_MIN);
		}

		[Conditional("DEBUG")]
		public static void WriteStartMethod(string message = null, [CallerMemberName] string callerName       = null,
			[CallerFilePath]                                                             string callerPath       = null,
			[CallerLineNumber]                                                           int    callerLineNumber = 0)
		{
			_indent++;
			Write(string.Format(WRITE_START + (message != null ? ": " + message : null), callerName, GetFileName(callerPath),
				callerLineNumber));
		}

		[Conditional("DEBUG")]
		public static void WriteInMethod(string message = null, [CallerMemberName] string callerName       = null,
			[CallerFilePath]                                                          string callerPath       = null,
			[CallerLineNumber]                                                        int    callerLineNumber = 0)
		{
			Write(string.Format(WRITE_IN + (message != null ? ": " + message : null), callerName, GetFileName(callerPath),
				callerLineNumber));
		}

		[Conditional("DEBUG")]
		public static void WriteEndMethod(string message = null, [CallerMemberName] string callerName       = null,
			[CallerFilePath]                                                           string callerPath       = null,
			[CallerLineNumber]                                                         int    callerLineNumber = 0)
		{
			Write(string.Format(WRITE_END + (message != null ? ": " + message : null), callerName, GetFileName(callerPath),
				callerLineNumber));

			_indent--;
		}

		private static string GetIndent()
		{
			return "".PadLeft(_indent * INDENT_CHARS_PER_LEVEL, INDENT_CHAR) + INDENT_END + " ";
		}


		[Conditional("DEBUG")]
		public static void Write(string message)
		{
			Debug.Write(GetIndent() + message);
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message)
		{
			Debug.WriteLine(GetIndent() + message);
		}

		[Conditional("DEBUG")]
		public static void WriteException(Exception ex)
		{
			WriteLine($"ERROR: An exception has been thrown: {ex.Message} -----> with following stack-trace: {ex.StackTrace}");
		}

		public static string GetFileName(string path)
		{
			var parts = path.Split('\\');
			return parts[parts.Length - 1];
		}
	}
}