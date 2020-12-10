@perFeatureContainer
@useApis
@useTransientTenant
Feature: Generate Template

Scenario: Generate populated template
	Given I have created and stored a notification template
		| notificationType             | smsTemplate                                         |
		| marain.notifications.test.v1 | {"body": "A new lead was added by {{leadAddedBy}}"} |
	And I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration  | eTag |
		| 1      | test@test.com | 041532211   | {"marain.notifications.test.v1": ["email", "sms"]} | null |
	When I send the generate template API request
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
	Then the response status code should be 'OK'
	And the response content should have a string property called 'notificationType' with value 'marain.notifications.test.v1'
	And the response content should have a json property called 'smsTemplate' with value '{"body": "A new lead was added by TestUser123"}'