using ConDep.Dsl.Operations.Windows;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class WindowsInfrastructureOptions : IOfferWindowsInfrastructureOptions
    {
        private readonly WindowsFeatureInfrastructureOperation _operation;

        public WindowsInfrastructureOptions(WindowsFeatureInfrastructureOperation operation)
        {
            _operation = operation;
        }

        public IOfferWindowsInfrastructureOptions InstallFeature(string featureName)
        {
            _operation.AddWindowsFeature(featureName);
            return this;
        }

        public IOfferWindowsInfrastructureOptions UninstallFeature(string featureName)
        {
            _operation.RemoveWindowsFeature(featureName);
            return this;
        }
    }
}