namespace ConDep.Dsl.Builders
{
    public interface IOfferIisInfrastructureRoleServices
    {
        IOfferIisInfrastructureRoleServices HttpRedirect();
        IOfferIisInfrastructureRoleServices DAVPublishing();
        IOfferIisInfrastructureRoleServices AspNet();
        IOfferIisInfrastructureRoleServices ASP();
        IOfferIisInfrastructureRoleServices CGI();
        IOfferIisInfrastructureRoleServices ServerSideIncludes();
        IOfferIisInfrastructureRoleServices LogLibraries();
        IOfferIisInfrastructureRoleServices HttpTracing();
        IOfferIisInfrastructureRoleServices CustomLogging();
        IOfferIisInfrastructureRoleServices ODBCLogging();
        IOfferIisInfrastructureRoleServices BasicAuth();
        IOfferIisInfrastructureRoleServices WindowsAuth();
        IOfferIisInfrastructureRoleServices DigestAuth();
        IOfferIisInfrastructureRoleServices ClientAuth();
        IOfferIisInfrastructureRoleServices CertAuth();
        IOfferIisInfrastructureRoleServices UrlAuth();
        IOfferIisInfrastructureRoleServices IPSecurity();
        IOfferIisInfrastructureRoleServices DynamicCompression();
        IOfferIisInfrastructureRoleServices ScriptingTools();
        IOfferIisInfrastructureRoleServices MgmtService();
        IOfferIisInfrastructureRoleServices RemoveIfExist { get; set; }
    }
}