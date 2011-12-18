Feature: Run Command
	In order to execute dos commands
	As a DSL user
	I want to execute dos commands using webdeploy DSL

Scenario: Retreiving date successfully
	Given the WebDeploy Agent Service is running 
	And I have entered the command date /t
	When I execute my Run Command
	Then I would expect no errors

Scenario: Exit codes greater than 0 should trigger exceptions
	Given the WebDeploy Agent Service is running 
	And I have entered the command bogus
	When I execute my Run Command
	Then I would expect an exit code error
