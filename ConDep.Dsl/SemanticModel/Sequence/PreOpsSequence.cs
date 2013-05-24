using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class PreOpsSequence : IManageRemoteSequence, IExecuteOnServer
    {
        private readonly List<IExecuteOnServer> _sequence = new List<IExecuteOnServer>();

        public void Add(IOperateRemote operation, bool addFirst = false)
        {
            if (addFirst)
            {
                _sequence.Insert(0, operation);
            }
            else
            {
                _sequence.Add(operation);
            }
        }

        public bool IsValid(Notification notification)
        {
            return true;
        }

        public CompositeSequence NewCompositeSequence(RemoteCompositeOperation operation)
        {
            var sequence = new CompositeSequence(operation.Name);
            _sequence.Add(sequence);
            return sequence;
        }

        public void Execute(ServerConfig server, IReportStatus status, ConDepSettings settings)
        {
            bool sectionAdded = false;
            try
            {
                if (ConDepGlobals.ServersWithPreOps.ContainsKey(server.Name))
                    return;

                ConDepGlobals.ServersWithPreOps.Add(server.Name, server);
                Logger.LogSectionStart("Pre-Operations");
                sectionAdded = true;

                foreach (var element in _sequence)
                {
                    element.Execute(server, status, settings);
                    //if (element is IOperateRemote)
                    //{
                    //    ((IOperateRemote)element).Execute(server, status, options);
                    //    if (status.HasErrors)
                    //        return status;
                    //}
                    //else if (element is CompositeSequence)
                    //{
                    //    ((CompositeSequence)element).Execute(server, status, options);
                    //}
                    //else
                    //{
                    //    throw new NotSupportedException();
                    //}

                    if (status.HasErrors)
                        return;
                }
            }
            finally
            {
                if(sectionAdded)
                {
                    Logger.LogSectionEnd("Pre-Operations");
                }
            }
        }

    }
}