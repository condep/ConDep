using System;
using System.Collections.Generic;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations;
using ConDep.Dsl.SemanticModel.WebDeploy;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class PostOpsSequence : IManageRemoteSequence
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

        public IReportStatus Execute(IReportStatus status, ConDepOptions options)
        {
            try
            {
                Logger.LogSectionStart("Post-Operations");

                var postRemoteOp = new PostRemoteOps();
                postRemoteOp.Configure(this);

                foreach (var server in ConDepGlobals.ServersWithPreOps.Values)
                {
                    foreach (var element in _sequence)
                    {
                        if (element is IOperateRemote)
                        {
                            ((IOperateRemote) element).Execute(server, status, options);
                            if (status.HasErrors)
                                return status;
                        }
                        else if (element is CompositeSequence)
                        {
                            ((CompositeSequence) element).Execute(server, status, options);
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }

                        if (status.HasErrors)
                            return status;
                    }
                }
            }
            finally
            {
                try
                {
                    WebDeployDeployer.DisposeAll();
                }
                catch(Exception ex)
                {
                    Logger.Warn("Unable to remove Web Deploy from server(s).", ex);

                }
                Logger.LogSectionEnd("Post-Operations");
            }

            return status;
        }

    }
}