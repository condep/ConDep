using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations;

namespace ConDep.Dsl.SemanticModel.Sequence
{
    public class PostOpsSequence : IManageRemoteSequence, IExecute
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

        public void Execute(IReportStatus status, ConDepSettings settings)
        {
            var postRemoteOp = new PostRemoteOps();
            postRemoteOp.Configure(this);

            foreach (var server in ConDepGlobals.ServersWithPreOps.Values)
            {
                foreach (var element in _sequence)
                {
                    element.Execute(server, status, settings);

                    //if (status.HasErrors)
                    //    return;
                }
            }
        }

        public string Name { get { return "Post-Operations"; } }
    }
}