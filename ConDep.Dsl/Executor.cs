using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ConDep.Dsl.Core
{
    public static class Executor
    {
        public static void ExecuteFromAssembly(Assembly assembly, ConDepEnvironmentSettings envSettings, TraceLevel traceLevel)
        {
            var type = assembly.GetTypes().Where(t => typeof(ConDepConfiguratorBase).IsAssignableFrom(t)).FirstOrDefault();
            var depObject = assembly.CreateInstance(type.FullName) as ConDepConfiguratorBase;
            if (depObject == null) throw new ArgumentException(string.Format("Unable to create instance of deployment class in assembly [{0}].", assembly.FullName));

            depObject.TraceLevel = traceLevel;
            
            ConDepConfiguratorBase.EnvSettings = envSettings;
            IoCBootstrapper.Bootstrap();

            //Check for load balancer
            //If load balancer found, take first server offline and deploy
            //If not load balancer found, deploy to all servers
            depObject.Configure();
        }
    }
}