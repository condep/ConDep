using System;

namespace ConDep.Dsl
{
    public interface IOfferRemoteDeployment
    {
        /// <summary>
        /// Will deploy local source directory to remote destination directory. This operation does dot just copy directory content, but synchronize the the source folder to the destination. If a file already exist on destination, it will be updated to match source file. If a file exist on destination, but not in source directory, it will be removed from destination. If a file is readonly on destination, but read/write in source, destination file will be updated with read/write.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <returns></returns>
        IOfferRemoteDeployment Directory(string sourceDir, string destDir);

        /// <summary>
        /// Will deploy local file and its attributes to remote server.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        /// <returns></returns>
        IOfferRemoteDeployment File(string sourceFile, string destFile);

        /// <summary>
        /// Works exactly as the Directory operation, except it will mark the directory as a Web Application on remote server.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="webAppName"></param>
        /// <param name="webSiteName"></param>
        /// <returns></returns>
        IOfferRemoteDeployment IisWebApplication(string sourceDir, string webAppName, string webSiteName);

        /// <summary>
        /// Will deploy and start provided Windows Service to remote server.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="relativeExePath"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        IOfferRemoteDeployment WindowsService(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath);

        /// <summary>
        /// Will deploy and start provided Windows Service to remote server with provided options.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="relativeExePath"></param>
        /// <param name="displayName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferRemoteDeployment WindowsService(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, Action<IOfferWindowsServiceOptions> options);

        /// <summary>
        /// Will deploy and start provided Windows Service using the built-in installer.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="relativeExePath"></param>
        /// <param name="displayName"></param>
        /// <param name="installerParams"></param>
        /// <returns></returns>
        IOfferRemoteDeployment WindowsServiceWithInstaller(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, string installerParams);

        /// <summary>
        /// Will deploy and start provided Windows Service with provided options using the built-in installer.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="relativeExePath"></param>
        /// <param name="displayName"></param>
        /// <param name="installerParams"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferRemoteDeployment WindowsServiceWithInstaller(string serviceName, string displayName, string sourceDir, string destDir, string relativeExePath, string installerParams, Action<IOfferWindowsServiceOptions> options);

        /// <summary>
        /// Exactly the same as the WindowsService operation, only tailored for NServiceBus.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="serviceName"></param>
        /// <param name="profile"> </param>
        /// <returns></returns>
        IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, string profile);

        /// <summary>
        /// Exactly the same as the WindowsService operation, only tailored for NServiceBus.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="serviceName"></param>
        /// <param name="profile"> </param>
        /// <param name="options"></param>
        /// <returns></returns>
        IOfferRemoteDeployment NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, string profile, Action<IOfferWindowsServiceOptions> options);

        /// <summary>
        /// Provide operations for deploying SSL certificates to remote server.
        /// </summary>
        IOfferRemoteCertDeployment SslCertificate { get; }
    }
}