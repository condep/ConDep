namespace ConDep.Dsl
{
    public interface IOfferWindowsInfrastructureOptions
    {
        /// <summary>
        /// Install windows feature. Equalent to PowerShell's Add-WindowsFeature in Server 2008 or Install-WindowsFeature in 2012 and above.
        /// </summary>
        /// <param name="featureName">Name of the Windows feature to install</param>
        /// <returns></returns>
        IOfferWindowsInfrastructureOptions InstallFeature(string featureName);

        /// <summary>
        /// Uninstall windows feature. Equalent to PowerShell's Remove-WindowsFeature in Server 2008 or Uninstall-WindowsFeature in 2012 and above.
        /// </summary>
        /// <param name="featureName">Name of the Windows feature to uninstall</param>
        /// <returns></returns>
        IOfferWindowsInfrastructureOptions UninstallFeature(string featureName);
    }
}