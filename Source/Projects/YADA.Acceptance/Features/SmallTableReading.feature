Feature: Small table reading
    In order to read a records from a small table
    As a developer
    I want to use YADA in the most simple way to read items out of the database and populate value objects

Background: 
    Given I have a test database created
    Given I have small table created
    Given I have small table populated

@database
Scenario: I want to read 1 row
    When using a store procedure to read a record
    Then the operation should happen in less than 1000 ms