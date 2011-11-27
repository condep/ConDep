using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConDep.WebDeploy.Dsl.Tests
{
	public abstract class SimpleTestFixture<THandler>
	{
		private readonly List<object> _handlers = new List<object>();
		protected THandler Handler { get; set; }

		protected virtual void Before()
		{
		}

		protected virtual void Given()
		{
		}

		protected abstract void When();

		//protected void ApplyEvent<TEvent>(TEvent e) where TEvent : IEvent
		//{
		//   _handlers.CallOnEach<IHandler<TEvent>>(h => h.Handle(e));
		//}

		public void SetFixture()
		{
			//var fixture = new Fixture().Customize(new AutoMoqCustomization());
			//fixture.Inject<IKeyValueStore>(RavenDbStore);

			//Assembly.GetAssembly(typeof(IHandler)).GetExportedTypes()
			//   .Where(t => typeof(IHandler).IsAssignableFrom(t) && t.IsClass)
			//   .Each(t =>
			//            {
			//               var handler = new SpecimenContext(fixture.Compose()).Resolve(new SeededRequest(t, null));
			//               _handlers.Add(handler);
			//            });

			//Handler = fixture.CreateAnonymous<THandler>();

			//Before();
			//Given();
			//When();
		}
	}
}