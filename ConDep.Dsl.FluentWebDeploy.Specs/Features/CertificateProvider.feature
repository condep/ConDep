Feature: Certificate Provider
	In order to install certificates
	As a DSL user
	I want to install certificates using webdeploy DSL

@package
Scenario: Deploy certificate from package
	Given I am using the Certificate provider
	When I deploy from package
	Then I would expect the certificate with thumbprint 6bc83fd84c0f1f90e776d86af6230d44e6ea0acb to be found in the cert store

Scenario: Deploy certificate with bogus thumbprint
	Given the WebDeploy Agent Service is running 
	And I am using the Certificate provider
	And I have entered the certificate thumbprint bogus
	When I execute my DSL
	Then an exception should occour
