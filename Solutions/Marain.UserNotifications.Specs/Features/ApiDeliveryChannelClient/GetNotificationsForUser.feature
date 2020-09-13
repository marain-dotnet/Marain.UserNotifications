@perFeatureContainer
@useApis
@useTransientTenant

Feature: Get Notifications For User via the client library

# IMPORTANT: Because all the setup/teardown is per feature, every spec in this feature that creates data for the scenario
# should use a different user Id to avoid conflicts.

Scenario: Request notifications for a user
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user100'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user101'
	When I use the client to send an API delivery request for 10 notifications for the user with Id 'user100'
	Then no exception should be thrown
	Then the client response status code should be 'OK'
	And the paged list of notifications in the API delivery channel response should contain 10 item links
	And the paged list of notifications in the API delivery channel response should contain 10 embedded items
	And the paged list of notifications in the API delivery channel response should have a 'self' link
	And the paged list of notifications in the API delivery channel response should have a 'next' link
	And the paged list of notifications in the API delivery channel response should have a 'newer' link

Scenario: Request notifications for a user using a continuation token from a previous request
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user250'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user251'
	And I have used the client to send an API delivery request for 10 notifications for the user with Id 'user250'	
	When I use the client to send an API delivery request for a paged list of notifications using the link called 'next' from the previous API delivery channel response
	Then no exception should be thrown
	Then the client response status code should be 'OK'
	And the paged list of notifications in the API delivery channel response should contain 10 item links
	And the paged list of notifications in the API delivery channel response should contain 10 embedded items
	And the paged list of notifications in the API delivery channel response should have a 'self' link
	And the paged list of notifications in the API delivery channel response should have a 'next' link
	And the paged list of notifications in the API delivery channel response should have a 'newer' link

Scenario: Request notifications with an invalid tenant Id
	When I use the client to send an API delivery request with an non-existent tenant Id for notifications for the user with Id 'user100'
	Then a 'UserNotificationsApiException' should be thrown
	And the UserNotificationsApiException status code should be 'NotFound'

Scenario Outline: Request notifications with an invalid maximum number of items
	When I use the client to send an API delivery request for <MaxItems> notifications for the user with Id 'user200'
	Then a 'UserNotificationsApiException' should be thrown
	And the UserNotificationsApiException status code should be 'BadRequest'

	Examples:
	| MaxItems |
	| 0        |
	| 101      |
	| 1000     |
	| -1       |

Scenario: Request notifications for a user without specifying the maximum number of items to return
	Given I have created and stored 60 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user150'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user151'
	When I use the client to send an API delivery request for notifications for the user with Id 'user150'
	Then no exception should be thrown
	Then the client response status code should be 'OK'
	And the paged list of notifications in the API delivery channel response should contain 50 item links
	And the paged list of notifications in the API delivery channel response should contain 50 embedded items

Scenario: Request more notifications for a user than exist
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user200'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user201'
	When I use the client to send an API delivery request for 100 notifications for the user with Id 'user200'
	Then the client response status code should be 'OK'
	And the paged list of notifications in the API delivery channel response should contain 25 item links
	And the paged list of notifications in the API delivery channel response should contain 25 embedded items

Scenario: Retrieve notifications for a user where none exist
	When I use the client to send an API delivery request for 100 notifications for the user with Id 'userXXXXX'
	Then the client response status code should be 'OK'
	And the paged list of notifications in the API delivery channel response should contain 0 item links
	And the paged list of notifications in the API delivery channel response should contain 0 embedded items

#Scenario: Request notifications for a user using a continuation token from a previous request
