namespace ConDep.Dsl.Builders
{
    public interface IOfferIisInfrastructureOptions
    {
        /// <summary>
        /// HTTP Redirection provides support to redirect user requests to a specific destination. Use HTTP redirection whenever you want customers who might use one URL to actually end up at another URL. This is helpful in many situations, from renaming your Web site, to overcoming a domain name that is difficult to spell, or forcing clients to use the HTTPS protocol.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions HttpRedirect();

        /// <summary>
        /// WebDAV Publishing (Web Distributed Authoring and Versioning) enables you to publish files to and from a Web server by using the HTTP protocol. Because WebDAV uses HTTP, it works through most firewalls without modification.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions DavPublishing();

        /// <summary>
        /// ASP.NET provides a server side object-oriented programming environment for building Web sites and Web applications that use managed code. ASP.NET is not just a new version of ASP. ASP.NET provides a robust infrastructure for building Web applications, and it has been completely re-architected to provide a highly productive programming experience based on the .NET Framework.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions AspNet();

        /// <summary>
        /// Active Server Pages (ASP) provides a server side scripting environment for building Web sites and Web applications. ASP offers improved performance over CGI scripts by providing IIS with native support for both VBScript and JScript. Use ASP if you have existing applications that require ASP support. For new development, consider using ASP.NET.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions Asp();

        /// <summary>
        /// Common Gateway Interface (CGI) defines how a Web server passes information to an external program. Typical uses might include using a Web form to collect information and then passing that information to a CGI script to be e-mailed somewhere else. Because CGI is a standard, CGI scripts can be written by using a variety of programming languages. The downside to using CGI is the performance overhead.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions Cgi();

        /// <summary>
        /// Server Side Includes (SSI) is a scripting language that is used to generate HTML pages dynamically. The script runs on the server before the page is delivered to the client and typically involves inserting one file into another. For example, you might create an HTML navigation menu and use SSI to dynamically add it to all pages on a Web site.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions ServerSideIncludes();

        /// <summary>
        /// Logging Tools provides infrastructure to manage Web server logs and automate common logging tasks.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions LoggingTools();

        /// <summary>
        /// Tracing provides infrastructure to diagnose and troubleshoot Web applications. By using failed request tracing, you can troubleshoot difficult to capture events like poor performance or authentication-related failures. This feature buffers trace events for a request and only flushes them to disk if the request falls into a user-configured error condition.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions Tracing();

        /// <summary>
        /// Custom Logging provides support for logging Web server activity in a format that differs greatly from how IIS generates log files. Use Custom Logging to create your own logging module. Custom logging modules are added to IIS by registering a new COM component that implements ILogPlugin or ILogPluginEx.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions CustomLogging();

        /// <summary>
        /// ODBC Logging provides infrastructure that supports logging Web server activity to an ODBC-compliant database. By using a logging database, you can programmatically display and manipulate data from the logging database on an HTML page. You might do this to search logs for specific events that you want to monitor.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions OdbcLogging();

        /// <summary>
        /// Basic Authentication offers strong browser compatibility. Appropriate for small internal networks, this authentication method is rarely used on the public Internet. Its major disadvantage is that it transmits passwords across the network using an easily decrypted algorithm. If intercepted, these passwords are simple to decipher. Use SSL with Basic authentication.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions BasicAuth();

        /// <summary>
        /// Windows Authentication is a low cost authentication solution for internal Web sites. This authentication scheme allows administrators in a Windows domain to take advantage of the domain infrastructure for authenticating users. Do not use Windows authentication if users who must be authenticated access your Web site from behind firewalls and proxy servers.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions WindowsAuth();

        /// <summary>
        /// Digest Authentication works by sending a password hash to a Windows domain controller to authenticate users. When you need improved security over Basic authentication, consider using Digest authentication, especially if users who must be authenticated access your Web site from behind firewalls and proxy servers.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions DigestAuth();

        /// <summary>
        /// Client Certificate Mapping Authentication uses client certificates to authenticate users. A client certificate is a digital ID from a trusted source. IIS offers two types of authentication using client certificate mapping. This type uses Active Directory to offer one-to-one certificate mappings across multiple Web servers.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions ActiveDirectoryClientCertMappingAuth();

        /// <summary>
        /// IS Client Certificate Mapping Authentication uses client certificates to authenticate users. A client certificate is a digital ID from a trusted source. IIS offers two types of authentication using client certificate mapping. This type uses IIS to offer one-to-one or many-to-one certificate mapping, and offers better performance over Client Certificate Mapping authentication.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions IisClientCertMappingAuth();

        /// <summary>
        /// URL Authorization allows you to create rules that restrict access to Web content. You can bind these rules to users, groups, or HTTP header verbs. By configuring URL authorization rules, you can prevent users who are not members of certain groups from accessing content or interacting with Web pages.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions UrlAuth();

        /// <summary>
        /// IP and Domain Restrictions allow you to enable or deny content based on the originating IP address or domain name of the request. Instead of using groups, roles, or NTFS file system permissions to control access to content, you can specify IP addresses or domain names.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions IpSecurity();

        /// <summary>
        /// Dynamic Content Compression provides infrastructure to configure HTTP compression of dynamic content. Enabling dynamic compression always gives you more efficient use of bandwidth, but if your server's processor utilization is already very high, the CPU load imposed by dynamic compression might make your site perform more slowly.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions DynamicContentCompression();

        /// <summary>
        /// IIS Management Scripts and Tools provide infrastructure to manage an IIS 7.5 Web server programmatically by using commands in a Command Prompt window or by running scripts. You can use these tools when you want to automate commands in batch files or when you do not want to incur the overhead of managing IIS by using the graphical user interface.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions MngmntScriptsAndTools();

        /// <summary>
        /// Management Service provides infrastructure to configure the IIS 7.5 user interface, IIS Manager, for remote management in IIS 7.5.
        /// </summary>
        /// <returns></returns>
        IOfferIisInfrastructureOptions MgmtService();
    }
}