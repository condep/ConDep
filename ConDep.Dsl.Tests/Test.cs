using Microsoft.Web.Administration;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void TestMethod()
		{
			using(var serverManager = ServerManager.OpenRemote("ffdevnlb01"))
			{
				
			}
		}
	}
}