namespace ConDep.Dsl.Core
{
	public class Credentials : IValidate
	{
		public string UserName { get; set; }
		public string Password { get; set; }

		public bool IsValid(Notification notification)
		{
			return true;		
		}
	}
}