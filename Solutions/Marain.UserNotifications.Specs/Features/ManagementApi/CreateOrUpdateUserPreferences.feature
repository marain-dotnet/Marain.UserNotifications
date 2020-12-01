@perFeatureContainer
@useApis
@useTransientTenant

Feature: User Preferences

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
