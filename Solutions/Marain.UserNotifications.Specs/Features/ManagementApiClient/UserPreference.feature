﻿@perFeatureContainer
@useApis
@useTransientTenant

Feature: User Preferences via the client library

Scenario: Create a User Preference
	When I use the client to send a management API request to create a User Preference
	"""
		{
			"userId": "123",
			"email": "testing@test.com",
			"phoneNumber": "+44911222000000",
			"communicationChannelsPerNotificationConfiguration": 
			{
				"NotificationType1": ["Email", "Sms", "WebPush"]
			}
		}
	"""
	Then the client response status code should be 'OK'
		 
Scenario: Update a User Preference
	Given I have created and stored a user preference for a user
	| userId | email                | phoneNumber     | communicationChannelsPerNotificationConfiguration   |
	| 123    | nottesting@gmail.com | +44911222000000 | { "NotificationType1": ["Email", "Sms", "WebPush"] }|
	When I use the client to send a management API request to update a User Preference
	"""
		{
			"userId": "123",
			"email": "testing@test.com",
			"phoneNumber": "+44911222000000",
			"communicationChannelsPerNotificationConfiguration": 
			{
				"NotificationType1": ["Email", "Sms", "WebPush"]
			}
		}
	"""
	Then the client response status code should be 'OK'
	And I use the client to send a management API request to get a User Preference for userId '123'


Scenario: Get a User Preference
	Given I have created and stored a user preference for a user
	| userId | email                | phoneNumber     | communicationChannelsPerNotificationConfiguration    |
	| 123    | nottesting@gmail.com | +44911222000000 | { "NotificationType1": ["Email", "Sms", "WebPush"] } |
	When I use the client to send a management API request to get a User Preference for userId '123'
	Then the client response status code should be 'OK'
	And the user preference in the UserManagement API response should have a 'UserId' with value '123'
	And the user preference in the UserManagement API response should have a 'Email' with value 'nottesting@gmail.com'
	And the user preference in the UserManagement API response should have a 'PhoneNumber' with value '+44911222000000'
