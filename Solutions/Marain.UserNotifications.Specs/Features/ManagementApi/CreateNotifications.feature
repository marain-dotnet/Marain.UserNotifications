@perFeatureContainer
@useManagementApi
@useTransientTenant

Feature: Create Notifications

Scenario: Create a notification for a single user
	When I send a request to create a new notification:
		"""
        {
            "notificationType": "marain.notifications.test.v1",
            "timestamp": "2020-07-21T17:32:28Z",
            "userIds": [
                "user1"
            ],
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "thing1": "value1",
                "thing2": "value2"
            }
        }
        """
	Then the response status code should be 'Accepted'
    And the response should contain a 'Location' header
    And the long running operation whose Url is in the response Location header should not have a 'status' of 'NotStarted' within 5 seconds
    And the long running operation whose Url is in the response Location header should have a 'status' of 'Succeeded' within 15 seconds

Scenario: Create notifications for multiple users
	When I send a request to create a new notification:
		"""
        {
            "notificationType": "marain.notifications.test.v1",
            "timestamp": "2020-07-21T17:32:28Z",
            "userIds": [
                "user2",
                "user3",
                "user4",
                "user5",
                "user6",
                "user7"
            ],
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "thing1": "value1",
                "thing2": "value2"
            }
        }
        """
	Then the response status code should be 'Accepted'
    And the response should contain a 'Location' header
    And the long running operation whose Url is in the response Location header should not have a 'status' of 'NotStarted' within 5 seconds
    And the long running operation whose Url is in the response Location header should have a 'status' of 'Succeeded' within 15 seconds
