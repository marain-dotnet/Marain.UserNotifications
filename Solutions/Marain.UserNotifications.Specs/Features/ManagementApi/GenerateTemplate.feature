@perFeatureContainer
@useApis
@useTransientTenant
Feature: Generate Template

Scenario: Generate populated template
	Given I have created and stored an email notification template
		| body                                    | subject                           | important | contentType                                                                    | image        | notificationType |
		| A new lead was added by {{leadAddedBy}} | A new lead added: {{leadAddedBy}} | true      | application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1 | Base+64xddfa | marain.NewLeadv1 |
	And I have created and stored a sms notification template
		| body                              | contentType                                                                  | notificationType |
		| New lead added by {{leadAddedBy}} | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.NewLeadv1 |
	And I have created and stored a web push notification template
		| body                                    | title                             | contentType                                                                      | actionUrl                 | image        | notificationType |
		| A new lead was added by {{leadAddedBy}} | A new lead added: {{leadAddedBy}} | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.NewLeadv1 |
	When I send the generate template API request
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
	Then the response status code should be 'OK'
	And the response content should have a string property called 'notificationType' with value 'marain.NewLeadv1'
	And the response content should have a json property called 'smsTemplate' with value '{ "notificationType": "marain.NewLeadv1", "body": "New lead added by TestUser123", "contentType": "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1" }'
	And the response content should have a json property called 'emailTemplate' with value '{ "notificationType": "marain.NewLeadv1", "body": "A new lead was added by TestUser123", "subject": "A new lead added: TestUser123", "important": false, "contentType": "application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1" }'
	And the response content should have a json property called 'webPushTemplate' with value '{ "notificationType": "marain.NewLeadv1", "body": "A new lead was added by TestUser123", "title": "A new lead added: TestUser123", "image": "Base+64xddfa", "actionUrl": "https://www.google.co.uk/", "contentType": "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1" }'
