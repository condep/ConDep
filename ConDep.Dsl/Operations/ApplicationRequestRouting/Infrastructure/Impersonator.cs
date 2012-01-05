using System;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ConDep.Dsl.Operations.ApplicationRequestRouting.Infrastructure
{
	public class Impersonator : IDisposable
	{
		private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
		private WindowsImpersonationContext _impersonationContext;

		public Impersonator(UserInfo user)
		{
			ImpersonateValidUser(user);
		}

		private void ImpersonateValidUser(UserInfo user)
		{
			IntPtr adminToken = IntPtr.Zero;
			WindowsIdentity windowsIdentityToken;

			try
			{
				if (RevertToSelf())
				{
					if (LogonUser(user.Username, user.Domain, user.Password, LOGON32_LOGON_NEW_CREDENTIALS, 0, ref adminToken) != 0)
					{
						windowsIdentityToken = new WindowsIdentity(adminToken);
						_impersonationContext = windowsIdentityToken.Impersonate();
					}
					else
					{
						throw new Win32Exception( Marshal.GetLastWin32Error() );
					}
				}
				else
				{
					throw new Win32Exception( Marshal.GetLastWin32Error() );
				}
			}
			finally
			{
				if (adminToken != IntPtr.Zero)
				{
					CloseHandle(adminToken);
				}
			}
		}

		private void UndoImpersonation()
		{
			if ( _impersonationContext!=null )
			{
				_impersonationContext.Undo();
			}	
		}

		public void Dispose()
		{
			UndoImpersonation();
		}

		[DllImport("advapi32.dll", SetLastError=true)]
		private static extern int LogonUser(
			string lpszUserName,
			string lpszDomain,
			string lpszPassword,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken);

		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern bool RevertToSelf();

		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		private static extern  bool CloseHandle(
			IntPtr handle);
	}
}