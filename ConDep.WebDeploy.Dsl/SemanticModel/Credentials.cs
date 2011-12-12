using ConDep.WebDeploy.Dsl.SemanticModel;

namespace ConDep.WebDeploy.Dsl
{
	public class Credentials : IWebDeployModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }

		public bool IsValid(Notification notification)
		{
			return true;		
		}
	}
}