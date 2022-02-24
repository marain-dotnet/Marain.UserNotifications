﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Get Notifications")]
    [NUnit.Framework.CategoryAttribute("perScenarioContainer")]
    [NUnit.Framework.CategoryAttribute("withUserNotificationTableStorage")]
    public partial class GetNotificationsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "perScenarioContainer",
                "withUserNotificationTableStorage"};
        
#line 1 "GetNotifications.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features/AzureTableStorage", "Get Notifications", null, ProgrammingLanguage.CSharp, new string[] {
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
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
#line 7
 testRunner.Given("I have created and stored 50 notifications with timestamps at 30 second intervals" +
                    " for the user with Id \'user1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
 testRunner.And("I have created and stored 5 notifications with timestamps at 30 second intervals " +
                    "for the user with Id \'user2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve notifications for a user")]
        public virtual void RetrieveNotificationsForAUser()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve notifications for a user", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 10
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
#line 6
this.FeatureBackground();
#line hidden
#line 11
 testRunner.When("I ask the user notification store for 20 notifications for the user with Id \'user" +
                        "1\' and call the result \'result1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 12
 testRunner.Then("the get notifications result called \'result1\' should contain 20 notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 13
 testRunner.And("the get notifications result called \'result1\' should contain notifications in des" +
                        "cending order of timestamp", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 14
 testRunner.And("the get notifications result called \'result1\' should contain a continuation token" +
                        "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve notifications for a user using a continuation token")]
        public virtual void RetrieveNotificationsForAUserUsingAContinuationToken()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve notifications for a user using a continuation token", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 16
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
#line 6
this.FeatureBackground();
#line hidden
#line 17
 testRunner.Given("I have asked the user notification store for 20 notifications for the user with I" +
                        "d \'user1\' and called the result \'result1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 18
 testRunner.When("I ask the user notification store for notifications for the user with Id \'user1\' " +
                        "using the continuation token from the result called \'result1\' and call the resul" +
                        "t \'result2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 19
 testRunner.Then("the get notifications result called \'result2\' should contain 20 notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 20
 testRunner.And("the get notifications result called \'result2\' should contain notifications in des" +
                        "cending order of timestamp", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 21
 testRunner.And("the get notifications result called \'result2\' should contain a continuation token" +
                        "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 22
 testRunner.And("the get notifications results called \'result1\' and \'result2\' should not contain a" +
                        "ny of the same notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 23
 testRunner.And("the get notifications result called \'result2\' should only contain notifications w" +
                        "ith an earlier timestamp than those in the get notifications result \'result1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve final page of notifications for a user using continuation tokens")]
        public virtual void RetrieveFinalPageOfNotificationsForAUserUsingContinuationTokens()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve final page of notifications for a user using continuation tokens", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 25
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
#line 6
this.FeatureBackground();
#line hidden
#line 26
 testRunner.Given("I have asked the user notification store for 20 notifications for the user with I" +
                        "d \'user1\' and called the result \'result1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 27
 testRunner.And("I have asked the user notification store for notifications for the user with Id \'" +
                        "user1\' using the continuation token from the result called \'result1\' and call th" +
                        "e result \'result2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 28
 testRunner.When("I ask the user notification store for notifications for the user with Id \'user1\' " +
                        "using the continuation token from the result called \'result2\' and call the resul" +
                        "t \'result3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 29
 testRunner.Then("the get notifications result called \'result3\' should contain 10 notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 30
 testRunner.And("the get notifications result called \'result3\' should contain notifications in des" +
                        "cending order of timestamp", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 31
 testRunner.And("the get notifications result called \'result3\' should not contain a continuation t" +
                        "oken", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 32
 testRunner.And("the get notifications results called \'result2\' and \'result3\' should not contain a" +
                        "ny of the same notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 33
 testRunner.And("the get notifications result called \'result3\' should only contain notifications w" +
                        "ith an earlier timestamp than those in the get notifications result \'result2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve notifications since a specified previous notification")]
        public virtual void RetrieveNotificationsSinceASpecifiedPreviousNotification()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve notifications since a specified previous notification", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 35
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
#line 6
this.FeatureBackground();
#line hidden
#line 36
 testRunner.Given("I have asked the user notification store for 20 notifications for the user with I" +
                        "d \'user1\' and called the result \'result1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 37
 testRunner.And("I have created and stored a notification for the user with Id \'user1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 38
 testRunner.When("I ask the user notification store for 20 notifications since the first notificati" +
                        "on in the results called \'result1\' for the user with Id \'user1\' and call the res" +
                        "ult \'result2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 39
 testRunner.Then("the get notifications result called \'result2\' should contain 1 notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 40
 testRunner.And("the get notifications result called \'result2\' should not contain a continuation t" +
                        "oken", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 41
 testRunner.And("the get notifications results called \'result1\' and \'result2\' should not contain a" +
                        "ny of the same notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 42
 testRunner.And("the get notifications result called \'result1\' should only contain notifications w" +
                        "ith an earlier timestamp than those in the get notifications result \'result2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve all notifications for a user in a single request")]
        public virtual void RetrieveAllNotificationsForAUserInASingleRequest()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve all notifications for a user in a single request", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 44
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
#line 6
this.FeatureBackground();
#line hidden
#line 45
 testRunner.When("I ask the user notification store for 20 notifications for the user with Id \'user" +
                        "2\' and call the result \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 46
 testRunner.Then("the get notifications result called \'result\' should contain 5 notifications", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 47
 testRunner.And("the get notifications result called \'result\' should contain notifications in desc" +
                        "ending order of timestamp", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 48
 testRunner.And("the get notifications result called \'result\' should not contain a continuation to" +
                        "ken", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
