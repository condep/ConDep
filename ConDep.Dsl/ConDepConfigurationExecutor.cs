using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ConDep.Dsl;

namespace ConDep.Dsl
{
    public class ConDepConfigurationExecutor
    {
        public void Execute(Assembly assembly, ConDepEnvironmentSettings envSettings, ConDepOptions options)
        {
            if (assembly == null) { throw new ArgumentException("assembly"); }
            if (envSettings == null) { throw new ArgumentException("envSettings"); }

            var type = assembly.GetTypes().Where(t => typeof(ConDepConfiguratorBase).IsAssignableFrom(t)).FirstOrDefault();
            if (type == null)
            {
                throw new ConDepConfigurationTypeNotFound(string.Format("A class inheriting from [{0}] must be present in assembly [{1}] for ConDep to work.", typeof(ConDepConfiguratorBase).FullName, assembly.FullName));
            }

            var depObject = assembly.CreateInstance(type.FullName) as ConDepConfiguratorBase;
            if (depObject == null) throw new NullReferenceException(string.Format("Instance of configuration class [{0}] in assembly [{1}] is null.", type.FullName, assembly.FullName));

            depObject.Options = options;
            IoCBootstrapper.Bootstrap(envSettings);
            depObject.Configure();
        }

        public static void ExecuteFromAssembly(Assembly assembly, ConDepEnvironmentSettings envSettings, ConDepOptions options)
        {
            new ConDepConfigurationExecutor().Execute(assembly, envSettings, options);
        }
    }
}