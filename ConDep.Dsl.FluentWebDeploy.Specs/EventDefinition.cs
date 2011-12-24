using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.FluentWebDeploy.Specs
{
    [Binding]
    public class EventDefinition
    {
        [AfterScenario("package")]
        public static void AfterScenario()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, "6bc83fd84c0f1f90e776d86af6230d44e6ea0acb", false);
            store.RemoveRange(certs);
            store.Close();
        }

        [BeforeScenario("thumbprint")]
        public static void BeforeScenario()
        {
            //var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            //store.Open(OpenFlags.ReadWrite);

            //var currentPath = Directory.GetCurrentDirectory();// Path. .GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            //var certPath = Path.Combine(currentPath, "SpecTestCert.pfx");
            //var cert = new X509Certificate2(certPath, "test123");

            //store.Add(cert);
            //store.Close();
        }
    }
}
