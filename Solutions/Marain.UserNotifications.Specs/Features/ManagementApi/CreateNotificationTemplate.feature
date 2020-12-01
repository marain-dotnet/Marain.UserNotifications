@perFeatureContainer
@useApis
@useTransientTenant

Feature: Create Notification Template

Scenario: Create a notification template for a certain tenant
	When I send the user notification template API a request to create a new user notification template
		"""
		{
			"notificationType": "smartr365.lead.added.v1",
			"smsObject": 
			{
				"body": "A new lead was added by {leadAddedBy}"
			}
		}
		"""
	Then the response status code should be 'OK'
	
Scenario: Update a notification template for a certain tenant
	Given I have created and stored a notification template
	| notificationType				 | sms                                         |
	| Marain.Notification.NewLead.v1 | {"body": "A new lead was added by {leadAddedBy}"} |
	When I send the user notification template API a request to create a new user notification template
		"""
		{
			"notificationType": "smartr365.lead.added.v1",
			"smsObject": 
			{
				"body": "Different template"
			}
		}
		"""
	Then the response status code should be 'OK'
	