namespace ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure
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