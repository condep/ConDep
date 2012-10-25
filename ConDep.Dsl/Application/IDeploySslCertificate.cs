namespace ConDep.Dsl.Application
{
    public interface IDeploySslCertificate
    {
        void FromStore();
        void FromFile();
    }
}