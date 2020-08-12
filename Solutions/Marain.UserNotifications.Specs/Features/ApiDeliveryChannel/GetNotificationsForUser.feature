@perFeatureContainer
@useApis
@useTransientTenant

Feature: Get Notifications For User

Background:
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user1'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user2'

Scenario: Retrieve notifications for a user
	When I send an API delivery request for 10 notifications for the user with Id 'user1'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 10 entries
	And the response content should have an array property called '_embedded.items' containing 10 entries
	And the response content should have a property called '_links.self'
	And the response content should have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Request more notifications for a user than exist
	When I send an API delivery request for 100 notifications for the user with Id 'user1'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 25 entries
	And the response content should have an array property called '_embedded.items' containing 25 entries
	And the response content should have a property called '_links.self'
	And the response content should not have a property called '_links.next'
	And the response content should have a property called '_links.newer'
