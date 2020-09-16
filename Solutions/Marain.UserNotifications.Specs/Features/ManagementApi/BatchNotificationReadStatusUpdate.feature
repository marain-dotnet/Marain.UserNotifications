@perFeatureContainer
@useApis
@useTransientTenant

Feature: Batch update of notification read statuses

Scenario: Set read state of a subset of notifications for a user
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user100'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user101'
	When I send a management API request to batch update the read status of the first 10 stored notifications for user 'user100' to 'Read' for the delivery channel with Id 'api'
	Then the response status code should be 'Accepted'
    And the response should contain a 'Location' header
    And the long running operation whose Url is in the response Location header should not have a 'status' of 'NotStarted' within 10 seconds
    And the long running operation whose Url is in the response Location header should have a 'status' of 'Succeeded' within 30 seconds
	And the first 10 notifications stored in the transient tenant for the user with Id 'user100' have the read status 'Read' for the delivery channel with Id 'api'
	And the first 10 notifications stored in the transient tenant for the user with Id 'user100' have the read status last updated set to within 60 seconds of now for the delivery channel with Id 'api'
	And the first 10 notifications stored in the transient tenant for the user with Id 'user100' have the delivery status 'Unknown' for the delivery channel with Id 'api'
	And the first 10 notifications stored in the transient tenant for the user with Id 'user100' have the delivery status last updated set to within 60 seconds of now for the delivery channel with Id 'api'

Scenario: Update read state of a subset of notifications for a user
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user200'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user201'
	And I have sent a management API request to batch update the read status of the first 10 stored notifications for user 'user200' to 'Unread' for the delivery channel with Id 'api'
    And I have waited for up to 30 seconds for the long running operation whose Url is in the response Location header to have a 'status' of 'Succeeded'
	When I send a management API request to batch update the read status of the first 5 stored notifications for user 'user200' to 'Read' for the delivery channel with Id 'api'
	Then the response status code should be 'Accepted'
    And the response should contain a 'Location' header
    And the long running operation whose Url is in the response Location header should not have a 'status' of 'NotStarted' within 10 seconds
    And the long running operation whose Url is in the response Location header should have a 'status' of 'Succeeded' within 30 seconds
	And the first 5 notifications stored in the transient tenant for the user with Id 'user200' have the read status 'Read' for the delivery channel with Id 'api'
