using System;

namespace ConDep.WebDeploy.Dsl.SemanticModel
{
	public class WebDeployDefinition
	{
		private readonly Source _source = new Source();
		private readonly Destination _destination = new Destination();
		private readonly Configuration _configuration = new Configuration();

		public Source Source
		{
			get { return _source; }
		}

		public Destination Destination
		{
			get { return _destination; }
		}

		public Configuration Configuration
		{
			get { return _configuration; }
		}
	}
}