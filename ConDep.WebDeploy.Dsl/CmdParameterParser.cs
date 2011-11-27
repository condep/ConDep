using System;
using System.Collections;
using System.Collections.Generic;

namespace ConDep.WebDeploy.Dsl
{
	internal abstract class CmdParameterParser
	{
		private const char COMMA_CHAR = ',';
		private const char DOUBLE_QUOTE_CHAR = '"';
		private const char EQUALS_CHAR = '=';
		private const char SINGLE_QUOTE_CHAR = '\'';
		private const char SPACE_CHAR = ' ';

		protected abstract void HandleObjectParameterAlreadySpecified(string name);

		internal Dictionary<string, string> Parse(string descriptor)
		{
			var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			while (!string.IsNullOrEmpty(descriptor))
			{
				string str;
				string str2;
				string str3;
				int length = SmartIndexOf(descriptor, COMMA_CHAR);
				if (length == -1)
				{
					str = descriptor;
				}
				else
				{
					str = descriptor.Substring(0, length);
				}
				int num2 = SmartIndexOf(str, EQUALS_CHAR);
				if (num2 == -1)
				{
					str3 = str;
					str2 = string.Empty;
				}
				else
				{
					str3 = str.Substring(0, num2);
					if (num2 < (str.Length - 1))
					{
						str2 = str.Substring(num2 + 1);
					}
					else
					{
						str2 = string.Empty;
					}
				}
				str3 = str3.Trim(new[] { SINGLE_QUOTE_CHAR }).Trim(new[] { DOUBLE_QUOTE_CHAR });
				str2 = str2.Trim(new[] { SINGLE_QUOTE_CHAR }).Trim(new[] { DOUBLE_QUOTE_CHAR });
				if (dictionary.ContainsKey(str3))
				{
					this.HandleObjectParameterAlreadySpecified(str3);
				}
				dictionary.Add(str3, ParseValue(str3, str2));
				if ((length != -1) && (length < (descriptor.Length - 1)))
				{
					descriptor = descriptor.Substring(length + 1);
				}
				else
				{
					descriptor = null;
				}
			}
			return dictionary;
		}

		public static List<string> ParseOnSpace(string input)
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
			return list;
		}

		protected abstract string ParseValue(string name, string value);

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