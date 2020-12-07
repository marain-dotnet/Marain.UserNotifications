@perFeatureContainer
@useApis
@useTransientTenant

Feature: Generate Notification Template via the client library

Scenario: Generate a Notification Template
	Given I have created and stored a notification template
		| notificationType             | smsTemplate                                         |
		| marain.notifications.test.v1 | {"body": "A new lead was added by {{leadAddedBy}}"} |
	And I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration    |
		| 1      | test@test.com | 041532211   | {"marain.notifications.test.v1": ["webpush", "sms"]} |
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.notifications.test.v1",
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
    And the client response for the notification template property 'SmsTemplate' should not be null
    And the client response for the object 'SmsTemplate' with property 'Body' should have a value of 'A new lead was added by TestUser123'

Scenario: Generation of a Notification Template is UnSuccessful
	Given I have created and stored a notification template
		| notificationType             | smsTemplate                                         |
		| marain.notifications.test.v1 | {"body": "A new lead was added by {{leadAddedBy}}"} |
	And I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 1      | test@test.com | 041532211   | {"marain.notifications.test.v1": ["webpush"]}     |
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.notifications.test.v1",
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
    And the client response for the notification template property 'WebPushTemplate' should be null
    And the client response for the notification template property 'SmsTemplate' should be null

Scenario: Generate a notification template for unconfigured user
    Given I have created and stored a notification template
		| notificationType             | smsTemplate                                         |
		| marain.notifications.test.v1 | {"body": "A new lead was added by {{leadAddedBy}}"} |
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.notifications.test.v1",
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
	Then a 'UserNotificationsApiException' should be thrown

Scenario: Generate a notification template for unconfigured communication channel
   Given I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 1      | test@test.com | 041532211   | {"marain.notifications.test.v1": ["webpush"]}     |
	When I use the client to send a generate template API request
		"""
        {
            "notificationType": "marain.notifications.test.v1",
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
	Then a 'UserNotificationsApiException' should be thrown