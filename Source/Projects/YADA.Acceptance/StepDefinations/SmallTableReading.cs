using System;
using System.Diagnostics;
using FluentAssertions;
using TechTalk.SpecFlow;
using YADA.Acceptance.StepDefinations.Values;

namespace YADA.Acceptance.StepDefinations
{
    [Binding]
    internal class SmallTableReading : BaseRunner
    {
        private TimeSpan ExecutionTime { get; set; }

        [AfterScenario("database")]
        public void CleanUp()
        {
            RunScriptAgainistDatabase(@"Scripts\RemoveYadaTestDB.sql");
        }

        [Given(@"I have a test database created")]
        public void GivenIHaveATestDatabaseCreated()
        {
            RunScriptAgainistDatabase(@"Scripts\CreateYadaTestDB.sql");
        }

        [Given(@"I have small table created")]
        public void GivenIHaveSmallTableCreated()
        {
            // Done in previous step
        }

        [Given(@"I have small table populated")]
        public void GivenIHaveSmallTablePopulated()
        {
            RunScriptAgainistDatabase(@"Scripts\InsertData.sql");
        }

        [Then(@"the operation should happen in less than (.*) ms")]
        public void ThenTheOperationShouldHappenInLessThanMS(int milliseconds)
        {
            ExecutionTime.Milliseconds.Should().BeLessThan(milliseconds);
        }


        [When(@"using a store procedure to read a record")]
        public void WhenUsingAStoreProcedureToReadARecord()
        {
            var stopWatch = Stopwatch.StartNew();

            var item = Reader<NarrowSmallData>.GetRecord("dbo.GetNarrowSmallDataByID", 1);

            stopWatch.Stop();

            ExecutionTime = stopWatch.Elapsed;

            item.TableKey.Should().Be(1);
            item.TestValue1.Should().Be("WhatIsOurTopic");
            item.TestValue2.Should().Be("RellectionAndTheBartletPyshcos");
            item.DateAdded.Should().BeBefore(DateTime.Now);
            item.DateAdded.Should().BeAfter(DateTime.Now.AddDays(-1));
        }
    }
}