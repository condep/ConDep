using System;
using ConDep.PowerShell.ApplicationRequestRouting.Infrastructure;

namespace ConDep.PowerShell.ApplicationRequestRouting
{
    public class FarmServerParamHandler
    {
        private readonly string _serverName;
        private readonly string _farmName;
        private readonly bool _useDnsLookup;
        private readonly bool _noParams;

        public FarmServerParamHandler(string serverName, string farmName, bool useDnsLookup)
        {
            _noParams = string.IsNullOrEmpty(serverName) && string.IsNullOrEmpty(farmName) && !useDnsLookup;

            _serverName = serverName;
            _farmName = farmName;
            _useDnsLookup = useDnsLookup;
        }

        private bool HasServerName()
        {
            return !string.IsNullOrEmpty(_serverName);
        }

        private bool HasFarmName()
        {
            return !string.IsNullOrEmpty(_farmName);
        }

        public WebFarmManager GetWebFarmManager()
        {
            if(_noParams)
            {
                return new WebFarmManager();
            }
            if(HasNameOrFarm())
            {
                return new WebFarmManager(_farmName, _serverName, _useDnsLookup);
            }

            throw new Exception("Params");
        }

        private bool HasNameOrFarm()
        {
            return HasServerName() || HasFarmName();
        }
    }
}