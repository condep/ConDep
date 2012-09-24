using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using ConDep.Dsl.WebDeploy;
using Microsoft.Web.Publishing.Tasks;

namespace ConDep.Dsl
{
	public class TransformConfigOperation : ConDepOperationBase
	{
		private readonly string _configDirPath;
		private readonly string _configName;
		private readonly string _transformName;
	    private const string CONDEP_CONFIG_EXTENSION = ".orig.condep";

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

			ValidatePaths(configFilePath, transformFilePath);

            if(ConDepConfigBackupExist(_configDirPath, _configName))
            {
                var args = new WebDeployMessageEventArgs();
                args.Level = TraceLevel.Info;
                args.Message = string.Format("Using [{0}] as configuration file to transform", _configDirPath + CONDEP_CONFIG_EXTENSION);
                output(this, args);
                configFilePath = Path.Combine(_configDirPath, _configName + CONDEP_CONFIG_EXTENSION);
            }
            else
            {
                BackupConfigFile(_configDirPath, _configName, output);
            }

            var transformMessage = new WebDeployMessageEventArgs
                                       {
                                           Level = TraceLevel.Info,
                                           Message =
                                               string.Format("Transforming [{0}] using [{1}]", configFilePath,
                                                             transformFilePath)
                                       };
            output(this, transformMessage);
            document.Load(configFilePath);

			var transformation = new XmlTransformation(transformFilePath, new WebTransformLogger(output, outputError));
			var success = transformation.Apply(document);
			document.Save(configFilePath);

			if(!success)
				throw new WebConfigTransformException(string.Format("Failed to transform [{0}] file.", _configName));

			return webDeploymentStatus;
		}

	    private static void ValidatePaths(string configFilePath, string transformFilePath)
	    {
	        if (!File.Exists(configFilePath))
	        {
	            throw new FileNotFoundException(string.Format("File [{0}] does not exist.", configFilePath));
	        }

	        if (!File.Exists(transformFilePath))
	        {
	            throw new FileNotFoundException(string.Format("File [{0}] does not exist.", transformFilePath));
	        }
	    }

	    private bool ConDepConfigBackupExist(string dir, string name)
	    {
	        return File.Exists(Path.Combine(dir, name + CONDEP_CONFIG_EXTENSION));
	    }

	    private void BackupConfigFile(string dir, string name, EventHandler<WebDeployMessageEventArgs> output)
	    {
	        var args = new WebDeployMessageEventArgs
	                       {
	                           Level = TraceLevel.Info,
	                           Message = string.Format("Backing up [{0}] to [{1}]", name, name + CONDEP_CONFIG_EXTENSION)
	                       };

	        output(this, args);
            File.Copy(Path.Combine(dir, name), Path.Combine(dir, name + CONDEP_CONFIG_EXTENSION));
	    }

	    public override bool IsValid(Notification notification)
		{
			return true;
		}
	}
}