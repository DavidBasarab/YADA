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
    Then the operation should happen in less than 3 ms

@database
Scenario: I want to read in 15 rows out of 1000
    Given I have small table populated with 1000 rows
    When using a store procedure to read in 15 records
    Then the operation should happen in less than 3 ms

@database
Scenario: I want to read in 150 rows out of 1000
    Given I have small table populated with 1000 rows
    When using a store procedure to read in 150 records
    Then the operation should happen in less than 3 ms

@database
Scenario: I want to read in 1500 rows out of 10000
    Given I have small table populated with 10000 rows
    When using a store procedure to read in 1500 records
    Then the operation should happen in less than 16 ms