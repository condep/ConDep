using System;
using System.IO;
using System.Text;
using ConDep.Dsl.Builders;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Operations.Application.Execution.PowerShell
{
    public class PowerShellProvider : RemoteCompositeOperation
    {
        public PowerShellProvider(string scriptOrCommand)
        {
            DestinationPath = scriptOrCommand;
        }

        public PowerShellProvider(FileInfo scriptFile)
        {
            throw new NotImplementedException();   
        }

        private bool IsCommand(string scriptOrCommand)
        {
            return !scriptOrCommand.Contains(Environment.NewLine);
        }

        public bool ContinueOnError { get; set; }
        public int WaitIntervalInSeconds { get; set; }
        public int RetryAttempts { get; set; }
        public override void Configure(IOfferRemoteOperations server)
        {
            if (!IsCommand(DestinationPath))
            {
                var builder = new StringBuilder(DestinationPath);
                builder.Replace(Environment.NewLine, "`" + Environment.NewLine);
                DestinationPath = builder.ToString();
            }
            //var script = AddExitCodeHandlingToScript(DestinationPath);
            //var filePath = CreateScriptFile(script);
            //var destFilePath = CopyScriptToDestination(server, filePath);
            //ExecuteScriptOnDestination(server, destFilePath);

            server.ExecuteRemote.DosCommand(string.Format(@"powershell.exe -InputFormat none -Command ""& {{ $ErrorActionPreference='stop'; {0}; exit $LASTEXITCODE }}""", DestinationPath), ContinueOnError, o => o.WaitIntervalInSeconds(WaitIntervalInSeconds));
        }

        //private string AddExitCodeHandlingToScript(string script)
        //{
        //    var builder = new StringBuilder(script);
        //    builder.Insert(0, "$ErrorActionPreference='stop';" + Environment.NewLine);
        //    builder.Append(Environment.NewLine + "exit $LASTEXITCODE");
        //    return builder.ToString();
        //}

        //private void ExecuteScriptOnDestination(ServerConfig server, string destFilePath)
        //{
        //    Configure<ProvideForInfrastructure>(server, po => po.RunCmd(string.Format(@"powershell.exe -InputFormat none -File '{0}'", destFilePath), ContinueOnError, o => o.WaitIntervalInSeconds(WaitInterval)));
        //}

        //private string CopyScriptToDestination(ServerConfig server, string filePath)
        //{
        //    var destFilePath = @"%temp%\" + Path.GetFileName(filePath);
        //    Configure<ProvideForDeployment>(server, c => c.CopyFile(filePath, destFilePath));
        //    return destFilePath;
        //}

        //private string CreateScriptFile(string script)
        //{
        //    var fileName = new Guid().ToString() + ".condep";
        //    var filePath = Path.Combine(Path.GetTempPath(), fileName);
        //    File.WriteAllText(filePath, script);
        //    return filePath;
        //}

        public override bool IsValid(Notification notification)
        {
            return string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(DestinationPath);
        }
    }
}