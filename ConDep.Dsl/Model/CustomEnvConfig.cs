using System.Collections.Generic;
using System.Linq;

namespace ConDep.Dsl
{
    //public class CustomEnvConfig
    //{
    //    private IDictionary<string, string> _internalConfig;

    //    public CustomEnvConfig(IDictionary<string, string> dictionary)
    //    {
    //        _internalConfig = dictionary;
    //    }

    //    public string this[string key]
    //    {
    //        get { return _internalConfig[key]; }
    //        set { _internalConfig[key] = value; }
    //    }

    //    public IDictionary<string,string> GetForProvider(IProvide provider)
    //    {
    //        return _internalConfig.Where(x => x.Key.StartsWith(provider.GetType().Name)).ToDictionary(x => x.Key, y => y.Value);
    //    }
    //}
}