using System;
using System.Net;
using ConDep.Dsl.LoadBalancer.Ace.Proxy;
using ConDep.Dsl.Operations.LoadBalancer;
using ConDep.Dsl.SemanticModel;
using System.Linq;

namespace ConDep.Dsl.LoadBalancer.Ace
{
    public class AceLoadBalancer_Anm_v41 : ILoadBalance
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _chassisIp;
        private IOperationManager _proxy;
        private bool _loggedIn;

        //"test_env_FARM"
        public AceLoadBalancer_Anm_v41(string username, string password, string chassisIp = null)
        {
            _proxy = new OperationManagerClient("OperationManagerPort");
            _username = username;
            _password = password;
            _chassisIp = chassisIp;

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }

        public void BringOffline(string serverName, string farm, LoadBalancerSuspendMethod suspendMethod, IReportStatus status)
        {
            SessionToken token = null;
            try
            {
                token = LogIn();
                var deviceId = GetDeviceId(token);
                var server = GetServer(serverName, farm, token, deviceId);

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
                var deviceId = GetDeviceId(token);
                var server = GetServer(serverName, farm, token, deviceId);

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

        private SessionToken LogIn()
        {
            var loginCredentials = new login {user = _username, password = _password};
            var loginResponse = _proxy.login(new loginRequest { login = loginCredentials});
            _loggedIn = true;
            return loginResponse.loginResponse.SessionToken;
        }

        private SfRserver GetServer(string serverName, string farm, SessionToken token, DeviceID deviceId)
        {
            var rServerRequest = new listServerfarmRservers
                                     {deviceID = deviceId, serverfarmname = farm, sessionToken = token};
            var rServers = _proxy.listServerfarmRservers(new listServerfarmRserversRequest { listServerfarmRservers = rServerRequest});
            var sfRServer = rServers.listServerfarmRserversResponse.SfRservers.Single(x => x.realserverName.ToLower() == serverName.ToLower());
            return sfRServer;
        }

        private DeviceID GetDeviceId(SessionToken token)
        {
            var deviceIdsRequest = new listDeviceIds
            {
                sessionToken = token,
                deviceType = DeviceType.VIRTUAL_CONTEXT
            };
            var deviceIds = _proxy.listDeviceIds(new listDeviceIdsRequest { listDeviceIds = deviceIdsRequest});
            return string.IsNullOrWhiteSpace(_chassisIp)
                               ? deviceIds.listDeviceIdsResponse.DeviceIDs[0]
                               : deviceIds.listDeviceIdsResponse.DeviceIDs.Single(x => x.chassisIPAddr == _chassisIp);
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
                    throw new Exception("Suspend method not supported");
            }
        }
    }
}
