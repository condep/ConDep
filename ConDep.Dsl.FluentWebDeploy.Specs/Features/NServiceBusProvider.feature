Feature: Deploy NServiceBus projects
	In order to deploy NServiceBus projects
	As a continuous delivering developer
	I want to easily deploy my NServiceBus project

Scenario: Deploy NServiceBus project
	Given I have fluently described how to deploy the NServiceBus project
	When I create an instance of my class
	Then the NServicebus project should successfully deploy
