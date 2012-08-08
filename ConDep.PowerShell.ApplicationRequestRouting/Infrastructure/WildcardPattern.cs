using System.Text.RegularExpressions;

namespace ConDep.PowerShell.ApplicationRequestRouting.Infrastructure
{
    public class WildcardPattern
    {
        private readonly string _value;

        public WildcardPattern(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

        public bool IsMatch(string s)
        {
            var escapedQuery = Regex.Escape(_value);
            var regex = new Regex(escapedQuery.Replace("\\*", ".*"));
            return regex.IsMatch(s);
        }

        public static bool ContainsWildcard(string value)
        {
            return value.Contains("*");
        }
    }
}