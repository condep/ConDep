using System.Security.Cryptography.X509Certificates;
using TechTalk.SpecFlow;

namespace ConDep.Dsl.Specs
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
    }
}
