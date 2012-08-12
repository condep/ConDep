About ConDep
============

About ConDep
============

ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows. It's main goal is to make it as simple as possible to define deployment and server configurations. ConDep can locally and remotely:
* Configure IIS (Application Pools, Web Sites, Web Applications and more)
* Deploy files and directories
* Copy certificates
* Deploy NServiceBus projects
* Execute remote PowerShell commands without enabling PowerShell Remoting
* Execute Dos commands
* Configure ACLs remotely
* Precompile .NET Web Applications
* Load balance with Application Request Routing (support for other LB's are coming)

As mentioned its built with [extensibillity in mind](https://github.com/torresdal/ConDep/wiki/Code-concepts-for-extending-ConDep) and you can easily extend it with your own Operations or Providers. And also, not a single prerequisite is needed on the remote server. For now it's only tested with Windows Server 2008 R2 and Windows 7, but probably also work with earlier and future versions of Windows Server.