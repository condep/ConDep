Feature: FowerShell Provider
	In order to execute powershell commands and scripts from WebDeploy
	As a PowerShell fanatic
	I want all my powershell commands and scripts to execute as expected

Scenario: Provider should fail with exit code > 0
	Given the WebDeploy Agent Service is running
	And I have provided the powershell command Exit 1 to the powershell provider
	When I execute my DSL
	Then an exception should occour
