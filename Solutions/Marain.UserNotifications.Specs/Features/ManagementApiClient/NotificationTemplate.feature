@perFeatureContainer
@useApis
@useTransientTenant

Feature: Notification Template via the client library

Scenario: Create a Notification Template
	When I use the client to send the notification template API a request to create a new notification template
		"""
		{
			"notificationType": "Marain.Notification.NewLead.v1",
			"smsTemplate": 
			{
				"body": "A new lead was added by {{leadAddedBy}}"
			}
		}
		"""
	Then the client response status code should be 'OK'

Scenario: Update a Notification Template
	Given I have created and stored a notification template
	| notificationType               | smsTemplate                                         |
	| Marain.Notification.NewLead.v1 | {"body": "A new lead was added by {{leadAddedBy}}"} |
	When I use the client to send the notification template API a request to create a new notification template
		"""
		{
			"notificationType": "Marain.Notification.NewLead.v1",
			"smsTemplate": 
			{
				"body": "Different template"
			}
		}
		"""
	Then the client response status code should be 'OK'
	# And the response content should have a json property called 'sms' with value '{"body": "Different template"}'

Scenario: Get a notification template
	Given I have created and stored a notification template
	| notificationType               | smsTemplate                                         |
	| Marain.Notification.NewLead.v1 | {"body": "A new lead was added by {{leadAddedBy}}"} |
	When I use the client to send the notification template API a request to get a notification template with notification type 'Marain.Notification.NewLead.v1'
	Then the response content should have a string property called 'notificationType' with value 'Marain.Notification.NewLead.v1'
	And the response content should have a json property called 'smsTemplate' with value '{"body": "Different template"}'
	