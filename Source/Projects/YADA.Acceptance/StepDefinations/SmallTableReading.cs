using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;
using YADA.Acceptance.StepDefinations.Values;

namespace YADA.Acceptance.StepDefinations
{
    [Binding]
    internal class SmallTableReading : BaseRunner
    {
        private IList<int> _executionTimes;
        private TimeSpan ExecutionTime { get; set; }

        private IList<int> ExecutionTimes
        {
            get { return _executionTimes ?? (_executionTimes = new List<int>()); }
            set { _executionTimes = value; }
        }

        public double AverageExecutionTime
        {
            get
            {
                return ExecutionTimes
                    .Average();
            }
        }

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
            AverageExecutionTime.Should().BeLessThan(milliseconds);
        }

        [When(@"using a store procedure to read a record")]
        public void WhenUsingAStoreProcedureToReadARecord()
        {
            for (var i = 0; i < 100; i++)
            {
                var stopWatch = Stopwatch.StartNew();

                var item = Reader<NarrowSmallData>.GetRecord("YadaTesting.dbo.GetNarrowSmallDataByID", 1);

                stopWatch.Stop();

                ExecutionTime = stopWatch.Elapsed;

                Console.WriteLine("Time for read {0} MS", ExecutionTime.Milliseconds);

                ExecutionTimes.Add(ExecutionTime.Milliseconds);
            }

            Console.WriteLine("Average Read Time for read {0} MS", AverageExecutionTime);

            //item.TableKey.Should().Be(1);
            //item.TestValue1.Should().Be("WhatIsOurTopic");
            //item.TestValue2.Should().Be("RellectionAndTheBartletPyshcos");
            //item.DateAdded.Should().BeBefore(DateTime.Now);
            //item.DateAdded.Should().BeAfter(DateTime.Now.AddDays(-1));
        }
    }
}