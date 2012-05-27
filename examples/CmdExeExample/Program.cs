using System.Security.AccessControl;
using ConDep.Dsl;

namespace TestWebDeployApp
{
    public class Program : ConDepConsoleApp<Program, ISettings>
    {
        static void Main(string[] args)
        {
            Initialize(args);
        }

        protected override void Execute(ISettings settings)
        {
            Setup(setup =>
                      
                          //setup.TransformConfig("", "web.config", "web.dev.config");
                          //setup.PreCompile("MyApp", @"C:\", "C:\temp");
                          //setup.ApplicationRequestRouting("MyNLBServer")
                          //    .LoadBalancer
                          //    .Farm("Sikker")
                          //    .TakeServerOffline("10.0.0.12");

                          setup.Deployment(
                              "myServer", 
                              serverSetup =>
                                    {
                                        //serverSetup.IIS.Define(customIisDefinition =>
                                        //{
                                        //    //iisSetup.AppPool("", "", "");
                                        //    customIisDefinition.WebApp("MyWebApp", "MyWebSite", "");
                                        //    customIisDefinition.WebSite("MyWebSite", "", options =>
                                        //    {
                                        //        //options.Binding("type","ipAddress","port","hostName","certificate");
                                        //        //options.AppPool("MyAppPool","classic","4.0");
                                        //        //options.WebApp("","","",waOptions=>
                                        //        //{
                                        //        //    waOptions.AppPool("","");
                                        //        //});
                                        //    });
                                        //});

                                        serverSetup.IIS.SyncFromExistingServer("someIISServer", sync =>
                                                                            {
                                                                                sync.WebApp("MyWebApp", "", "");
                                                                                //sync.WebSite("MySite", "MyNewDestSite");
                                                                            });

                                        //serverSetup.Certificate(@"C:\cert.cer");
                                        ////serverSetup.Certificate("srcServer", "thumbprint");
                                        ////serverSetup.CopyDir("srcServer", "srcDir", "dstDir");
                                        ////serverSetup.CopyDir("srcDir", "dstDir");
                                        ////serverSetup.CopyFile("srcServer", "srcFile", "dstFile");
                                        ////serverSetup.CopyFile("srcFile", "dstFile");
                                        //serverSetup.RunCmd("ipconfig /flushdns");
                                        //serverSetup.PowerShell("Get-IPConfig");
                                        //serverSetup.SetAcl("", o => o.Permissions(FileSystemRights.Read, ""));
                                        //serverSetup.NServiceBus("", "", o => o.DestinationDir(""));
                                        ////serverSetup.WindowsService("");


                                    } )

                      );
        }
    }
}
