using System;
using System.Collections.Generic;
using ConDep.Dsl.Operations.Application.Local.PreCompile;
using ConDep.Dsl.Operations.Application.Local.TransformConfig;
using ConDep.Dsl.Operations.Application.Local.WebRequest;
using ConDep.Dsl.SemanticModel.Sequence;
using TinyIoC;

namespace ConDep.Dsl.Builders
{
    public class LocalOperationsBuilder : IOfferLocalOperations
    {
        private readonly IManageGeneralSequence _localSequence;
        private List<InfrastructureArtifact> _infrastructures = new List<InfrastructureArtifact>();

        public LocalOperationsBuilder(IManageGeneralSequence localSequence)
        {
            _localSequence = localSequence;
        }

        public IManageGeneralSequence Sequence { get { return _localSequence; } }

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
            var appServerConfigurator = TinyIoCContainer.Current.Resolve<IOfferRemoteOperations>();
            action(appServerConfigurator);
            return appServerConfigurator;

        }

        public void AddInfrastructure(InfrastructureArtifact infrastructure)
        {
            _infrastructures.Add(infrastructure);    
        }
    }
}