Feature: Web Site
	In order to successfully deploy my web applications
	As a deployer
	I want to deploy an entire web site from one server to another

Scenario: Deploy Web Site
	Given the WebDeploy Agent Service is running 
	And I am using the WebSite provider
	When I execute my DSL
	Then I would expect no errors
