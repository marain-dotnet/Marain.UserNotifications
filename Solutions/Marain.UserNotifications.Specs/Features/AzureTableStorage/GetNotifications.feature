@perScenarioContainer
@withUserNotificationTableStorage

Feature: Get Notifications

Background:
	Given I have created and stored 50 notifications with timestamps at 30 second intervals for the user with Id 'user1'
	And I have created and stored 5 notifications with timestamps at 30 second intervals for the user with Id 'user2'

Scenario: Retrieve notifications for a user
	When I ask the user notification store for 20 notifications for the user with Id 'user1' and call the result 'result1'
	Then the get notifications result called 'result1' should contain 20 notifications
	And the get notifications result called 'result1' should contain notifications in descending order of timestamp
	And the get notifications result called 'result1' should contain a continuation token

Scenario: Retrieve notifications for a user using a continuation token
	Given I have asked the user notification store for 20 notifications for the user with Id 'user1' and called the result 'result1'
	When I ask the user notification store for notifications for the user with Id 'user1' using the continuation token from the result called 'result1' and call the result 'result2'
	Then the get notifications result called 'result2' should contain 20 notifications
	And the get notifications result called 'result2' should contain notifications in descending order of timestamp
	And the get notifications result called 'result2' should contain a continuation token
	And the get notifications results called 'result1' and 'result2' should not contain any of the same notifications
	And the get notifications result called 'result2' should only contain notifications with an earlier timestamp than those in the get notifications result 'result1'

Scenario: Retrieve final page of notifications for a user using continuation tokens
	Given I have asked the user notification store for 20 notifications for the user with Id 'user1' and called the result 'result1'
	And I have asked the user notification store for notifications for the user with Id 'user1' using the continuation token from the result called 'result1' and call the result 'result2'
	When I ask the user notification store for notifications for the user with Id 'user1' using the continuation token from the result called 'result2' and call the result 'result3'
	Then the get notifications result called 'result3' should contain 10 notifications
	And the get notifications result called 'result3' should contain notifications in descending order of timestamp
	And the get notifications result called 'result3' should not contain a continuation token
	And the get notifications results called 'result2' and 'result3' should not contain any of the same notifications
	And the get notifications result called 'result3' should only contain notifications with an earlier timestamp than those in the get notifications result 'result2'

Scenario: Retrieve notifications since a specified previous notification
	Given I have asked the user notification store for 20 notifications for the user with Id 'user1' and called the result 'result1'
	And I have created and stored a notification for the user with Id 'user1'
	When I ask the user notification store for 20 notifications since the first notification in the results called 'result1' for the user with Id 'user1' and call the result 'result2'
	Then the get notifications result called 'result2' should contain 1 notifications
	And the get notifications result called 'result2' should not contain a continuation token
	And the get notifications results called 'result1' and 'result2' should not contain any of the same notifications
	And the get notifications result called 'result1' should only contain notifications with an earlier timestamp than those in the get notifications result 'result2'

Scenario: Retrieve all notifications for a user in a single request
	When I ask the user notification store for 20 notifications for the user with Id 'user2' and call the result 'result'
	Then the get notifications result called 'result' should contain 5 notifications
	And the get notifications result called 'result' should contain notifications in descending order of timestamp
	And the get notifications result called 'result' should not contain a continuation token
