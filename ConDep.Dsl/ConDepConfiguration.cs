using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ConDep.Dsl
{
	public abstract class ConDepConfiguration
	{
		protected ConDepConfiguration()
		{
			SetDefaultValues();
		}

		private void SetDefaultValues()
		{
			foreach (var property in GetType().GetProperties())
			{
			    if (!property.IsDefined(typeof (DefaultValueAttribute), false)) continue;
			    
                var attrib = property.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;
			    property.SetValue(this, attrib.Value, null);
			}
		}

		//private dynamic _environment;

		//public TEnvironment GetEnvironment<TEnvironment>() where TEnvironment : ConDepConfigEnvironment, new()
		//{
		//   return _environment ?? (_environment = new TEnvironment()); 
		//}
	}

	//public abstract class ConDepConfiguration : BasicConfiguration 
	//{
	//   //private TEnvironment _environment;

	//   //public TEnvironment Environment 
	//   //{ 
	//   //   get { return _environment ?? (_environment = new TEnvironment()); }
	//   //}
	//}

	public abstract class ConDepConfigEnvironment<TSetting> where TSetting : ConDepConfiguration, new()
	{
		private TSetting _settings;
		public abstract string Name { get; }
		public abstract IList<string> Servers { get; }
		public TSetting Settings
		{
			get { return _settings ?? (_settings = new TSetting()); }
		}
		public abstract void InitializeSettings(TSetting settings);
	}
}