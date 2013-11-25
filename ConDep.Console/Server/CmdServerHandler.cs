using ConDep.Dsl.Logging;

namespace ConDep.Console.Server
{
    public class CmdServerHandler : IHandleConDepCommands
    {
        private CmdServerHelpWriter _helpWriter;
        private CmdServerParser _parser;

        public CmdServerHandler(string[] args)
        {
            _parser = new CmdServerParser(args);
            _helpWriter = new CmdServerHelpWriter(System.Console.Out);
        }

        public void Execute(CmdHelpWriter helpWriter)
        {
        }

        public void WriteHelp()
        {
            _helpWriter.WriteHelp(_parser.OptionSet);
        }

        public void Cancel()
        {
            
        }
    }
}