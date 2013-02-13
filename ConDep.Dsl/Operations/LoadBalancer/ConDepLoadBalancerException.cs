using System;

namespace ConDep.Dsl.Operations.LoadBalancer
{
    public class ConDepLoadBalancerException : Exception
    {
        public ConDepLoadBalancerException(string message)
            : base(message)
        {

        }
    }
}