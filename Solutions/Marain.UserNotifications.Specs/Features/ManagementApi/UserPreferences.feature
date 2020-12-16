@perFeatureContainer
@useApis
@useTransientTenant
Feature: User Preference

Scenario: Create a user prefence for a user
	When I send the user preference API a request to create a new user preference
		"""
		{
		    "userId": "1",
		    "email": "test@test.com",
		    "phoneNumber": "",
		    "communicationChannelsPerNotificationConfiguration": 
		    {
		        "notificationtype1": ["Sms", "Email"],
		        "notificationtype2": ["Sms", "WebPush"]
		    },
		}
		"""
	Then the response status code should be 'OK'

Scenario: Get a user preference for a certain user
	Given I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 2      | test@test.com | 041532211   | {"notificationType1": ["email", "sms"]}           |
	When I send a user preference API request to retreive a user preference
	Then the response status code should be 'OK'
	And the response content should have a property called '_links.self'
	And the response content should have a property called 'eTag'
	And the response content should have a string property called 'userId' with value '2'
	And the response content should have a string property called 'email' with value 'test@test.com'
	And the response content should have a string property called 'phoneNumber' with value '041532211'

Scenario: Update a user preference for a user by multiple requests handling concurency issues
	Given I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 3      | test@test.com | 041532211   | {"notificationType1": ["email", "sms"]}           |
	When I send a user preference API request to update a previously saved user preference
		| userId | email            | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 3      | testing@test.com | 0987654321  | {"notificationType1": ["email", "sms"]}           |
	And I send a user preference API request to retreive a user preference
	Then the response status code should be 'OK'
	And the response content should have a property called '_links.self'
	And the response content should have a property called 'eTag'
	And the response content should have a string property called 'userId' with value '3'
	And the response content should have a string property called 'email' with value 'testing@test.com'
	And the response content should have a string property called 'phoneNumber' with value '0987654321'

Scenario: Update a user preference for a user by multiple requests handling concurency issues having invalid etag
	Given I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 4      | test@test.com | 041532211   | {"notificationType1": ["email", "sms"]}           |
	When I send a user preference API request to update a previously saved user preference that has an invalid etag in the request body
		| userId | email            | phoneNumber | communicationChannelsPerNotificationConfiguration | eTag                    |
		| 4      | testing@test.com | 0987654321  | {"notificationType1": ["email", "sms"]}           | "\"0x8D89CF9D612C7F1\"" |
	Then the response status code should be 'BadRequest'

Scenario: Update a user preference for a user by multiple requests handling concurency issues having no etag
	Given I have created and stored a user preference for a user
		| userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 5      | test@test.com | 041532211   | {"notificationType1": ["email", "sms"]}           |
	When I send a user preference API request to update a previously saved user preference that has no etag in the request body
		| userId | email            | phoneNumber | communicationChannelsPerNotificationConfiguration |
		| 5      | testing@test.com | 0987654321  | {"notificationType1": ["email", "sms"]}           |
	Then the response status code should be 'InternalServerError'