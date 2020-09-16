@perFeatureContainer
@useApis
@useTransientTenant

Feature: Batch update of notification read statuses via the client library

Scenario: Set read state of a subset of notifications for a user
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user100'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user101'
	When I use the client to send a management API request to batch update the read status of the first 10 stored notifications for user 'user100' to 'Read' for the delivery channel with Id 'api'
	Then no exception should be thrown
	And the client response status code should be 'Accepted'
    And the client response should contain a 'Location' header