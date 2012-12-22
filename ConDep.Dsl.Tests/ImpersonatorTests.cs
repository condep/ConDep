using ConDep.Dsl.Impersonation;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
    [TestFixture]
    public class ImpersonatorTests
    {
         [Test]
        public void TestThatUserNameWithNoDomainFormatsCorrectly()
         {
             var username = "jat";
             Assert.That(Impersonator.GetUserName(username), Is.EqualTo(username));
             Assert.That(Impersonator.GetDomain(username), Is.EqualTo(""));
         }

        [Test]
        public void TestThatUserNameWithDomainFormatsCorrectly()
        {
            var username = "condep\\jat";
            Assert.That(Impersonator.GetUserName(username), Is.EqualTo("jat"));
            Assert.That(Impersonator.GetDomain(username), Is.EqualTo("condep"));
        }

    }
}