using ConDep.WebDeploy.Dsl;

namespace TestWebDeployApp
{
	public class Program : ConDepConsoleApp<Program>
	{
		static void Main(string[] args)
		{
			var program = Initialize(args);

			program.Sync(s => s.FromLocalHost()
			                  	.UsingProvider(p =>
			                  	               	{
			                  	               		p.Certificate("");
			                  	               		p.DefineCustom("providerName", "sourcePath", "destinationPath");
																	p.DefineCustom("providerName", "sourcePath", "destinationPath", cpo =>
																	                                                                	{
																	                                                                		cpo.Define("name", "value");
																	                                                                		cpo.Define("name", "value");
																	                                                                	});
			                  	               	}));


		}

	}
}
