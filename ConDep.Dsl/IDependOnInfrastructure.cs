namespace ConDep.Dsl
{
    /// <summary>
    /// A generic marker interface to let an ApplicationArtifact depend on a InfrastructureArtifact
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDependOnInfrastructure<T> where T : InfrastructureArtifact
    {   
    }
}