using System.Collections.Generic;
using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageExecutionSequence : IManageGeneralSequence, IValidate
    {
        void Add(IManageRemoteSequence remoteSequence);
        void Add(IManageLocalSequence localSequence);
        IManageRemoteSequence NewRemoteSequence(IEnumerable<ServerConfig> servers);
        IManageRemoteSequence NewRemoteSequenceNoLoadBalancing(IEnumerable<ServerConfig> servers);
        IManageLocalSequence NewLocalSequence();
        IReportStatus Execute(IReportStatus status);
    }
}