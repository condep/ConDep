using System;
using System.IO;
using System.Xml;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;
using Microsoft.Web.Publishing.Tasks;

namespace ConDep.Dsl
{
	public class TransformWebConfigOperation : IOperateConDep
	{
		private readonly string _webConfigDirPath;
		private readonly string _buildConfigName;

		public TransformWebConfigOperation(string webConfigDirPath, string buildConfigName)
		{
			_webConfigDirPath = webConfigDirPath;
			_buildConfigName = buildConfigName;
		}

		public WebDeploymentStatus Execute(EventHandler<WebDeployMessageEventArgs> output, EventHandler<WebDeployMessageEventArgs> outputError, WebDeploymentStatus webDeploymentStatus)
		{
			var document = new XmlDocument();
			document.Load(Path.Combine(_webConfigDirPath, "web.config"));

			var transformation = new XmlTransformation(Path.Combine(_webConfigDirPath, string.Format("web.{0}.config", _buildConfigName)), new WebTransformLogger(output, outputError));
			bool success = transformation.Apply(document);
			document.Save(Path.Combine(_webConfigDirPath, "webNew.config"));

			if(!success)
				throw new Exception("Failed to transform web.config file.");

			return webDeploymentStatus;
		}

		public bool IsValid(Notification notification)
		{
			throw new NotImplementedException();
		}
	}
}