@perFeatureContainer
@useApis
@useTransientTenant
Feature: Generate Notification Template via the client library

Scenario: Generate a web push Notification Template
	Given I have created and stored a web push notification template
		| body                                    | title                             | contentType                                                                      | actionUrl                 | image        | notificationType |
		| A new lead was added by {{leadAddedBy}} | A new lead added: {{leadAddedBy}} | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.NewLeadv1 |
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.NewLeadv1",
            "timestamp": "2020-07-21T17:32:28Z",
            "userIds": [
                "1"
            ],
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "leadAddedBy": "TestUser123",
            }
        }
		"""
	Then the client response status code should be 'OK'
	And the client response for the notification template property 'SmsTemplate' should be null
	And the client response for the notification template property 'WebPushTemplate' should not be null
	And the client response for the object 'WebPushTemplate' with property 'Body' should have a value of 'A new lead was added by TestUser123'
	And the client response for the object 'WebPushTemplate' with property 'Title' should have a value of 'A new lead added: TestUser123'
	And the client response for the object 'WebPushTemplate' with property 'ActionUrl' should have a value of 'https://www.google.co.uk/'

Scenario: Generate a web push, sms and email Notification Template
	Given I have created and stored a web push notification template
		| body                                    | title                            | contentType                                                                      | actionUrl                 | image        | notificationType |
		| A new lead was added by {{leadAddedBy}} | You have a {{mortgageType}} case | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.NewLeadv2 |
	And I have created and stored a sms notification template
		| body                                    | contentType                                                                  | notificationType |
		| A new lead was added by {{leadAddedBy}} | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.NewLeadv2 |
	And I have created and stored an email notification template
		| body                                                                         | subject                  | important | contentType                                                                    | image        | notificationType |
		| A new lead was added by {{leadAddedBy}} with Mortgage Type: {{mortgageType}} | New lead {{leadAddedBy}} | true      | application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1 | Base+64xddfa | marain.NewLeadv2 |
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.NewLeadv2",
            "timestamp": "2020-07-21T17:32:28Z",
            "userIds": [
                "2"
            ],
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "leadAddedBy": "TestUser123",
                "mortgageType": "First time buyer"
            }
        }
		"""
	Then the client response status code should be 'OK'
	And the client response for the notification template property 'WebPushTemplate' should not be null
	And the client response for the notification template property 'SmsTemplate' should not be null
	And the client response for the notification template property 'EmailTemplate' should not be null
	And the client response for the object 'WebPushTemplate' with property 'Body' should have a value of 'A new lead was added by TestUser123'
	And the client response for the object 'WebPushTemplate' with property 'Title' should have a value of 'You have a First time buyer case'
	And the client response for the object 'WebPushTemplate' with property 'Image' should have a value of 'Base+64xddfa'
	And the client response for the object 'WebPushTemplate' with property 'ActionUrl' should have a value of 'https://www.google.co.uk/'
	And the client response for the object 'SmsTemplate' with property 'Body' should have a value of 'A new lead was added by TestUser123'
	And the client response for the object 'EmailTemplate' with property 'Body' should have a value of 'A new lead was added by TestUser123 with Mortgage Type: First time buyer'
	And the client response for the object 'EmailTemplate' with property 'Subject' should have a value of 'New lead TestUser123'

Scenario: Generation of a Notification Template is UnSuccessful
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.notifications.test.v1",
            "timestamp": "2020-07-21T17:32:28Z",
            "userIds": [
                "3"
            ],
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "leadAddedBy": "TestUser123",
            }
        }
		"""
	Then the client response status code should be 'OK'
	And the client response for the notification template property 'WebPushTemplate' should be null
	And the client response for the notification template property 'SmsTemplate' should be null

Scenario: Generate a notification template for unconfigured communication channel
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.notifications.test.v2",
            "timestamp": "2020-07-21T17:32:28Z",
            "userIds": [
                "4"
            ],
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "leadAddedBy": "TestUser123",
            }
        }
		"""
	Then the client response for the notification template property 'WebPushTemplate' should be null
	And the client response for the notification template property 'SmsTemplate' should be null
	And the client response for the notification template property 'EmailTemplate' should be null