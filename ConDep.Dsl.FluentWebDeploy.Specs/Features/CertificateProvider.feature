Feature: Certificate Provider
	In order to install certificates
	As a DSL user
	I want to install certificates using webdeploy DSL

Scenario: Deploy certificate from package
	Given the WebDeploy Agent Service is running 
	And I am using the Certificate provider
	And I have entered the certificate thumbprint 6b c8 3f d8 4c 0f 1f 90 e7 76 d8 6a f6 23 0d 44 e6 ea 0a cb
	When I execute my DSL
	Then I would expect no errors

Scenario: Deploy invalid certificate
	Given the WebDeploy Agent Service is running 
	And I am using the Certificate provider
	And I have entered the certificate thumbprint bogus
	When I execute my DSL
	Then an exception should occour
