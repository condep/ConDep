namespace ConDep.Dsl.LoadBalancer
{
    public interface ILookupLoadBalancer
    {
        ILoadBalance GetLoadBalancer();
    }
}