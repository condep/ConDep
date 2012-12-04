using System.Security.AccessControl;
using NUnit.Framework;
using ConDep.Dsl;

namespace ConDep.Dsl.Tests.Providers
{
    //public class when_using_set_acl_provider : ProviderTestFixture<SetAclProvider, ProvideForInfrastructure>
    //{
    //    protected override void When()
    //    {
    //        Providers
    //            .SetAcl(SourcePath, c => c.Permissions(Permissions, UserName));
    //    }

    //    [Test]
    //    public void should_not_have_source_path()
    //    {
    //        Assert.That(Provider.SourcePath, Is.Null.Or.Empty);
    //    }

    //    [Test]
    //    public void should_have_valid_destination_path()
    //    {
    //        Assert.That(SourcePath, Is.EqualTo(Provider.DestinationPath));
    //    }

    //    [Test]
    //    public void should_have_valid_username()
    //    {
    //        Assert.That(UserName, Is.EqualTo(Provider.User));
    //    }

    //    [Test]
    //    public void should_have_valid_permissions()
    //    {
    //        Assert.That(Permissions, Is.EqualTo(Provider.Permissions));
    //    }

    //    public string SourcePath
    //    {
    //        get { return @"C:\tmp"; }
    //    }

    //    public string UserName
    //    {
    //        get { return @"username"; }
    //    }

    //    public FileSystemRights Permissions
    //    {
    //        get { return FileSystemRights.FullControl; }
    //    }
    //}
}