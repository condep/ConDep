using ConDep.Dsl.FluentWebDeploy.SemanticModel;

namespace ConDep.Dsl.FluentWebDeploy
{
    public class NServiceBusProvider : CompositeProvider
    {
        public NServiceBusProvider(string path)
        {
            SourcePath = path;

            Sync(p => p.CopyDir(path)
                          .SetRemotePathTo(DestinationPath));
        }

        public override bool IsValid(Notification notification)
        {
            var valid = true;
            foreach (var childProvider in ChildProviders)
            {
                if(!childProvider.IsValid(notification))
                {
                    valid = false;
                }
            }
            return valid;
        }
    }
}