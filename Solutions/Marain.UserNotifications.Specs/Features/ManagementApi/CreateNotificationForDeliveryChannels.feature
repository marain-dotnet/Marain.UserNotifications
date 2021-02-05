@perFeatureContainer
@useApis
@useTransientTenant
Feature: Create Notification For Third Party Delivery Channels

Scenario: Create a web push notification for a single user
	When I send a management API request to create a new notification via third party delivery channels
		"""
        {
            "notificationType": "marain.notifications.test.v1",
            "timestamp": "2020-07-21T17:32:28Z",
            "userIds": [
                "user1"
            ],
            "deliveryChannelConfiguredPerCommunicationType": {
                "webPush": "application/vnd.marain.usernotifications.deliverychannel.airship.v1"
            },
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "thing1": "value1",
                "thing2": "value2"
            }
        }
		"""
	Then the response status code should be 'Accepted'
	And the response should contain a 'Location' header
	And the long running operation whose Url is in the response Location header should not have a 'status' of 'NotStarted' within 10 seconds
	And the long running operation whose Url is in the response Location header should have a 'status' of 'Succeeded' within 30 seconds

Scenario: Create web push notifications for multiple users
	When I send a management API request to create a new notification via third party delivery channels
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
            "deliveryChannelConfiguredPerCommunicationType": {
                "webPush": "application/vnd.marain.usernotifications.deliverychannel.airship.v1"
            },
            "correlationIds": ["cid1", "cid2"],
            "properties": {
                "thing1": "value1",
                "thing2": "value2"
            }
        }
		"""
	Then the response status code should be 'Accepted'
	And the response should contain a 'Location' header
	And the long running operation whose Url is in the response Location header should not have a 'status' of 'NotStarted' within 10 seconds
	And the long running operation whose Url is in the response Location header should have a 'status' of 'Succeeded' within 30 seconds