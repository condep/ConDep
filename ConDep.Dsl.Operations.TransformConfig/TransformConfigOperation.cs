using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using ConDep.Dsl.Core;
using Microsoft.Web.Publishing.Tasks;

namespace ConDep.Dsl
{
	public class TransformConfigOperation : ConDepOperation
	{
		private readonly string _configDirPath;
		private readonly string _configName;
		private readonly string _transformName;

		public TransformConfigOperation(string configDirPath, string configName, string transformName)
		{
			_configDirPath = configDirPath;
			_configName = configName;
			_transformName = transformName;
		}

        public override WebDeploymentStatus Execute(TraceLevel traceLevel, EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			var document = new XmlDocument();
			var configFilePath = Path.Combine(_configDirPath, _configName);
			var transformFilePath = Path.Combine(_configDirPath, _transformName);

			if(!File.Exists(configFilePath))
			{
				throw new FileNotFoundException(string.Format("File [{0}] does not exist.", configFilePath));
			}

			if (!File.Exists(transformFilePath))
			{
				throw new FileNotFoundException(string.Format("File [{0}] does not exist.", transformFilePath));
			}

			document.Load(configFilePath);

			var transformation = new XmlTransformation(transformFilePath, new WebTransformLogger(output, outputError));
			var success = transformation.Apply(document);
			document.Save(configFilePath);

			if(!success)
				throw new Exception("Failed to transform web.config file.");

			return webDeploymentStatus;
		}

        public override bool IsValid(Notification notification)
		{
			return true;
		}
	}
}