@perFeatureContainer
@useApis
@useTransientTenant

Feature: Get Notifications For User

Scenario: Request notifications for a user
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user100'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user101'
	When I send an API delivery request for 10 notifications for the user with Id 'user100'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 10 entries
	And the response content should have an array property called '_embedded.items' containing 10 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Request notifications for a user without specifying the maximum number of items to return
	Given I have created and stored 60 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user150'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user151'
	When I send an API delivery request for notifications for the user with Id 'user150'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 50 entries
	And the response content should have an array property called '_embedded.items' containing 50 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Request more notifications for a user than exist
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user200'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user201'
	When I send an API delivery request for 100 notifications for the user with Id 'user200'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 25 entries
	And the response content should have an array property called '_embedded.items' containing 25 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should not have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Retrieve notifications for a user where none exist
	When I send an API delivery request for 10 notifications for the user with Id 'userXXXXXX'
	Then the response status code should be 'OK'
	And the response content should not have a property called '_links.items'
	And the response content should not have a property called '_embedded.items'
	And the response content should have a property called '_links.self'
	And the response content should not have a property called '_links.next'
	And the response content should not have a property called '_links.newer'

Scenario: Request notifications for a user using a continuation token from a previous request
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user250'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user251'
	And I have sent an API delivery request for 10 notifications for the user with Id 'user100'	
	And I have stored the value of the response object property called '_links.next.href' as 'nextLink'
	When I send an API delivery request using the path called 'nextLink'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 10 entries
	And the response content should have an array property called '_embedded.items' containing 10 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Request the final page of notifications for a user using continuation tokens from previous requests
	Given I have created and stored 25 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user250'
	And I have created and stored 5 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user251'
	And I have sent an API delivery request for 10 notifications for the user with Id 'user100'	
	And I have stored the value of the response object property called '_links.next.href' as 'nextLink'
	And I have sent an API delivery request using the path called 'nextLink'
	And I have stored the value of the response object property called '_links.next.href' as 'nextLink'
	When I send an API delivery request using the path called 'nextLink'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 5 entries
	And the response content should have an array property called '_embedded.items' containing 5 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should not have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Request notifications for a user since a previously retrieved notification when there are no new notifications
	Given I have created and stored 10 notifications in the current transient tenant with timestamps at 30 second intervals for the user with Id 'user300'
	And I have sent an API delivery request for 10 notifications for the user with Id 'user300'	
	And I have stored the value of the response object property called '_links.newer.href' as 'newerLink'
	When I send an API delivery request using the path called 'newerLink'
	Then the response status code should be 'OK'
	And the response content should not have a property called '_links.items'
	And the response content should not have a property called '_embedded.items'
	And the response content should have a property called '_links.self'
	And the response content should not have a property called '_links.next'
	And the response content should not have a property called '_links.newer'

Scenario: Request notifications for a user since a previously retrieved notification when there is a single page of new notifications
	Given I have created and stored 10 notifications in the current transient tenant with timestamps at 300 second intervals for the user with Id 'user300'
	And I have sent an API delivery request for 10 notifications for the user with Id 'user300'	
	And I have stored the value of the response object property called '_links.newer.href' as 'newerLink'
	And I have created and stored 3 notifications in the current transient tenant with timestamps at 1 second intervals for the user with Id 'user300'
	When I send an API delivery request using the path called 'newerLink'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 3 entries
	And the response content should have an array property called '_embedded.items' containing 3 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should not have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Request notifications for a user since a previously retrieved notification when there are multiple pages of new notifications
	Given I have created and stored 10 notifications in the current transient tenant with timestamps at 300 second intervals for the user with Id 'user300'
	And I have sent an API delivery request for 10 notifications for the user with Id 'user300'	
	And I have stored the value of the response object property called '_links.newer.href' as 'newerLink'
	And I have created and stored 15 notifications in the current transient tenant with timestamps at 1 second intervals for the user with Id 'user300'
	When I send an API delivery request using the path called 'newerLink'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 10 entries
	And the response content should have an array property called '_embedded.items' containing 10 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should have a property called '_links.next'
	And the response content should have a property called '_links.newer'

Scenario: Request a second page of notifications for a user since a previously retrieved notification
	Given I have created and stored 10 notifications in the current transient tenant with timestamps at 300 second intervals for the user with Id 'user300'
	And I have sent an API delivery request for 10 notifications for the user with Id 'user300'	
	And I have stored the value of the response object property called '_links.newer.href' as 'newerLink'
	And I have created and stored 15 notifications in the current transient tenant with timestamps at 1 second intervals for the user with Id 'user300'
	And I have sent an API delivery request using the path called 'newerLink'
	And I have stored the value of the response object property called '_links.next.href' as 'nextLink'
	When I send an API delivery request using the path called 'nextLink'
	Then the response status code should be 'OK'
	And the response content should have an array property called '_links.items' containing 5 entries
	And the response content should have an array property called '_embedded.items' containing 5 entries
	And each item in the response content array property called '_embedded.items' should have a property called 'userId'
	And each item in the response content array property called '_embedded.items' should have a property called 'notificationType'
	And each item in the response content array property called '_embedded.items' should have a property called 'properties'
	And each item in the response content array property called '_embedded.items' should have a property called 'timestamp'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.self'
	And each item in the response content array property called '_embedded.items' should have a property called '_links.mark-read'
	And the response content should have a property called '_links.self'
	And the response content should not have a property called '_links.next'
	And the response content should have a property called '_links.newer'
