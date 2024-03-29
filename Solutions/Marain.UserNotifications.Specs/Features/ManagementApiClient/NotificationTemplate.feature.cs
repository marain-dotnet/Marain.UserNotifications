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
    [NUnit.Framework.DescriptionAttribute("Notification Template via the client library")]
    [NUnit.Framework.CategoryAttribute("perFeatureContainer")]
    [NUnit.Framework.CategoryAttribute("useApis")]
    [NUnit.Framework.CategoryAttribute("useTransientTenant")]
    public partial class NotificationTemplateViaTheClientLibraryFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "perFeatureContainer",
                "useApis",
                "useTransientTenant"};
        
#line 1 "NotificationTemplate.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features/ManagementApiClient", "Notification Template via the client library", null, ProgrammingLanguage.CSharp, featureTags);
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
        [NUnit.Framework.DescriptionAttribute("Create a Web Push Notification Template")]
        public void CreateAWebPushNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a Web Push Notification Template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 9
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 10
 testRunner.When("I use the client to send the notification template API a request to create a new " +
                        "web push notification template", "{\r\n\t\"body\": \"this is a test template\",\r\n\t\"title\": \"test\",\r\n\t\"contentType\": \"appli" +
                        "cation/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1\",\r\n\t" +
                        "\"notificationType\": \"marain.test.template\",\r\n\t\"actionUrl\": \"https://www.google.c" +
                        "o.uk/\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 20
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update a Web Push Notification Template")]
        public void UpdateAWebPushNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update a Web Push Notification Template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 22
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                            "body",
                            "title",
                            "contentType",
                            "actionUrl",
                            "image",
                            "notificationType"});
                table15.AddRow(new string[] {
                            "body",
                            "test",
                            "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1",
                            "https://www.google.co.uk/",
                            "Base+64xddfa",
                            "marain.test.template3"});
#line 23
 testRunner.Given("I have created and stored a web push notification template", ((string)(null)), table15, "Given ");
