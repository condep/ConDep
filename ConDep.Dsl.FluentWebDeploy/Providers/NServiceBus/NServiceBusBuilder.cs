namespace ConDep.Dsl.FluentWebDeploy
{
    public class NServiceBusBuilder
    {
        private readonly NServiceBusProvider _nservicebusProvider;

        public NServiceBusBuilder(NServiceBusProvider nservicebusProvider)
        {
            _nservicebusProvider = nservicebusProvider;
        }
        
        public NServiceBusBuilder ToDirectory(string path)
        {
            _nservicebusProvider.DestinationPath = path;
            return this;
        }
    }
}