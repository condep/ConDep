Feature: FowerShell Provider
	In order to execute powershell commands from WebDeploy
	As a PowerShell fanatic
	I want all my powershell commands to execute as expected

Scenario: Provider should fail with exit code > 0
	Given the WebDeploy Agent Service is running
	And I am using the PowerShell provider
	And I have entered the command Exit 1 
	When I execute my DSL
	Then an exception should occour

Scenario: Execute Get-Date PowerShell command
	Given the WebDeploy Agent Service is running
	And I am using the PowerShell provider
	And I have entered the command Get-Date
	When I execute my DSL
	Then I would expect no errors
