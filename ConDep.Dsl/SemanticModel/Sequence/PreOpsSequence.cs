using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class PreOpsSequence : IManageRemoteSequence
    {
        private readonly List<object> _sequence = new List<object>();

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

        public IReportStatus Execute(ServerConfig server, IReportStatus status, ConDepOptions options)
        {
            bool sectionAdded = false;
            try
            {
                if (ConDepGlobals.ServersWithPreOps.ContainsKey(server.Name))
                    return status;

                ConDepGlobals.ServersWithPreOps.Add(server.Name, server);
                Logger.LogSectionStart("Pre-Operations");
                sectionAdded = true;

                var remotePreOps = new PreRemoteOps(server, this, options);
                remotePreOps.Configure();
                remotePreOps.Execute(status);
                
                foreach (var element in _sequence)
                {
                    if (element is IOperateRemote)
                    {
                        ((IOperateRemote)element).Execute(server, status, options);
                        if (status.HasErrors)
                            return status;
                    }
                    else if (element is CompositeSequence)
                    {
                        ((CompositeSequence)element).Execute(server, status, options);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    if (status.HasErrors)
                        return status;
                }

                return status;
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