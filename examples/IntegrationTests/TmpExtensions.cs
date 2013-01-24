using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Application.Local.WebRequest;
using ConDep.Dsl.SemanticModel;

namespace ConDep.Dsl.Experimental
{
    public static class TmpExtensions
    {
        public static IOfferLocalOperations WebRequestPut(this IOfferLocalOperations local, string url)
        {
            var op = new WebRequestOperation(url, "PUT");
            Configure.LocalOperations.AddOperation(op);
            return local;
        }
    }
}