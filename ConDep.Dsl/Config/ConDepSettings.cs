namespace ConDep.Dsl.Config
{
    public class ConDepSettings
    {
        public ConDepSettings()
        {
            Config = new ConDepEnvConfig();
            Options = new ConDepOptions();
        }

        public ConDepEnvConfig Config { get; set; }
        public ConDepOptions Options { get; set; }
    }
}