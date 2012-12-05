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
        public void BringOffline(string serverName, LoadBalancerSuspendMethod suspendMethod, IReportStatus status)
        {
            var client = new OperationManagerClient("OperationManagerPort");
            SessionToken token = null;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                var loginCredentials = new login {user = "104170-ace", password = "noldus,2000"};
                var loginResponse = client.login(loginCredentials);

                token = loginResponse.SessionToken;

                var deviceIdsRequest = new listDeviceIds
                                           {
                                               sessionToken = loginResponse.SessionToken,
                                               deviceType = DeviceType.VIRTUAL_CONTEXT
                                           };
                var deviceIds = client.listDeviceIds(deviceIdsRequest);
                var deviceId = deviceIds.DeviceIDs.Single(x => x.chassisIPAddr == "217.173.255.1");

                var rServerRequest = new listServerfarmRservers
                                         {deviceID = deviceId, serverfarmname = "test_env_FARM", sessionToken = token};

                var rServers = client.listServerfarmRservers(rServerRequest);
                var rServer = rServers.SfRservers.Single(x => x.realserverName == "z63os2swb01-t");

                var suspend = new suspendServerfarmRserver
                                  {
                                      deviceID = deviceId,
                                      reason = "ConDep deployment",
                                      sessionToken = loginResponse.SessionToken,
                                      suspendState = SuspendState.Suspend,
                                      rserver = rServer
                                  };


                client.suspendServerfarmRserver(suspend);
            }
            catch(Exception ex)
            {
                var message = ex.Message;
            }
            finally
            {
                var logout = new logout {sessionToken = token};
                client.logout(logout);
            }
        }

        public void BringOnline(string serverName, IReportStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
