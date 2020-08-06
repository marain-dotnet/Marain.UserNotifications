@perScenarioContainer
@withUserNotificationTableStorage

Feature: Store Notification

Scenario: Store a new notification
	Given I have a notification called 'input'
	| NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| marain.test.notification.v1 | 097C13C5-BF37-4C1F-9170-819BFC8733BC | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	When I tell the notification store to store the notification called 'input' and call the result 'result'
	Then no exception should be thrown
	And the properties of the notification called 'result' should match the notification called 'input'
	And the Id of the notification called 'result' should be set


Scenario: Attempting to store the same notification twice throws a concurrency exception
	Given I have a notification called 'input'
	| NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| marain.test.notification.v1 | 93B6A389-A40F-4807-B6EE-AC41F44A3CCD | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	And I have told the notification store to store the notification called 'input'
	When I tell the notification store to store the notification called 'input' and call the result 'result'
	Then a 'UserNotificationStoreConcurrencyException' should be thrown
