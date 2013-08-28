using System;
using System.Globalization;
using System.Linq;
using ConDep.Console.Help;

namespace ConDep.Console
{
    public class CmdFactory
    {
        private string[] _args;

        public CmdFactory(string[] args)
        {
            _args = args;
        }

        public static IHandleConDepCommands Resolve(string[] args)
        {   
            return new CmdFactory(args).Resolve();
        }

        private string CmdName
        {
            get
            {
                if(_args == null || _args.Length == 0)
                    throw new ConDepCmdParseException("No arguments in argument list.");

                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_args[0]);
            }
        }

        private string[] Args
        {
            get
            {
                if (_args == null || _args.Length < 2)
                {
                    return new string[]{};
                }
                return _args.Skip(1).ToArray();
            }
        }
        public IHandleConDepCommands Resolve()
        {
            try
            {
                if (_args.Length == 0)
                {
                    return new CmdHelpHandler(Args);
                }

                var conventionType = GetType().Assembly.GetTypes().Single(type => type.Name == "Cmd" + CmdName + "Handler");
                return (IHandleConDepCommands) conventionType.GetConstructors().First().Invoke(new object[] {Args});
            }
            catch (Exception ex)
            {
                throw new ConDepCmdParseException(string.Format("The command [{0}] is not known to ConDep.", CmdName), ex);
            }

            //var instance = conventionType.Assembly.CreateInstance(conventionType.FullName);
            //return instance;

            //if (args == null || args.Length == 0)
            //{
            //    return new CmdHelpHandler(new CmdHelpParser(args), new CmdHelpValidator(), new CmdHelpWriter(System.Console.Out));
            //}

            //var cmd = args[0].ToLower();
            //if (cmd == "deploy")
            //    return new CmdDeployHandler(new CmdDeployParser(args), new CmdDeployValidator(), new CmdHelpWriter(System.Console.Out));

            //if (cmd == "encrypt")
            //    return null;

            //if (cmd == "decrypt")
            //    return null;

            //if (cmd == "help")
            //    return new CmdHelpHandler(new CmdHelpParser(args), new CmdHelpValidator(), new CmdHelpWriter(System.Console.Out));

            //throw new ConDepCmdParseException("No command found.");
        }

    }
}