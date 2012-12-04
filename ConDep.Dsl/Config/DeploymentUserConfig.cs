namespace ConDep.Dsl.Config
{
    public class DeploymentUserConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDefined { get { return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); } }
    }
}