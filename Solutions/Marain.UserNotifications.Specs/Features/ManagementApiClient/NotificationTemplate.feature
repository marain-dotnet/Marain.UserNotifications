@perFeatureContainer
@useApis
@useTransientTenant
Feature: Notification Template via the client library

########################################
# Web Push notification template tests #
########################################
Scenario: Create a Web Push Notification Template
	When I use the client to send the notification template API a request to create a new web push notification template
		"""
		{
			"body": "this is a test template",
			"title": "test",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1",
			"notificationType": "marain.test.template",
			"actionUrl": "https://www.google.co.uk/"
		}
		"""
	Then the client response status code should be 'OK'

Scenario: Update a Web Push Notification Template
	Given I have created and stored a web push notification template
		| body | title | contentType                                                                      | actionUrl                 | image        | notificationType      |
		| body | test  | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.test.template3 |
	When I use the client to send the notification template API a request to update a web push notification template
		"""
		{
			"body": "this is an updated test template2",
			"title": "test",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1",
			"notificationType": "marain.test.template3",
			"actionUrl": "https://www.google.co.uk/"
		}
		"""
	Then the client response status code should be 'OK'

Scenario: Get a Web Push notification template
	Given I have created and stored a web push notification template
		| body | title | contentType                                                                      | actionUrl                 | image        | notificationType      |
		| body | test  | application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1 | https://www.google.co.uk/ | Base+64xddfa | marain.test.template4 |
	When I use the client to send the notification template API a request to get a notification template with notification type 'marain.test.template4' and communication type 'WebPush'
	Then the client response status code should be 'OK'
	And the web push template in the UserManagement API response should have a 'body' with value 'body'
	And the web push template in the UserManagement API response should have a 'title' with value 'test'
	And the web push template in the UserManagement API response should have a 'contentType' with value 'application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1'
	And the web push template in the UserManagement API response should have a 'image' with value 'Base+64xddfa'
	And the web push template in the UserManagement API response should have a 'notificationType' with value 'marain.test.template4'
	And the web push template in the UserManagement API response should have a 'actionUrl' with value 'https://www.google.co.uk/'

#####################################
# Email notification template tests #
#####################################
Scenario: Create an email notification template
	When I use the client to send the notification template API a request to create a new email notification template
		"""
		{
			"body": "this is a test template",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1",
			"notificationType": "marain.test.template"
		}
		"""
	Then the client response status code should be 'OK'

Scenario: Update an email notification template
	Given I have created and stored an email notification template
		| body | subject | contentType                                                                    | important | notificationType      |
		| body | test    | application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1 | true      | marain.test.template5 |
	When I use the client to send the notification template API a request to update an email notification template
		"""
		{
			"body": "this is an updated test template2",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1",
			"notificationType": "marain.test.template5"
		}
		"""
	Then the client response status code should be 'OK'

Scenario: Get an email notification template
	Given I have created and stored an email notification template
		| body | subject | contentType                                                                    | important | notificationType      |
		| body | test    | application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1 | true      | marain.test.template6 |
	When I use the client to send the notification template API a request to get a notification template with notification type 'marain.test.template6' and communication type 'Email'
	Then the client response status code should be 'OK'
	And the email template in the UserManagement API response should have a 'body' with value 'body'
	And the email template in the UserManagement API response should have a 'subject' with value 'test'
	And the email template in the UserManagement API response should have a 'contentType' with value 'application/vnd.marain.usernotifications.notificationtemplate.emailtemplate.v1'
	And the email template in the UserManagement API response should have a 'important' with value 'true'
	And the email template in the UserManagement API response should have a 'notificationType' with value 'marain.test.template6'

########################################
# Sms notification template tests	   #
########################################
Scenario: Create an sms notification template
	When I use the client to send the notification template API a request to create a new sms notification template
		"""
		{
			"body": "this is a test template",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1",
			"notificationType": "marain.test.template"
		}
		"""
	Then the client response status code should be 'OK'

Scenario: Update an sms notification template
	Given I have created and stored an sms notification template
		| body | contentType                                                                  | notificationType      |
		| body | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.template7 |
	When I use the client to send the notification template API a request to update an sms notification template
		"""
		{
			"body": "this is an updated test template2",
			"contentType": "application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1",
			"notificationType": "marain.test.template7"
		}
		"""
	Then the client response status code should be 'OK'

Scenario: Get an sms notification template
	Given I have created and stored an sms notification template
		| body | contentType                                                                  | notificationType      |
		| body | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.template8 |
	When I use the client to send the notification template API a request to get a notification template with notification type 'marain.test.template8' and communication type 'Sms'
	Then the client response status code should be 'OK'
	And the sms template in the UserManagement API response should have a 'body' with value 'body'
	And the sms template in the UserManagement API response should have a 'contentType' with value 'application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1'
	And the sms template in the UserManagement API response should have a 'notificationType' with value 'marain.test.template8'

Scenario: Request sms notification template by self link
	Given I have created and stored an sms notification template
		| body | contentType                                                                  | notificationType      |
		| body | application/vnd.marain.usernotifications.notificationtemplate.smstemplate.v1 | marain.test.template9 |
	And I use the client to send the notification template API a request to get a notification template with notification type 'marain.test.template9' and communication type 'Sms'
	When I use the client to send a management API request to get a sms notification template using the link called 'self' from the previous API response
	Then the client response status code should be 'OK'