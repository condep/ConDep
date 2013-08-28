namespace ConDep.Dsl.Security
{
    public class EncryptedPassword
    {
        public EncryptedPassword(string iv, string encryptedPassword)
        {
            IV = iv;
            Password = encryptedPassword;
        }

        public string IV { get; private set; }

        public string Password { get; private set; }
    }
}