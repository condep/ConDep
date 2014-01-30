using System.Web.Http.Dependencies;
using StructureMap;

namespace ConDep.Server
{
    public class StructureMapDependencyResolver : StructureMapScope, IDependencyResolver
    {
        private readonly IContainer container;

        public StructureMapDependencyResolver(IContainer container) : base(container)
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            var childContainer = container.GetNestedContainer();
            return new StructureMapScope(childContainer);
        }
    }
}