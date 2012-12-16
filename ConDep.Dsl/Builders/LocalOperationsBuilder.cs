using System;
using System.Collections.Generic;
using ConDep.Dsl.Config;
using ConDep.Dsl.Operations.Application.Local.PreCompile;
using ConDep.Dsl.Operations.Application.Local.TransformConfig;
using ConDep.Dsl.Operations.Application.Local.WebRequest;
using ConDep.Dsl.SemanticModel.Sequence;
using ConDep.Dsl.SemanticModel.WebDeploy;
using TinyIoC;

namespace ConDep.Dsl.Builders
{
    public class LocalOperationsBuilder : IOfferLocalOperations
    {
        private readonly LocalSequence _localSequence;
        private readonly IManageInfrastructureSequence _infrastructureSequence;
        private readonly IEnumerable<ServerConfig> _servers;
        private readonly IOperateWebDeploy _webDeploy;

        public LocalOperationsBuilder(LocalSequence localSequence, IManageInfrastructureSequence infrastructureSequence, IEnumerable<ServerConfig> servers, IOperateWebDeploy webDeploy)
        {
            _localSequence = localSequence;
            _infrastructureSequence = infrastructureSequence;
            _servers = servers;
            _webDeploy = webDeploy;
        }

        //public IManageGeneralSequence Sequence { get { return _localSequence; } }

        public IOfferLocalOperations TransformConfigFile(string configDirPath, string configName, string transformName)
        {
            var operation = new TransformConfigOperation(configDirPath, configName,
                                                                                    transformName);
            _localSequence.Add(operation);
            return this;
        }

        public IOfferLocalOperations PreCompile(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
        {
            var operation = new PreCompileOperation(webApplicationName, webApplicationPhysicalPath,
                                                                          preCompileOutputpath);
            _localSequence.Add(operation);
            return this;
        }

        public IOfferLocalOperations ExecuteWebRequest(string method, string url)
        {
            var operation = new WebRequestOperation(url, method);
            _localSequence.Add(operation);
            return this;
        }

        public IOfferRemoteOperations ToEachServer(Action<IOfferRemoteOperations> action)
        {
            var builder = new RemoteOperationsBuilder(_localSequence.NewRemoteSequence(_infrastructureSequence, _servers), _webDeploy);
            action(builder);
            return builder;
        }

        //public void AddInfrastructure(InfrastructureArtifact infrastructure)
        //{
        //    _infrastructures.Add(infrastructure);    
        //}
    }
}