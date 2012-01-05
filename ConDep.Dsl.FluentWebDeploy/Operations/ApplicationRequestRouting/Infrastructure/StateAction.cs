namespace ConDep.Dsl.FluentWebDeploy.PreSyncOperations.ApplicationRequestRouting.Infrastructure
{
	public enum StateAction
	{
		Unknown,
		MakeServerUnavailableGracefully,
		MakeServerUnavailable,
		DisallowNewConnections,
		MakeServerAvailable,
	}
}