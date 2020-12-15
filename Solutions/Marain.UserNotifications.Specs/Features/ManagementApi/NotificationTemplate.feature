@perFeatureContainer
@useApis
@useTransientTenant
Feature: Notification Template

Scenario: Create a web push notification template
	When I send the user notification template API a request to create a new user notification template
		"""
		{
			"body": "this is a test template",
			"title": "test",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1",
			"notificationType": "marain.test.template"
		}
		"""
	Then the response status code should be 'OK'

Scenario: Update a web push notification notification template
	Given I have created and stored a web push notification template
		| body | title | contentType                                                                      | image        | notificationType     |
		| body | test  | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | Base+64xddfa | marain.test.template |
	When I send the user notification template API a request to update a new user notification template
		"""
		{
			"body": "this is an updated test template2",
			"title": "test",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1",
			"notificationType": "marain.test.template"
		}
		"""
	Then the response status code should be 'OK'

Scenario: Get a web push notification template
	Given I have created and stored a web push notification template
		| body                                    | title | contentType                                                                      | image        | notificationType     |
		| A new lead was added by {{leadAddedBy}} | test  | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | Base+64xddfa | marain.test.template |
	When I send the notification template API a request to retreive a notification template with notificationType 'marain.test.template' and communicationType 'webPush'
	Then the response status code should be 'OK'
	And the response content should have a string property called 'notificationType' with value 'marain.test.template'
	And the response content should have a json property called 'body' with value '{"body": "A new lead was added by {{leadAddedBy}}"}'
	And the response content should have a json property called 'title' with value 'test'
	And the response content should have a json property called 'contentType' with value 'application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1'
	And the response content should have a json property called 'image' with value 'Base+64xddfa'
	And the response content should have a property called '_links.self'