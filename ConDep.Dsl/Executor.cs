using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
    public static class Executor
    {
        public static void Execute(Action<DeploymentOptions> depOptions)
        {
            var executor = new InternalExecutor();
            executor.SetupInternal(depOptions);
        }

        private class InternalExecutor : ConDepOperation
        {
            public void SetupInternal(Action<DeploymentOptions> depOptions)
            {
                Setup(depOptions);
            }

            protected override void OnMessage(object sender, WebDeployMessageEventArgs e)
            {
                throw new NotImplementedException();
            }

            protected override void OnErrorMessage(object sender, WebDeployMessageEventArgs e)
            {
                throw new NotImplementedException();
            }
        }
    }

    
}