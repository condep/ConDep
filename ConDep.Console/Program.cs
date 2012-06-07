using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConDep.Console
{
    class Program
    {
        //Split between Infrastructure and Deployment?

        //Blue/Green deployment

        //ConDep.exe MyAssembly.dll Env=Test [Server=web01] [Applications=Selvbetjent]
        //
        //If only assembly and Env is provided, then ConDep will deploy all applications to all servers utilizing load balancer if provided
        //If assembly, Env and Server is provided, then ConDep will take Server offline from Load Balancer if provided and deploy all applications
        //If assembly, Env, Server and Application is provided, then ConDep will take Server offline from Load Balancer if provided and deploy only the Applications specified
        static void Main(string[] args)
        {

        }
    }
}
