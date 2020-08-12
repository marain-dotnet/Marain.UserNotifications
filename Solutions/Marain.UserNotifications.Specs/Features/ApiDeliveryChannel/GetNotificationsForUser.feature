@perFeatureContainer
@useApis
@useTransientTenant

Feature: Get Notifications For User

Background:
	Given I have used the management API to create 25 notifications with timestamps at 30 second intervals for the user with Id 'user1'
	And I have used the management API to create 5 notifications with timestamps at 30 second intervals for the user with Id 'user2'

Scenario: Retrieve notifications for a user
	When I send an API delivery request for 10 notifications for the user with Id 'user1'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 10 entries
	And the response content should have an array property called '_embedded.items' containing 10 entries
	And the response content should have a property called '_links.self'
