Spec for synchronising files and folders between a ConDep Server and a Node
===========================================================================

For a folder with files and/or folders
--------------------------------------
1. Check if folder exist on target Node
	1.1 If not, Copy folder
	1.2 End
2. If exist
	2.1 Collect the entire Directory structure from Node in json-format
	2.2 Compare json from Node with ConDep server
		2.2.1 Return 3 json sections
			2.2.1.1 Identify directories that does not exist on Node (Need to be copied)
			2.2.1.2 Identify directories that only exist on Node (Need to be deleted)
			2.2.1.3 Identify directories that exist on both ConDep server and Node and collect:
				2.2.1.3.1 Length (bytes)
				2.2.1.3.2 LastWriteTimeUtc
				2.2.1.3.3 Attributes
					2.2.1.3.3.1 ReadOnly
					2.2.1.3.3.2 Hidden
		2.2.2 If anything in section 2.2.1.3
			2.2.2.1 Send over json with all file info
			2.2.2.2 Compare LastWriteTimeUtc for each file
			#2.2.2.3 Compute hash values for all files on ConDep server
			#2.2.2.4 Send over hashes for comparison on node
			2.2.2.3 Return file info that diff
		2.2.3 Copy all files from ConDep server to Node identified in 2.2.1.1
		2.2.4 Remove all files from Node identified in 2.2.1.2
		2.2.5 Overwrite files on Node identified in 2.2.2.3

For a single file
-----------------
1. Check if file exist on Node
	1.1 If exist, compute hash
	1.2 Return hash to ConDep server
	1.3 Compare hash with file on ConDep server
	1.4 If diff
		1.4.1 Copy file to Node
	1.5 If equal
		1.5.1 Do nothing

Considerations
--------------
1) Do we specifically need to copy attributes like read only, hidden etc?
2) Consider doing bitwise compare on FileAttributes
3) Consider supporting environment variables in file paths
4) Consider supporting wildcards for copying multiple files
5) For example structure look at xml output from WebDeploy command:
	C:\Program Files\IIS\Microsoft Web Deploy V3>msdeploy -verb:dump -source:dirpath="C:\Program Files\IIS\Application Request Routing" -xml

	<output>
	  <MSDeploy.dirPath>
	    <dirPath path="C:\Program Files\IIS\Application Request Routing" securityDescriptor="D:" parentSecurityDescriptors="" attributes="Directory">
	      <filePath path="ArrPerfCounters.man" size="16593" attributes="Archive" lastWriteTime="10/06/2009 11:07:44" securityDescriptor="D:" />
	      <filePath path="ARR_ReadMe.htm" size="8385" attributes="Archive" lastWriteTime="10/10/2009 05:13:52" securityDescriptor="D:" />
	      <dirPath path="en-us" securityDescriptor="D:" parentSecurityDescriptors="" attributes="Directory">
	        <filePath path="requestRouter.dll.mui" size="16112" attributes="Archive" lastWriteTime="11/03/2009 10:57:14" securityDescriptor="D:" />
	      </dirPath>
	      <filePath path="gzip.dll" size="72960" attributes="Archive" lastWriteTime="11/03/2009 10:57:14" securityDescriptor="D:" />
	      <filePath path="license.rtf" size="1216" attributes="Archive" lastWriteTime="10/08/2009 14:12:48" securityDescriptor="D:" />
	      <filePath path="requestRouter.dll" size="285424" attributes="Archive" lastWriteTime="11/03/2009 10:57:16" securityDescriptor="D:" />
	      <filePath path="requestrouterhelper_x64.msi" size="605696" attributes="Archive" lastWriteTime="10/30/2009 10:07:52" securityDescriptor="D:" />
	      <filePath path="requestrouterhelper_x86.msi" size="562176" attributes="Archive" lastWriteTime="10/30/2009 10:07:52" securityDescriptor="D:" />
	      <filePath path="requestRouterRSCA.dll" size="107760" attributes="Archive" lastWriteTime="11/03/2009 10:57:16" securityDescriptor="D:" />
	      <filePath path="scavenge.exe" size="71920" attributes="Archive" lastWriteTime="11/03/2009 10:57:16" securityDescriptor="D:" />
	    </dirPath>
	  </MSDeploy.dirPath>
	</output>