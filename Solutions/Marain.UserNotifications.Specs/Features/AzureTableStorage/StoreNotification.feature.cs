﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.8.0.0
//      SpecFlow Generator Version:3.8.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Marain.UserNotifications.Specs.Features.AzureTableStorage
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.8.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Store Notification")]
    [NUnit.Framework.CategoryAttribute("perScenarioContainer")]
    [NUnit.Framework.CategoryAttribute("withUserNotificationTableStorage")]
    public partial class StoreNotificationFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "perScenarioContainer",
                "withUserNotificationTableStorage"};
        
#line 1 "StoreNotification.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features/AzureTableStorage", "Store Notification", null, ProgrammingLanguage.CSharp, new string[] {
                        "perScenarioContainer",
                        "withUserNotificationTableStorage"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
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
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Store a new user notification")]
        public virtual void StoreANewUserNotification()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Store a new user notification", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 6
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                            "NotificationType",
                            "UserId",
                            "Timestamp",
                            "PropertiesJson",
                            "CorrelationIds"});
                table4.AddRow(new string[] {
                            "marain.test.notification.v1",
                            "097C13C5-BF37-4C1F-9170-819BFC8733BC",
                            "2012-03-19T07:22Z",
                            "{ \"prop1\": \"val1\" }",
                            "[\"id1\", \"id2\"]"});
#line 7
 testRunner.Given("I have a user notification called \'input\'", ((string)(null)), table4, "Given ");
#line hidden
#line 10
 testRunner.When("I tell the user notification store to store the user notification called \'input\' " +
                        "and call the result \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 11
 testRunner.Then("no exception should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 12
 testRunner.And("the properties of the user notification called \'result\' should match the user not" +
                        "ification called \'input\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 13
 testRunner.And("the Id of the user notification called \'result\' should be set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Attempting to store the same notification twice throws a concurrency exception")]
        public virtual void AttemptingToStoreTheSameNotificationTwiceThrowsAConcurrencyException()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Attempting to store the same notification twice throws a concurrency exception", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 15
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                            "NotificationType",
                            "UserId",
                            "Timestamp",
                            "PropertiesJson",
                            "CorrelationIds"});
                table5.AddRow(new string[] {
                            "marain.test.notification.v1",
                            "93B6A389-A40F-4807-B6EE-AC41F44A3CCD",
                            "2012-03-19T07:22Z",
                            "{ \"prop1\": \"val1\" }",
                            "[\"id1\", \"id2\"]"});
#line 16
 testRunner.Given("I have a user notification called \'input\'", ((string)(null)), table5, "Given ");
#line hidden
#line 19
 testRunner.And("I have told the user notification store to store the user notification called \'in" +
                        "put\' and call the result \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 20
 testRunner.When("I tell the user notification store to store the user notification called \'input\' " +
                        "and call the result \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
 testRunner.Then("a \'UserNotificationStoreConcurrencyException\' should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Attempting to store equivalent notifications throws a concurrency exception")]
        public virtual void AttemptingToStoreEquivalentNotificationsThrowsAConcurrencyException()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Attempting to store equivalent notifications throws a concurrency exception", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 23
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                            "Name",
                            "NotificationType",
                            "UserId",
                            "Timestamp",
                            "PropertiesJson",
                            "CorrelationIds"});
                table6.AddRow(new string[] {
                            "input1",
                            "marain.test.notification.v1",
                            "F88677CE-8A9B-41FC-9E75-A8F71C9470C8",
                            "2012-03-19T07:22Z",
                            "{ \"prop1\": \"val1\" }",
                            "[\"id1\", \"id2\"]"});
                table6.AddRow(new string[] {
                            "input2",
                            "marain.test.notification.v1",
                            "F88677CE-8A9B-41FC-9E75-A8F71C9470C8",
                            "2012-03-19T07:22Z",
                            "{ \"prop1\": \"val1\" }",
                            "[\"id3\", \"id4\"]"});
#line 24
 testRunner.Given("I have user notifications", ((string)(null)), table6, "Given ");
#line hidden
#line 28
 testRunner.And("I have told the user notification store to store the user notification called \'in" +
                        "put1\' and call the result \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 29
 testRunner.When("I tell the user notification store to store the user notification called \'input2\'" +
                        " and call the result \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 30
 testRunner.Then("a \'UserNotificationStoreConcurrencyException\' should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Attempting to update a notification that doesn\'t exist throws a not found excepti" +
            "on")]
        public virtual void AttemptingToUpdateANotificationThatDoesntExistThrowsANotFoundException()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Attempting to update a notification that doesn\'t exist throws a not found excepti" +
                    "on", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 32
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                            "NotificationType",
                            "UserId",
                            "Timestamp",
                            "PropertiesJson",
                            "CorrelationIds"});
                table7.AddRow(new string[] {
                            "marain.test.notification.v1",
                            "93B6A389-A40F-4807-B6EE-AC41F44A3CCD",
                            "2012-03-19T07:22Z",
                            "{ \"prop1\": \"val1\" }",
                            "[\"id1\", \"id2\"]"});
#line 33
 testRunner.Given("I have a user notification called \'input\'", ((string)(null)), table7, "Given ");
#line hidden
#line 36
 testRunner.And("the user notification called \'input\' has the etag W/\"datetime\'2020-09-14T09%3A12%" +
                        "3A05.413Z\'\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 37
 testRunner.When("I tell the user notification store to store the user notification called \'input\' " +
                        "and call the result \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 38
 testRunner.Then("a \'UserNotificationNotFoundException\' should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Attempting to update a notification that has been modified elsewhere throws a con" +
            "currency exception")]
        public virtual void AttemptingToUpdateANotificationThatHasBeenModifiedElsewhereThrowsAConcurrencyException()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Attempting to update a notification that has been modified elsewhere throws a con" +
                    "currency exception", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 40
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                            "NotificationType",
                            "UserId",
                            "Timestamp",
                            "PropertiesJson",
                            "CorrelationIds"});
                table8.AddRow(new string[] {
                            "marain.test.notification.v1",
                            "93B6A389-A40F-4807-B6EE-AC41F44A3CCD",
                            "2012-03-19T07:22Z",
                            "{ \"prop1\": \"val1\" }",
                            "[\"id1\", \"id2\"]"});
#line 41
 testRunner.Given("I have a user notification called \'input\'", ((string)(null)), table8, "Given ");
#line hidden
#line 44
 testRunner.And("I have told the user notification store to store the user notification called \'in" +
                        "put\' and call the result \'originalResult\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 45
 testRunner.And("I update the delivery status of the notification called \'originalResult\' to \'Unde" +
                        "livered\' for the delivery channel \'api\' and call it \'updated1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 46
 testRunner.And("I have told the user notification store to store the user notification called \'up" +
                        "dated1\' and call the result \'updated1Result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 47
 testRunner.And("I update the delivery status of the notification called \'originalResult\' to \'Deli" +
                        "vered\' for the delivery channel \'api\' and call it \'updated2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 48
 testRunner.When("I tell the user notification store to store the user notification called \'updated" +
                        "2\' and call the result \'updated2Result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 49
 testRunner.Then("a \'UserNotificationStoreConcurrencyException\' should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
