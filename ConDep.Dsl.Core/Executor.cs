using System;
using System.Linq;
using System.Reflection;

namespace ConDep.Dsl.Core
{
    public static class Executor
    {
        public static void ExecuteFromAssembly(Assembly assembly, ConDepEnvironmentSettings envSettings)
        {
            var type = assembly.GetTypes().Where(t => typeof(ConDepConfigurator).IsAssignableFrom(t)).FirstOrDefault();
            var depObject = assembly.CreateInstance(type.FullName) as ConDepConfigurator;

            if (depObject == null) throw new ArgumentException(string.Format("Unable to create instance of deployment class in assembly [{0}].", assembly.FullName));
            
            ConDepConfigurator.EnvSettings = envSettings;

            //Check for load balancer
            //If load balancer found, take first server offline and deploy
            //If not load balancer found, deploy to all servers
            depObject.Execute();
        }
    }
}