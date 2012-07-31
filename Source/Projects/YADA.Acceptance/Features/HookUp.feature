Feature: Hooking up to needed resources
    In order to run these tests
    As a developer
    I want to be able to 'hook up' with various resources

Scenario: Connect to a database
    Given I have a connection string configured
    When I attempt to connect to the database
    Then I can connect to the database

Scenario: There is an adventure works database installed
    Given I have a connection string configured
    When I attempt to read from an adventure works table
    Then I can get results from the database