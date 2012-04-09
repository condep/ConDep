using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace ConDep.Dsl.Console
{
    internal static class CmdLineHandler
    {
        public static IEnumerable<string> GetCmdParams()
        {
            return CmdParameterParser.GetCmdParameters(GetCmdLine());
        }

        public static string GetEnvFromCmdLine(IEnumerable<CmdParam> cmdParams)
        {
            var param = cmdParams.FirstOrDefault(x => x.ParamName == "env");
            if (param == null)
            {
                throw new ArgumentException("Command line argument for environment [env] is missing.");
            }

            return param.ParamValue;
        }

        public static IEnumerable<CmdParam> ExtractSettingsParams(IEnumerable<string> cmdParams)
        {
            return cmdParams.
                Where(p => p.Contains('=')).
                Select(param =>
                           {
                               var paramNameValue = param.Split('=');
                               return new CmdParam
                                          {
                                              ParamName = paramNameValue[0],
                                              ParamValue = paramNameValue[1]
                                          };
                           });
        }

        public static bool UserNeedsHelp(IEnumerable<string> cmdParams)
        {
            return cmdParams.Any(x =>
                                 x.Equals("/?") ||
                                 x.Equals("-?") ||
                                 x.Equals("-help", StringComparison.OrdinalIgnoreCase) ||
                                 x.Equals("--help", StringComparison.OrdinalIgnoreCase));
        }

        private static string GetCmdLine()
        {
            string commandLine;
            try
            {
                commandLine = Environment.CommandLine;
            }
            catch (SecurityException exception)
            {
                throw new Exception("Insufficient permissions to run exe", exception);
            }
            return commandLine;
        }
    }
}