Feature: Hooking up to needed resources
	In order to run these tests
	As a developer
	I want to be able to 'hook up' with various resources

@mytag
Scenario: Connect to a database
	Given I have a connection string configured
	When I attempt to connect to the database
	Then I can connect to the database

Scenario: Have rights to drop and create databases
	Given I have a connection string configured
	When I attempt to create a database
	And I attempt to delete a database
	Then I can create the database
	And I can delete the database
