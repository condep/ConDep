{
    LoadBalancer    : 
    {
        Name              : "z63os2snlb01-t",
        Provider          : "ConDep.Dsl.Operations.ApplicationRequestRouting",
        DeploymentScheeme : "Sequential, Last, Half"
    },
    Servers         :
    [
        {
            Name        : "localhost"
        }
    ]
}