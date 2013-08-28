namespace ConDep.Console.Help
{
    public class ConDepHelpOptions
    {
        public bool NoOptions()
        {
            return Command == ConDepCommand.NotFound;
        }

        public ConDepCommand Command { get; set; }
    }
}