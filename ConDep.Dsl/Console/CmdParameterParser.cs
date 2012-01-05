using System.Collections;
using System.Collections.Generic;

namespace ConDep.Dsl.Console
{
	internal static class CmdParameterParser
	{
		private const char DOUBLE_QUOTE_CHAR = '"';
		private const char SINGLE_QUOTE_CHAR = '\'';
		private const char SPACE_CHAR = ' ';

		public static List<string> GetCmdParameters(string input)
		{
			var list = new List<string>();
			if (!string.IsNullOrEmpty(input))
			{
				string str = input.Trim();
				while (true)
				{
					int length = SmartIndexOf(str, SPACE_CHAR);
					if (length <= 0)
					{
						break;
					}
					list.Add(str.Substring(0, length));
					str = str.Substring(length + 1).Trim();
				}
				if (!string.IsNullOrEmpty(str))
				{
					list.Add(str);
				}
			}
			if(list.Count > 0)
			{
				list.RemoveAt(0);
			}
			return list;
		}

		private static int SmartIndexOf(string str, char ch)
		{
			var stack = new Stack();
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == ch)
				{
					if (stack.Count == 0)
					{
						return i;
					}
				}
				else if ((str[i] == SINGLE_QUOTE_CHAR) || (str[i] == DOUBLE_QUOTE_CHAR))
				{
					if ((stack.Count == 0) || (((char)stack.Peek()) != str[i]))
					{
						stack.Push(str[i]);
					}
					else
					{
						stack.Pop();
					}
				}
			}
			return -1;
		}
	}
}