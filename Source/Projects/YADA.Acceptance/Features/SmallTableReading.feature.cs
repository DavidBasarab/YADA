﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.8.1.0
//      SpecFlow Generator Version:1.8.0.0
//      Runtime Version:4.0.30319.17626
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace YADA.Acceptance.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.8.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Small table reading")]
    public partial class SmallTableReadingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SmallTableReading.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Small table reading", "  In order to read a records from a small table\r\n  As a developer\r\n  I want to us" +
                    "e YADA in the most simple way to read items out of the database and populate val" +
                    "ue objects", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line 7
    testRunner.Given("I can connect to AdventureWorks database");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to read 1 row")]
        public virtual void IWantToRead1Row()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to read 1 row", ((string[])(null)));
#line 9
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 10
    testRunner.When("using a store procedure to read a record");
#line 11
    testRunner.Then("the operation should happen in less than 3 ms");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to read in 40 rows out of table between 500 to 1000 rows")]
        public virtual void IWantToReadIn40RowsOutOfTableBetween500To1000Rows()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to read in 40 rows out of table between 500 to 1000 rows", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 14
    testRunner.Given("I have a table populated with between 500 to 1000 rows");
#line 15
    testRunner.When("using a store procedure (Sales.SmallRowTest) to read the records");
#line 16
    testRunner.Then("the operation should happen in less than 5 ms");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to read in 150 rows of table between 10000 to 20000 rows")]
        public virtual void IWantToReadIn150RowsOfTableBetween10000To20000Rows()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to read in 150 rows of table between 10000 to 20000 rows", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 19
    testRunner.Given("I have a table populated with between 10000 to 20000 rows");
#line 20
    testRunner.When("using a store procedure (Sales.MediumRowTest) to read the records");
#line 21
    testRunner.Then("the operation should happen in less than 5 ms");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I want to read in 1500 rows over 100000 rows")]
        public virtual void IWantToReadIn1500RowsOver100000Rows()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I want to read in 1500 rows over 100000 rows", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 24
    testRunner.Given("I have a table populated with over 100000 rows");
#line 25
    testRunner.When("using a store procedure (Sales.LargeRowTest) to read the records");
#line 26
    testRunner.Then("the operation should happen in less than 20 ms");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
