using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace ConDep.Dsl.Impersonation
{
    public class Impersonator : IDisposable
    {
        private readonly string _username;
        private readonly string _password;
        private readonly bool _crossDomain;
        private readonly string _domain;
        private bool _disposed = false;
        private IntPtr _tokenDuplicate;
        private WindowsImpersonationContext _impersonationContext;
        private IntPtr _token;

        public Impersonator(string username, string password, bool crossDomain)
        {
            if (username == null || password == null)
                return;

            _username = GetUserName(username);
            _domain = GetDomain(username);
            _password = password;
            _crossDomain = crossDomain;

            try
            {
                Initialize();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public static string GetUserName(string username)
        {
            var split = username.Split('\\');
            if (split.Length == 1)
            {
                return username;
            }
            if (split.Length == 2)
            {
                return split[1];
            }
            throw new NotSupportedException("Username format not supported. More than one '\\' found in username string.");
        }

        public static string GetDomain(string username)
        {
            var split = username.Split('\\');
            if (split.Length == 1)
            {
                return "";
            }
            if (split.Length == 2)
            {
                return split[0];
            }
            throw new NotSupportedException("Username format not supported. More than one '\\' found in username string.");
        }

        private void Initialize()
        {
            if (!NativeMethods.LogonUser(
                _username,
                _domain,
                _password,
                _crossDomain ? NativeMethods.LogonType.NewCredentials : NativeMethods.LogonType.NetworkCleartext,
                NativeMethods.LogonProvider.Default,
                out _token))
            {
                throw new Win32Exception();
            }

            if (!NativeMethods.DuplicateToken(
                _token,
                NativeMethods.SecurityImpersonationLevel.Impersonation,
                out _tokenDuplicate))
            {
                throw new Win32Exception();
            }

            _impersonationContext = new WindowsIdentity(_tokenDuplicate).Impersonate();
                // Do stuff with your share here.
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    if(_impersonationContext != null)
                    {
                        _impersonationContext.Undo();
                        _impersonationContext.Dispose();
                    }

                    if (_tokenDuplicate != IntPtr.Zero)
                    {
                        if (!NativeMethods.CloseHandle(_tokenDuplicate))
                        {
                            // Uncomment if you need to know this case.
                            ////throw new Win32Exception();
                        }
                    }

                    if (_token != IntPtr.Zero)
                    {
                        if (!NativeMethods.CloseHandle(_token))
                        {
                            // Uncomment if you need to know this case.
                            ////throw new Win32Exception();
                        }
                    }
                }

                _disposed = true;

            }
        }

        ~Impersonator()
        {
            Dispose(false);
        }
    }

    /// <summary>
    /// Implements P/Invoke Interop calls to the operating system.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// The type of logon operation to perform.
        /// </summary>
        internal enum LogonType : int
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively
            /// using the computer, such as a user being logged on by a
            /// terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon
            /// information for disconnected operations; therefore, it is
            /// inappropriate for some client/server applications, such as a
            /// mail server.
            /// </summary>
            Interactive = 2,

            /// <summary>
            /// This logon type is intended for high performance servers to
            /// authenticate plaintext passwords.
            /// The LogonUser function does not cache credentials for this
            /// logon type.
            /// </summary>
            Network = 3,

            /// <summary>
            /// This logon type is intended for batch servers, where processes
            /// may be executing on behalf of a user without their direct
            /// intervention.  This type is also for higher performance servers
            /// that process many plaintext authentication attempts at a time,
            /// such as mail or Web servers.
            /// The LogonUser function does not cache credentials for this
            /// logon type.
            /// </summary>
            Batch = 4,

            /// <summary>
            /// Indicates a service-type logon.  The account provided must have
            /// the service privilege enabled.
            /// </summary>
            Service = 5,

            /// <summary>
            /// This logon type is for GINA DLLs that log on users who will be
            /// interactively using the computer.
            /// This logon type can generate a unique audit record that shows
            /// when the workstation was unlocked.
            /// </summary>
            Unlock = 7,

            /// <summary>
            /// This logon type preserves the name and password in the
            /// authentication package, which allows the server to make
            /// connections to other network servers while impersonating the
            /// client.  A server can accept plaintext credentials from a
            /// client, call LogonUser, verify that the user can access the
            /// system across the network, and still communicate with other
            /// servers.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            NetworkCleartext = 8,

            /// <summary>
            /// This logon type allows the caller to clone its current token
            /// and specify new credentials for outbound connections.  The new
            /// logon session has the same local identifier but uses different
            /// credentials for other network connections.
            /// NOTE: This logon type is supported only by the
            /// LOGON32_PROVIDER_WINNT50 logon provider.
            /// NOTE: Windows NT:  This value is not supported.
            /// </summary>
            NewCredentials = 9
        }

        /// <summary>
        /// Specifies the logon provider.
        /// </summary>
        internal enum LogonProvider : int
        {
            /// <summary>
            /// Use the standard logon provider for the system.
            /// The default security provider is negotiate, unless you pass
            /// NULL for the domain name and the user name is not in UPN format.
            /// In this case, the default provider is NTLM.
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            Default = 0,

            /// <summary>
            /// Use this provider if you'll be authenticating against a Windows
            /// NT 3.51 domain controller (uses the NT 3.51 logon provider).
            /// </summary>
            WinNT35 = 1,

            /// <summary>
            /// Use the NTLM logon provider.
            /// </summary>
            WinNT40 = 2,

            /// <summary>
            /// Use the negotiate logon provider.
            /// </summary>
            WinNT50 = 3
        }

        /// <summary>
        /// The type of logon operation to perform.
        /// </summary>
        internal enum SecurityImpersonationLevel : int
        {
            /// <summary>
            /// The server process cannot obtain identification information
            /// about the client, and it cannot impersonate the client.  It is
            /// defined with no value given, and thus, by ANSI C rules,
            /// defaults to a value of zero.
            /// </summary>
            Anonymous = 0,

            /// <summary>
            /// The server process can obtain information about the client,
            /// such as security identifiers and privileges, but it cannot
            /// impersonate the client.  This is useful for servers that export
            /// their own objects, for example, database products that export
            /// tables and views.  Using the retrieved client-security
            /// information, the server can make access-validation decisions
            /// without being able to use other services that are using the
            /// client's security context.
            /// </summary>
            Identification = 1,

            /// <summary>
            /// The server process can impersonate the client's security
            /// context on its local system.  The server cannot impersonate the
            /// client on remote systems.
            /// </summary>
            Impersonation = 2,

            /// <summary>
            /// The server process can impersonate the client's security
            /// context on remote systems.
            /// NOTE: Windows NT:  This impersonation level is not supported.
            /// </summary>
            Delegation = 3
        }

        /// <summary>
        /// Logs on the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="password">The password.</param>
        /// <param name="logonType">Type of the logon.</param>
        /// <param name="logonProvider">The logon provider.</param>
        /// <param name="token">The token.</param>
        /// <returns>True if the function succeeds, false if the function fails.
        /// To get extended error information, call GetLastError.</returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LogonUser(
            string userName,
            string domain,
            string password,
            LogonType logonType,
            LogonProvider logonProvider,
            out IntPtr token);

        /// <summary>
        /// Duplicates the token.
        /// </summary>
        /// <param name="existingTokenHandle">The existing token
        /// handle.</param>
        /// <param name="securityImpersonationLevel">The security impersonation
        /// level.</param>
        /// <param name="duplicateTokenHandle">The duplicate token
        /// handle.</param>
        /// <returns>True if the function succeeds, false if the function fails.
        /// To get extended error information, call GetLastError.</returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DuplicateToken(
            IntPtr existingTokenHandle,
            SecurityImpersonationLevel securityImpersonationLevel,
            out IntPtr duplicateTokenHandle);

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns>True if the function succeeds, false if the function fails.
        /// To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr handle);
    }
}
