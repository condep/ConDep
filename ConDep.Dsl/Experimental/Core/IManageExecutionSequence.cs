using System.Collections.Generic;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Experimental.Core
{
    public interface IManageExecutionSequence : IManageGeneralSequence, IValidate
    {
        void Add(IManageRemoteSequence remoteSequence);
        void Add(IManageLocalSequence localSequence);
        IManageRemoteSequence NewRemoteSequence(IEnumerable<ServerConfig> servers);
        IManageLocalSequence NewLocalSequence();
        IReportStatus Execute(IReportStatus status);
    }
}