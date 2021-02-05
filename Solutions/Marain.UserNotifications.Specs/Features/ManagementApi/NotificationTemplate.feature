@perFeatureContainer
@useApis
@useTransientTenant
Feature: Notification Template

########################################
# Web Push notification template tests #
########################################
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

Scenario: Update a web push notification template
	Given I have created and stored a web push notification template
		| body | title | contentType                                                                      | actionUrl                 | image        | notificationType      |
		| body | test  | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.test.template1 |
	When I send the user notification template API a request to update an existing web push notification template
		| body                  | title | contentType                                                                      | actionUrl                 | image        | notificationType      |
		| updated body template | test  | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.test.template1 |
	Then the response status code should be 'OK'

Scenario: Get a web push notification template
	Given I have created and stored a web push notification template
		| body                                    | title | contentType                                                                      | actionUrl                 | image        | notificationType      |
		| A new lead was added by {{leadAddedBy}} | test  | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.test.template2 |
	When I send the notification template API a request to retreive a notification template with notificationType 'marain.test.template2' and communicationType 'webPush'
	Then the response status code should be 'OK'
	And the response header should have a property called eTag that should not be null
	And the response content should have a string property called 'notificationType' with value 'marain.test.template2'
	And the response content should have a json property called 'body' with value 'A new lead was added by {{leadAddedBy}}'
	And the response content should have a json property called 'title' with value 'test'
	And the response content should have a json property called 'contentType' with value 'application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1'
	And the response content should have a json property called 'image' with value 'Base+64xddfa'
	And the response content should have a json property called 'actionUrl' with value 'https://www.google.co.uk/'
	And the response content should have a property called '_links.self'

Scenario: Get a web push notification for invalid notificationType
	When I send the notification template API a request to retreive a notification template with notificationType 'marain.test.invalidNotificationType' and communicationType 'webPush'
	Then the response status code should be 'NotFound'

#####################################
# Email notification template tests #
#####################################
Scenario: Create an email notification template
	When I send the user notification template API a request to create a new user notification template
		"""
		{
			"body": "this is a test template",
			"subject": "test",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1",
			"notificationType": "marain.test.template"
		}
		"""
	Then the response status code should be 'OK'

Scenario: Update an email notification template
	Given I have created and stored an email notification template
		| body | subject | contentType                                                                    | image        | notificationType      |
		| body | test    | application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1 | Base+64xddfa | marain.test.template3 |
	When I send the user notification template API a request to update an existing email notification template
		| body                  | subject | contentType                                                                    | image        | notificationType      |
		| updated body template | test    | application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1 | Base+64xddfa | marain.test.template3 |
	Then the response status code should be 'OK'

Scenario: Get an email notification template
	Given I have created and stored an email notification template
		| body                                    | subject | contentType                                                                    | image        | notificationType      |
		| A new lead was added by {{leadAddedBy}} | test    | application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1 | Base+64xddfa | marain.test.template4 |
	When I send the notification template API a request to retreive a notification template with notificationType 'marain.test.template4' and communicationType 'email'
	Then the response status code should be 'OK'
	And the response header should have a property called eTag that should not be null
	And the response content should have a string property called 'notificationType' with value 'marain.test.template4'
	And the response content should have a json property called 'body' with value 'A new lead was added by {{leadAddedBy}}'
	And the response content should have a json property called 'subject' with value 'test'
	And the response content should have a json property called 'contentType' with value 'application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1'
	And the response content should have a property called '_links.self'

########################################
# Sms notification template tests	   #
########################################
Scenario: Create a sms notification template
	When I send the user notification template API a request to create a new user notification template
		"""
		{
			"body": "this is a sms test template",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1",
			"notificationType": "marain.test.notification"
		}
		"""
	Then the response status code should be 'OK'

Scenario: Update a sms notification template
	Given I have created and stored a sms notification template
		| body | contentType                                                                  | notificationType          |
		| body | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.notification5 |
	When I send the user notification template API a request to update an existing sms notification template
		| body                                  | contentType                                                                  | notificationType          |
		| this is an updated sms test template2 | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.notification5 |
	Then the response status code should be 'OK'

Scenario: Get a sms notification template
	Given I have created and stored a sms notification template
		| body                                    | contentType                                                                  | notificationType          |
		| A new lead was added by {{leadAddedBy}} | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.notification6 |
	When I send the notification template API a request to retreive a notification template with notificationType 'marain.test.notification6' and communicationType 'sms'
	Then the response status code should be 'OK'
	And the response header should have a property called eTag that should not be null
	And the response content should have a string property called 'notificationType' with value 'marain.test.notification6'
	And the response content should have a json property called 'body' with value 'A new lead was added by {{leadAddedBy}}'
	And the response content should have a json property called 'contentType' with value 'application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1'
	And the response content should have a property called '_links.self'

Scenario: Update a sms notification template without an eTag
	Given I have created and stored a sms notification template
		| body | contentType                                                                  | notificationType          |
		| body | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.notification7 |
	When I send the user notification template API a request to update an existing sms notification template without an eTag
		| body                                  | contentType                                                                  | notificationType          |
		| this is an updated sms test template2 | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.notification7 |
	Then the response status code should be 'InternalServerError'

Scenario: Update a sms notification template without an invalid eTag
	Given I have created and stored a sms notification template
		| body | contentType                                                                  | notificationType          |
		| body | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.notification8 |
	When I send the user notification template API a request to update an existing sms notification template with an invalid eTag
		| body                                  | contentType                                                                  | notificationType          | eTag                    |
		| this is an updated sms test template2 | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.notification8 | "\"0x8D89CF9D612C7F1\"" |
	Then the response status code should be 'BadRequest'