using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Model.Config;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Experimental.Core.Impl
{
    public class RemoteSequenceManager : IManageRemoteSequence
    {
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly List<IOperateRemote> _sequence = new List<IOperateRemote>();

        public RemoteSequenceManager(IEnumerable<ServerConfig> servers)
        {
            _servers = servers;
        }

        public void Add(IOperateRemote remoteOperation)
        {
            _sequence.Add(remoteOperation);
        }

        //public IManageLocalSequence NewLocalSequence()
        //{
        //    var localSeq = new LocalSequenceManager();
        //    _sequence.Add(localSeq);
        //    return localSeq;
        //}

        public IReportStatus Execute(IReportStatus status)
        {
            foreach(var server in _servers)
            {
                foreach(var element in _sequence)
                {
                    element.Execute(server, status);
                    if (status.HasErrors)
                        return status;
                }
            }
            return status;
        }

        public bool IsValid(Notification notification)
        {
            return !_sequence.Any(x => x.IsValid(notification) == false);
        }
    }
}