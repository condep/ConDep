using ConDep.Dsl.Builders;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebApp
{
    public class IisWebAppOptions : IOfferIisWebAppOptions
    {
        private readonly IisWebAppOptionsValues _values;

        public IisWebAppOptions(string name)
        {
            _values = new IisWebAppOptionsValues(name);
        }

        public IOfferIisWebAppOptions PhysicalPath(string path)
        {
            _values.PhysicalPath = path;
            return this;
        }

        public IOfferIisWebAppOptions AppPool(string name)
        {
            _values.AppPool = name;
            return this;
        }

        public IisWebAppOptionsValues Values { get { return _values; } }

        public class IisWebAppOptionsValues
        {
            private readonly string _name;

            public IisWebAppOptionsValues(string name)
            {
                _name = name;
            }

            public string PhysicalPath { get; set; }
            public string AppPool { get; set; }
            public string Name { get { return _name; } }
        }
    }
}