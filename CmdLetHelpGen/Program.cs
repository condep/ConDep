namespace CmdLetHelpGen
{
    sealed class Program
    {
        static void Main(string[] args)
        {
            Lapointe.PowerShell.MamlGenerator.CmdletHelpGenerator.GenerateHelp(args[0], args[1], true);
        }
    }
}
