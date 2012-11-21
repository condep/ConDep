using System.IO;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Operations.TransformConfig
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

        public override IReportStatus Execute(IReportStatus status)
        {
			var configFilePath = Path.Combine(_configDirPath, _configName);
			var transformFilePath = Path.Combine(_configDirPath, _transformName);
            var backupPath = "";

			ValidatePaths(configFilePath, transformFilePath);

            if(ConDepConfigBackupExist(_configDirPath, _configName))
            {
                Logger.Info("Using [{0}] as configuration file to transform", _configDirPath + CONDEP_CONFIG_EXTENSION);
                backupPath = Path.Combine(_configDirPath, _configName + CONDEP_CONFIG_EXTENSION);
            }
            else
            {
                BackupConfigFile(_configDirPath, _configName);
            }

            Logger.Info("Transforming [{0}] using [{1}]", configFilePath, transformFilePath);
            var trans = new SlowCheetah.Tasks.TransformXml
            {
                BuildEngine = new TransformConfigBuildEngine(),
                Source = string.IsNullOrWhiteSpace(backupPath) ? configFilePath : backupPath,
                Transform = transformFilePath,
                Destination = configFilePath
            };

            var success = trans.Execute();
			if(!success)
				throw new CondepWebConfigTransformException(string.Format("Failed to transform [{0}] file.", _configName));

			return status;
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

	    private void BackupConfigFile(string dir, string name)
	    {
            Logger.Info("Backing up [{0}] to [{1}]", name, name + CONDEP_CONFIG_EXTENSION);
            File.Copy(Path.Combine(dir, name), Path.Combine(dir, name + CONDEP_CONFIG_EXTENSION));
	    }

	    public override bool IsValid(Notification notification)
		{
			return true;
		}
	}
}