#line hidden
#line 26
 testRunner.When("I use the client to send the notification template API a request to update a web " +
                        "push notification template", @"{
	""body"": ""this is an updated test template2"",
	""title"": ""test"",
	""contentType"": ""application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1"",
	""notificationType"": ""marain.test.template3"",
	""actionUrl"": ""https://www.google.co.uk/""
}", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 36
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get a Web Push notification template")]
        public void GetAWebPushNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get a Web Push notification template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 38
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 39
 testRunner.Given("I use the client to send the notification template API a request to create a new " +
                        "web push notification template", @"{
	""body"": ""body"",
	""title"": ""test"",
	""contentType"": ""application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1"",
	""notificationType"": ""marain.test.template4"",
	""image"": ""Base+64xddfa"",
	""actionUrl"": ""https://www.google.co.uk/""
}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 50
 testRunner.When("I use the client to send the notification template API a request to get a notific" +
                        "ation template with notification type \'marain.test.template4\' and communication " +
                        "type \'WebPush\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 51
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 52
 testRunner.And("the client response should contain a \'ETag\' header", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 53
 testRunner.And("the web push template in the UserManagement API response should have a \'Body\' wit" +
                        "h value \'body\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 54
 testRunner.And("the web push template in the UserManagement API response should have a \'Title\' wi" +
                        "th value \'test\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 55
 testRunner.And("the web push template in the UserManagement API response should have a \'ContentTy" +
                        "pe\' with value \'application/vnd.marain.usernotifications.notificationtemplate.we" +
                        "bpushtemplate.v1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 56
 testRunner.And("the web push template in the UserManagement API response should have a \'Image\' wi" +
                        "th value \'Base+64xddfa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 57
 testRunner.And("the web push template in the UserManagement API response should have a \'Notificat" +
                        "ionType\' with value \'marain.test.template4\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 58
 testRunner.And("the web push template in the UserManagement API response should have a \'ActionUrl" +
                        "\' with value \'https://www.google.co.uk/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get a Web Push notification template that doesn\'t exist")]
        public void GetAWebPushNotificationTemplateThatDoesntExist()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get a Web Push notification template that doesn\'t exist", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 60
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 61
 testRunner.When("I use the client to send the notification template API a request to get a notific" +
                        "ation template with notification type \'marain.test.notcreatedtemplate\' and commu" +
                        "nication type \'WebPush\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 62
 testRunner.Then("the UserNotificationsApiException status code should be \'NotFound\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create an email notification template")]
        public void CreateAnEmailNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create an email notification template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 67
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 68
 testRunner.When("I use the client to send the notification template API a request to create a new " +
                        "email notification template", "{\r\n\t\"body\": \"this is a test template\",\r\n\t\"contentType\": \"application/vnd.marain.u" +
                        "sernotifications.notificationtemplate.emailtemplate.v1\",\r\n\t\"notificationType\": \"" +
                        "marain.test.template\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 76
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update an email notification template")]
        public void UpdateAnEmailNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update an email notification template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 78
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                            "body",
                            "subject",
                            "contentType",
                            "notificationType"});
                table16.AddRow(new string[] {
                            "body",
                            "test",
                            "application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1",
                            "marain.test.template5"});
#line 79
 testRunner.Given("I have created and stored an email notification template", ((string)(null)), table16, "Given ");
#line hidden
#line 82
 testRunner.When("I use the client to send the notification template API a request to update an ema" +
                        "il notification template", "{\r\n\t\"body\": \"this is an updated test template2\",\r\n\t\"contentType\": \"application/vn" +
                        "d.marain.usernotifications.notificationtemplate.emailtemplate.v1\",\r\n\t\"notificati" +
                        "onType\": \"marain.test.template5\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 90
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get an email notification template")]
        public void GetAnEmailNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get an email notification template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 92
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 93
 testRunner.Given("I use the client to send the notification template API a request to create a new " +
                        "email notification template", "{\r\n\t\"body\": \"body\",\r\n\t\"subject\": \"test\",\r\n\t\"contentType\": \"application/vnd.marain" +
                        ".usernotifications.notificationtemplate.emailtemplate.v1\",\r\n\t\"notificationType\":" +
                        " \"marain.test.template6\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 102
 testRunner.When("I use the client to send the notification template API a request to get a notific" +
                        "ation template with notification type \'marain.test.template6\' and communication " +
                        "type \'Email\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 103
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 104
 testRunner.And("the client response should contain a \'ETag\' header", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 105
 testRunner.And("the email template in the UserManagement API response should have a \'Body\' with v" +
                        "alue \'body\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 106
 testRunner.And("the email template in the UserManagement API response should have a \'Subject\' wit" +
                        "h value \'test\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 107
 testRunner.And("the email template in the UserManagement API response should have a \'ContentType\'" +
                        " with value \'application/vnd.marain.usernotifications.notificationtemplate.email" +
                        "template.v1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 108
 testRunner.And("the email template in the UserManagement API response should have a \'Notification" +
                        "Type\' with value \'marain.test.template6\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get a Email notification template that doesn\'t exist")]
        public void GetAEmailNotificationTemplateThatDoesntExist()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get a Email notification template that doesn\'t exist", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 110
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 111
 testRunner.When("I use the client to send the notification template API a request to get a notific" +
                        "ation template with notification type \'marain.test.notcreatedtemplate\' and commu" +
                        "nication type \'Email\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 112
 testRunner.Then("the UserNotificationsApiException status code should be \'NotFound\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create an sms notification template")]
        public void CreateAnSmsNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create an sms notification template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 117
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 118
 testRunner.When("I use the client to send the notification template API a request to create a new " +
                        "sms notification template", "{\r\n\t\"body\": \"this is a test template\",\r\n\t\"contentType\": \"application/vnd.marain.u" +
                        "sernotifications.notificationtemplate.smstemplate.v1\",\r\n\t\"notificationType\": \"ma" +
                        "rain.test.template\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 126
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update an sms notification template")]
        public void UpdateAnSmsNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update an sms notification template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 128
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                            "body",
                            "contentType",
                            "notificationType"});
                table17.AddRow(new string[] {
                            "body",
                            "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1",
                            "marain.test.template7"});
#line 129
 testRunner.Given("I have created and stored an sms notification template", ((string)(null)), table17, "Given ");
#line hidden
#line 132
 testRunner.When("I use the client to send the notification template API a request to update an sms" +
                        " notification template", "{\r\n\t\"body\": \"this is an updated test template2\",\r\n\t\"contentType\": \"application/vn" +
                        "d.marain.usernotifications.notificationtemplate.smstemplate.v1\",\r\n\t\"notification" +
                        "Type\": \"marain.test.template7\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 140
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get an sms notification template")]
        public void GetAnSmsNotificationTemplate()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get an sms notification template", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 142
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 143
 testRunner.Given("I use the client to send the notification template API a request to create a new " +
                        "sms notification template", "{\r\n\t\"body\": \"body\",\r\n\t\"contentType\": \"application/vnd.marain.usernotifications.no" +
                        "tificationtemplate.smstemplate.v1\",\r\n\t\"notificationType\": \"marain.test.template8" +
                        "\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 151
 testRunner.When("I use the client to send the notification template API a request to get a notific" +
                        "ation template with notification type \'marain.test.template8\' and communication " +
                        "type \'Sms\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 152
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 153
 testRunner.And("the client response should contain a \'ETag\' header", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 154
 testRunner.And("the sms template in the UserManagement API response should have a \'Body\' with val" +
                        "ue \'body\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 155
 testRunner.And("the sms template in the UserManagement API response should have a \'ContentType\' w" +
                        "ith value \'application/vnd.marain.usernotifications.notificationtemplate.smstemp" +
                        "late.v1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 156
 testRunner.And("the sms template in the UserManagement API response should have a \'NotificationTy" +
                        "pe\' with value \'marain.test.template8\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Request sms notification template by self link")]
        public void RequestSmsNotificationTemplateBySelfLink()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Request sms notification template by self link", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 158
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                            "body",
                            "contentType",
                            "notificationType"});
                table18.AddRow(new string[] {
                            "body",
                            "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1",
                            "marain.test.template9"});
#line 159
 testRunner.Given("I have created and stored an sms notification template", ((string)(null)), table18, "Given ");
#line hidden
#line 162
 testRunner.And("I use the client to send the notification template API a request to get a notific" +
                        "ation template with notification type \'marain.test.template9\' and communication " +
                        "type \'Sms\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 163
 testRunner.When("I use the client to send a management API request to get a sms notification templ" +
                        "ate using the link called \'self\' from the previous API response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 164
 testRunner.Then("the client response status code should be \'OK\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get a sms notification template that doesn\'t exist")]
        public void GetASmsNotificationTemplateThatDoesntExist()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get a sms notification template that doesn\'t exist", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 166
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 167
 testRunner.When("I use the client to send the notification template API a request to get a notific" +
                        "ation template with notification type \'marain.test.notcreatedtemplate\' and commu" +
                        "nication type \'Sms\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 168
 testRunner.Then("the UserNotificationsApiException status code should be \'NotFound\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
