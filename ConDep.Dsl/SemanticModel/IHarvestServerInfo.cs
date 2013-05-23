using ConDep.Dsl.Config;

namespace ConDep.Dsl.SemanticModel
{
    internal interface IHarvestServerInfo
    {
        void Harvest(ServerConfig server);
    }
}