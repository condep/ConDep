namespace ConDep.Dsl.Config
{
    /// <summary>
    /// A container object for <see cref="ConDepEnvConfig"/> and <see cref="ConDepOptions"/>.
    /// </summary>
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