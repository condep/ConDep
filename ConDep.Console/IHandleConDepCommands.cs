using System.IO;
using ConDep.Dsl.Logging;

namespace ConDep.Console
{
    public interface IHandleConDepCommands//<TParser, TValidator>
    {
        void Execute(CmdHelpWriter helpWriter);
        //TParser Parser { get; }
        //TValidator Validator { get; }
        void WriteHelp();
        void Cancel();
    }
}