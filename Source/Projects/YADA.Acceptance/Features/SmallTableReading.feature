Feature: Small table reading
    In order to read a records from a small table
    As a developer
    I want to use YADA in the most simple way to read items out of the database and populate value objects

Background: 
    Given I can connect to AdventureWorks database

Scenario: I want to read 1 row
    When using a store procedure to read a record
    Then the operation should happen in less than 3 ms

Scenario: I want to read in 40 rows out of table between 500 to 1000 rows
    Given I have a table populated with between 500 to 1000 rows
    When using a store procedure (Sales.SmallRowTest) to read the records
    Then the operation should happen in less than 3 ms

Scenario: I want to read in 150 rows of table between 10000 to 20000 rows
    Given I have a table populated with between 10000 to 20000 rows
    When using a store procedure (Sales.MediumRowTest) to read the records
    Then the operation should happen in less than 3 ms

Scenario: I want to read in 1500 rows over 100000 rows
    Given I have a table populated with over 100000 rows
    When using a store procedure (Sales.LargeRowTest) to read the records
    Then the operation should happen in less than 16 ms