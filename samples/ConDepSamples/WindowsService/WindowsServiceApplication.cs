using ConDep.Dsl;
using ConDep.Dsl.Config;

namespace ConDepSamples.WindowsService
{
    public class WindowsServiceApplication : ApplicationArtifact
    {
        public override void Configure(IOfferLocalOperations onLocalMachine, ConDepConfig config)
        {
            onLocalMachine.ToEachServer
            (
                //Deploy a Windows Service to remote server(s)
                server => server.Deploy.WindowsService
                (
                    serviceName:        "WindowsServiceApp",                                    
                    displayName:        "ConDep Windows Service Application",                   
                    sourceDir:          @"..\..\..\SampleApps\WindowsServiceApp\bin\Debug",     
                    destDir:            @"C:\ConDepSamples\WindowsServiceApp",                  
                    relativeExePath:    "WindowsServiceApp.exe"
                )
            );
        }
    }
}