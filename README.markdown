About ConDep
============

ConDep is short for Continuous Deployment, but it really does allow for much more than just that. It's a very powerful DSL (Domain Specific Language) written in C# allowing you to define deployment and server configuration in the simplest way possible. In short it support Continuous Deployment, Continuous Delivery and Infrastructure as Code. ConDep can locally and remotely:
* Configure IIS (Application Pools, Web Sites, Web Applications and more)
* Deploy files and directories
* Copy certificates
* Deploy NServiceBus projects
* Execute remote PowerShell commands without enabling PowerShell Remoting
* Execute Dos commands
* Configure ACLs remotely
* Precompile .NET Web Applications
* Load balance with Application Request Routing (support for other LB's are coming)

In addition its built with [extensibillity in mind](wiki/Code-concepts-for-extending-ConDep) and you can easily extend it with your own Operations or Providers. And also, not a single prerequisite is needed on the remote server. For now it's only tested with Windows Server 2008 R2 and Windows 7, but probably also work with earlier versions of Windows Server.