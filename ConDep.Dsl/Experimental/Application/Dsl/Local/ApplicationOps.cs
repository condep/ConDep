using System;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.Experimental.Core.Impl;

namespace ConDep.Dsl.Experimental.Application
{
    public class ApplicationOps : IOfferApplicationOps
    {
        private readonly IManageGeneralSequence _localSequence;

        public ApplicationOps(IManageGeneralSequence localSequence)
        {
            _localSequence = localSequence;
        }

        public IManageGeneralSequence Sequence { get { return _localSequence; } }

        public IOfferApplicationOps TransformConfigFile(string configDirPath, string configName, string transformName)
        {
            var operation = new Operations.TransformConfig.TransformConfigOperation(configDirPath, configName,
                                                                                    transformName);
            _localSequence.Add(operation);
            return this;
        }

        public IOfferApplicationOps PreCompile(string webApplicationName, string webApplicationPhysicalPath, string preCompileOutputpath)
        {
            var operation = new Operations.PreCompile.PreCompileOperation(webApplicationName, webApplicationPhysicalPath,
                                                                          preCompileOutputpath);
            _localSequence.Add(operation);
            return this;
        }

        public IOfferApplicationOps ExecuteWebRequest(string method, string url)
        {
            var operation = new Operations.WebRequest.WebRequestOperation(url, method);
            _localSequence.Add(operation);
            return this;
        }

        public IOfferRemoteOptions ToEachServer(Action<IOfferRemoteOptions> action)
        {
            var appServerConfigurator = TinyIoC.TinyIoCContainer.Current.Resolve<IOfferRemoteOptions>();
            action(appServerConfigurator);
            return appServerConfigurator;

        }
    }
}