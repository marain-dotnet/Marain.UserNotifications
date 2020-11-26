@perFeatureContainer
@useApis
@useTransientTenant

Feature: Create User Preferences

Scenario: Create a user prefence for a user
    When I send a user prefence API a request to create a new user preference
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

Scenario: Get a user preference for a user
    Given I have created and stored a user preference for user
    | userId | email         | phoneNumber | communicationChannelsPerNotificationConfiguration |
    | 1      | test@test.com | 041532211   | {"notificationType1": ["email", "sms"]}           |
    When I send a user preference API request to retreive a user preference 
    Then the response status code should be 'OK'
    And the response content should have a string property called 'userId' with value '1'
    And the response content should have a string property called 'email' with value 'test@test.com'
    And the response content should have a string property called 'phoneNumber' with value '041532211'
