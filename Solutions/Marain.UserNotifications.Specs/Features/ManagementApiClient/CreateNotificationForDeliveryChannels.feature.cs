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
namespace Marain.UserNotifications.Specs.Features.ManagementApiClient
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Create Notification For Third Party Delivery Channels via the client library")]
    [NUnit.Framework.CategoryAttribute("perFeatureContainer")]
    [NUnit.Framework.CategoryAttribute("useApis")]
    [NUnit.Framework.CategoryAttribute("useTransientTenant")]
    public partial class CreateNotificationForThirdPartyDeliveryChannelsViaTheClientLibraryFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "perFeatureContainer",
                "useApis",
                "useTransientTenant"};
        
#line 1 "CreateNotificationForDeliveryChannels.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features/ManagementApiClient", "Create Notification For Third Party Delivery Channels via the client library", null, ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create web push notifications")]
        [NUnit.Framework.TestCaseAttribute("Single user", "marain.test.notification.v1", "[\"304ABC0E-08AF-4EF5-A9AC-281B67D633F4\"]", "2012-03-19T07:22Z", "{ \"prop1\": \"val1\" }", "[\"id1\", \"id2\"]", "{ \"webPush\": \"airship\" }", null)]
        [NUnit.Framework.TestCaseAttribute("Multiple users", "marain.test.notification.v1", "[\"304ABC0E-08AF-4EF5-A9AC-281B67D633F4\", \"5547F9E1-A3B1-4D39-BFA9-73129EF475A9\"]", "2012-03-19T07:22Z", "{ \"prop1\": \"val1\" }", "[\"id1\", \"id2\"]", "{ \"webPush\": \"airship\" }", null)]
        public void CreateWebPushNotifications(string notes, string notificationType, string userIds, string timestamp, string propertiesJson, string correlationIds, string deliveryChannelConfiguredPerCommunicationType, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("Notes", notes);
            argumentsOfScenario.Add("NotificationType", notificationType);
            argumentsOfScenario.Add("UserIds", userIds);
            argumentsOfScenario.Add("Timestamp", timestamp);
            argumentsOfScenario.Add("PropertiesJson", propertiesJson);
            argumentsOfScenario.Add("CorrelationIds", correlationIds);
            argumentsOfScenario.Add("DeliveryChannelConfiguredPerCommunicationType", deliveryChannelConfiguredPerCommunicationType);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create web push notifications", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 6
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                            "NotificationType",
                            "UserIds",
                            "Timestamp",
                            "PropertiesJson",
                            "CorrelationIds",
                            "DeliveryChannelConfiguredPerCommunicationType"});
                table9.AddRow(new string[] {
                            string.Format("{0}", notificationType),
                            string.Format("{0}", userIds),
                            string.Format("{0}", timestamp),
                            string.Format("{0}", propertiesJson),
                            string.Format("{0}", correlationIds),
                            string.Format("{0}", deliveryChannelConfiguredPerCommunicationType)});
#line 7
 testRunner.When("I use the client to send a management API request to create a new notification vi" +
                        "a third party delivery channels", ((string)(null)), table9, "When ");
#line hidden
#line 10
 testRunner.Then("no exception should be thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 11
 testRunner.And("the client response status code should be \'Accepted\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 12
 testRunner.And("the client response should contain a \'Location\' header", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
