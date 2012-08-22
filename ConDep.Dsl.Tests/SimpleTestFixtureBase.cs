using System;
using NUnit.Framework;

namespace ConDep.Dsl.Tests
{
	[TestFixture]
	public abstract class SimpleTestFixtureBase : IDisposable
	{
		private bool _caughtAccessed;
		private Exception _caught;

		public Exception Caught
		{
			get
			{
				_caughtAccessed = true;
				return _caught;
			}
		}

		protected virtual void Before() { }
		protected abstract void Given();

		protected abstract void When();

		protected SimpleTestFixtureBase()
		{
			_caught = null;

			Before();
			Given();

			try
			{
				When();
			}
			catch (Exception e)
			{
				_caught = e;
			}

		}

		protected virtual void After() { }

		public void Dispose()
		{
			After();

			if (_caught != null && !_caughtAccessed)
			{
				throw _caught;
			}

		}
	}
}