using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using ConDep.Dsl.Config;
using ConDep.Dsl.LoadBalancer.Ace.Proxy;
using ConDep.Dsl.Logging;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using System.Linq;

namespace ConDep.Dsl.LoadBalancer.Ace
{
    public class AceLoadBalancer_Anm_v41 : ILoadBalance
    {
        private readonly string _username;
        private readonly string _password;
        private IOperationManager _proxy;
        private bool _loggedIn;

        //"test_env_FARM"
        public AceLoadBalancer_Anm_v41(LoadBalancerConfig lbConfig)
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            var remoteAddress = new EndpointAddress(lbConfig.Name);

            _proxy = new OperationManagerClient(binding, remoteAddress);
            _username = lbConfig.UserName;
            _password = lbConfig.Password;

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }

        public void BringOffline(string serverName, string farm, LoadBalancerSuspendMethod suspendMethod, IReportStatus status)
        {
            SessionToken token = null;
            try
            {
                token = LogIn();
                var deviceIds = GetDeviceIds(token);
                DeviceID deviceId;
                var server = GetServer(serverName, farm, token, deviceIds, out deviceId);

                var suspend = new suspendServerfarmRserver
                {
                    deviceID = deviceId,
                    reason = "ConDep deployment",
                    sessionToken = token,
                    suspendState = GetSuspendState(suspendMethod),
                    rserver = server
                };

                _proxy.suspendServerfarmRserver(new suspendServerfarmRserverRequest { suspendServerfarmRserver = suspend});
            }
            finally
            {
                if (_loggedIn)
                {
                    LogOut(token);
                }
            }

        }

        private void LogOut(SessionToken token)
        {
            _proxy.logout(new logoutRequest{ logout = new logout {sessionToken = token}});
        }

        public void BringOnline(string serverName, string farm, IReportStatus status)
        {
            SessionToken token = null;
            try
            {
                token = LogIn();
                var deviceIds = GetDeviceIds(token);
                DeviceID deviceId;
                var server = GetServer(serverName, farm, token, deviceIds, out deviceId);

                var activate = new activateServerfarmRserver
                                   {
                                       deviceID = deviceId,
                                       reason = "ConDep deployment",
                                       sessionToken = token,
                                       rserver = server
                                   };
                _proxy.activateServerfarmRserver(new activateServerfarmRserverRequest { activateServerfarmRserver = activate});
            }
            finally
            {
                if (_loggedIn)
                {
                    _proxy.logout(new logoutRequest { logout = new logout { sessionToken = token } });
                }
            }
        }

        public LbMode Mode { get; set; }

        private SessionToken LogIn()
        {
            var loginCredentials = new login {user = _username, password = _password};
            var loginResponse = _proxy.login(new loginRequest { login = loginCredentials});
            _loggedIn = true;
            return loginResponse.loginResponse.SessionToken;
        }

        private SfRserver GetServer(string serverName, string farm, SessionToken token, IEnumerable<DeviceID> deviceIds, out DeviceID deviceId)
        {
            deviceId = null;
            SfRserver sfRServer = null;
            foreach (var currentDeviceId in deviceIds)
            {
                try
                {
                    var rServerRequest = new listServerfarmRservers { deviceID = currentDeviceId, serverfarmname = farm, sessionToken = token };
                    var rServers = _proxy.listServerfarmRservers(new listServerfarmRserversRequest { listServerfarmRservers = rServerRequest });
                    sfRServer = rServers.listServerfarmRserversResponse.SfRservers.Single(x => x.realserverName.ToLower() == serverName.ToLower());
                    deviceId = currentDeviceId;
                }
                catch(FaultException<WSException> aceEx)
                {
                    Logger.Verbose("Web Service Fault: {0}", aceEx.Message);
                    Logger.Verbose("Since this device [{0}] faulted, ConDep will try next device.", currentDeviceId.name);
                }
            }
            if(sfRServer == null)
            {
                throw new ConDepLoadBalancerException("Unable to get real server from load balancer. Use verbose logging for more details.");
            }
            return sfRServer;
        }

        private IEnumerable<DeviceID> GetDeviceIds(SessionToken token)
        {
            var deviceIdsRequest = new listDeviceIds
            {
                sessionToken = token,
                deviceType = DeviceType.VIRTUAL_CONTEXT
            };
            var deviceIds = _proxy.listDeviceIds(new listDeviceIdsRequest { listDeviceIds = deviceIdsRequest});
            return deviceIds.listDeviceIdsResponse.DeviceIDs;

        }

        private SuspendState GetSuspendState(LoadBalancerSuspendMethod suspendMethod)
        {
            switch(suspendMethod)
            {
                case LoadBalancerSuspendMethod.Graceful:
                    return SuspendState.Graceful;
                case LoadBalancerSuspendMethod.Suspend:
                    return SuspendState.Suspend;
                case LoadBalancerSuspendMethod.SuspendClearConnections:
                    return SuspendState.Suspend_Clear_Connections;
                default:
                    throw new ConDepLoadBalancerException("Suspend method not supported");
            }
        }
    }
}
