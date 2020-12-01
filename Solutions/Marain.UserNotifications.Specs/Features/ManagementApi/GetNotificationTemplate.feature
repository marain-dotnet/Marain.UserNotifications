@perFeatureContainer
@useApis
@useTransientTenant

Feature: Get Notification Template

Scenario: Get a notification template
	Given I have created and stored a notification template
		| notificationType				 | sms                                               |
		| Marain.Notification.NewLead.v1 | {"body": "A new lead was added by {leadAddedBy}"} |
	When I send the notification template API a request to retreive a notification template with notificationType 'Marain.Notification.NewLead.v1'
	Then the response status code should be 'OK'
	And the response content should have a string property called 'notificationType' with value 'Marain.Notification.NewLead.v1'
	And the response content should have a json property called 'sms' with value '{"body": "A new lead was added by {leadAddedBy}"}'
