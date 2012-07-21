using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
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
            // ReSharper disable ConvertToLambdaExpression


            
            //Provider for PowerShell
            
            
            //SetupInfrastructure
            //DeployContent
            //SetupInfrastructure();
            //SetupDeployment();

            Setup(setup =>
                      {
                          //Import-Module Servermanager
                          //Add-WindowsFeature Web-Server,Web-WebServer,Web-Common-Http,Web-Static-Content,Web-Default-Doc,Web-Http-Errors,Web-App-Dev,Web-Asp-Net,Web-Net-Ext,Web-ISAPI-Ext,Web-ISAPI-Filter,Web-Health,Web-Http-Logging,Web-Request-Monitor,Web-Security,Web-Basic-Auth,Web-Windows-Auth,Web-Filtering,Web-Performance,Web-Stat-Compression,Web-Mgmt-Tools,Web-Mgmt-Console,Web-Scripting-Tools,Web-Mgmt-Service
                          //
                          //start /w pkgmgr /iu:IIS-WebServerRole;IIS-WebServer;IIS-CommonHttpFeatures;IIS-StaticContent;IIS-DefaultDocument;IIS-DirectoryBrowsing;IIS-HttpErrors;IIS-ApplicationDevelopment;IIS-ASPNET;IIS-NetFxExtensibility;IIS-ISAPIExtensions;IIS-ISAPIFilter;IIS-HealthAndDiagnostics;IIS-HttpLogging;IIS-RequestMonitor;IIS-Security;IIS-BasicAuthentication;IIS-WindowsAuthentication;IIS-RequestFiltering;IIS-Performance;IIS-HttpCompressionStatic;IIS-WebServerManagementTools;IIS-ManagementConsole;IIS-ManagementService
                          setup.Infrastructure.Windows.InstallIIS();
                          setup.Infrastructure.IIS.EnableRemoteManagement();

                          setup.Infrastructure.IIS.Define(iisDef =>
                                                              {
                                                                  iisDef.WebSite("MyWebSite", 5, @"C:\Web\MyWebSite", webSiteOpt =>
                                                                                                                          {
                                                                                                                              webSiteOpt.HttpsBinding(443, "tjenester.frende.no", binding =>
                                                                                                                                                                                      {
                                                                                                                                                                                          binding.Ip = settings.Ip;
                                                                                                                                                                                          binding.HostName = settings.HostName;
                                                                                                                                                                                      });
                                                                                                                              webSiteOpt.HttpBinding(50, bo=>
                                                                                                                                                             {
                                                                                                                                                                 bo.
                                                                                                                                                             });
                                                                                                                              webSiteOpt.ApplicationPool("MyAppPool");
                                                                                                                              webSiteOpt.WebApp("MyWebApp1");
                                                                                                                              webSiteOpt.WebApp("MyWebApp2");
                                                                                                                              webSiteOpt.WebApp("MyWebApp3");
                                                                                                                          });
                                                              });
                          setup.Deployment(dep =>
                                               {
                                                   dep.CopyDir();
                                               });








































                          setup.PreCompile("MyWebApp", "MyWebAppPath", "outputPath");
                          setup.TransformConfig("", "web.config", "web.dev.config");

                          setup.LoadBalancer.ApplicationRequestRouting("").Farm("").TakeServerOffline("asdfasdf");

                          //What if this is what's applicable for all servers in the definition?
                          setup.Infrastructure.IIS.Define(iisDefinition =>
                            {
                                iisDefinition.WebSite("MyNewWebSite", 3, @"C:\Web\MyNewWebSite", webSiteOptions =>
                                {
                                    webSiteOptions.ApplicationPool("MyAppPool");
                                    webSiteOptions.HttpsBinding(443, "MyCert");
                                    webSiteOptions.HttpBinding(8080);
                                    webSiteOptions.WebApp("MyWebApp1");
                                    webSiteOptions.WebApp("MyWebApp2");
                                    webSiteOptions.WebApp("MyWebApp3");
                                });
                            });

                          //setup.Deployment.
                              setup.Deployment(serverSetup =>
                                                                {
                                                                    //serverSetup.
                                                                    serverSetup.IIS.SyncFromExistingServer("jat-web02", sync =>
                                                                    {
                                                                        sync.Certificate("MyThumbprint");
                                                                        sync.WebSite("ConDep", "MyNewDestSite", @"C:\Web\MyWebSite", options =>
                                                                        {
                                                                            options.Include.AppPools();
                                                                            options.Include.Certificates();
                                                                        });

                                                                    });

                                                                    serverSetup.CopyCertificate(@"C:\temp\myCert.cer");
                                                                    serverSetup.CopyCertificate("jat", X509FindType. FindBySubjectName);





                                                                    //serverSetup.IIS.Define(iis =>
                                                                    //    {
                                                                    //        iis.WebSite("ConDepSite1", 2, @"C:\Web\MyFirstCustomWebSite", options =>
                                                                    //            {
                                                                    //                options.HttpBinding(8080, o => o.HostHeader("blog.torresdal.net").Ip("10.0.0.11"));
                                                                    //                options.HttpsBinding(444, "localhost", o => o.HostHeader("www.con-dep.net").Ip("10.0.0.12"));
                                                                    //                options.ApplicationPool("MyFirstCustomAppPool", o =>
                                                                    //                                                                    {
                                                                    //                                                                        o.NetFrameworkVersion(NetFrameworkVersion.Net4_0);
                                                                    //                                                                        o.Enable32Bit = true;
                                                                    //                                                                        o.ManagedPipeline(ManagedPipeline.Integrated);
                                                                    //                                                                        o.Identity.UserName("torresdal\\jat").Password("asdfasdf");
                                                                    //                                                                        o.IdleTimeoutInMinutes = 10;
                                                                    //                                                                        o.LoadUserProfile = false;
                                                                    //                                                                        o.RecycleTimeIntervalInMinutes = 1000;
                                                                    //                                                                    });
                                                                    //                options.WebApp("MyWebApp");
                                                                    //                options.WebApp("MyWebApp2", o =>
                                                                    //                                                {
                                                                    //                                                    o.PhysicalPath = @"C:\Web\MyWebApp2_2";
                                                                    //                                                    o.ApplicationPool = "MyFirstCustomAppPool";
                                                                    //                                                });
                                                                    //            });

                                                                    //        iis.WebSite("ConDepSite2", 3, @"C:\Web\MySecondCustomWebSite", o=>
                                                                    //                                          {
                                                                    //                                              o.HttpBinding(81);
                                                                    //                                          });
                                                                    //        iis.WebApp("MyWebApp3", "ConDepSite2");
                                                                    //    });








                                                                    // ReSharper restore ConvertToLambdaExpression


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

                                                                    //serverSetup.IIS.SyncFromExistingServer("127.0.0.1", sync =>
                                                                    //                                    {
                                                                    //                                        sync.WebApp("Default Web Site", "MailChimpIntegration", "Default Web Site", "ConDepTestMailChimp");
                                                                    //                                        //sync.WebSite("MySite", "MyNewDestSite");
                                                                    //                                    });


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


                                                                });
                      }

                      );
        }
    }
}
