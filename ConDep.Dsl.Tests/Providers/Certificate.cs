using System.Security.Cryptography.X509Certificates;
using System.Text;
using NUnit.Framework;

namespace ConDep.Dsl.Tests.Providers
{
    //public class when_using_certificate_provider : ProviderTestFixture<CertficiateDeploymentProvider, ProvideForDeploymentIis>
    //{
    //    private X509Certificate2 _cert;

    //    protected override void Before()
    //    {
    //        InstallCertInStore();
    //    }

    //    protected override void When()
    //    {
    //        Providers.Certificate(SourcePath);
    //    }

    //    protected override void After()
    //    {
    //        RemoveCertFromStore();
    //    }

    //    private void InstallCertInStore()
    //    {
    //        var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
    //        store.Open(OpenFlags.ReadWrite);
    //        store.Add(Certificate); 
    //        store.Close();
    //    }

    //    private void RemoveCertFromStore()
    //    {
    //        var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
    //        store.Open(OpenFlags.ReadWrite);
    //        store.Remove(Certificate); 
    //        store.Close();
    //    }

    //    private X509Certificate2 Certificate
    //    {
    //        get
    //        {
    //            if(_cert == null)
    //            {
    //                var certArray = Encoding.UTF8.GetBytes(Properties.Resources.ConDepCert);
    //                _cert = new X509Certificate2(certArray);
    //            }
    //            return _cert;
    //        }
    //    }

    //    [Test]
    //    public void should_have_valid_source_path()
    //    {
    //        Assert.That(SourcePath, Is.EqualTo(Provider.SourcePath));
    //    }

    //    [Test]
    //    public void should_not_have_destination_path()
    //    {
    //        Assert.That(Provider.DestinationPath, Is.Null.Or.Empty);
    //    }

    //    public string SourcePath
    //    {
    //        get { return "dfc1228322458329b37c07a895bba599c797ed5b"; }
    //    }
    //}
}