@perScenarioContainer
@withUserNotificationTableStorage

Feature: Store Notification

Scenario: Store a new user notification
	Given I have a user notification called 'input'
	| NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| marain.test.notification.v1 | 097C13C5-BF37-4C1F-9170-819BFC8733BC | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	When I tell the user notification store to store the user notification called 'input' and call the result 'result'
	Then no exception should be thrown
	And the properties of the user notification called 'result' should match the user notification called 'input'
	And the Id of the user notification called 'result' should be set


Scenario: Attempting to store the same notification twice throws a concurrency exception
	Given I have a user notification called 'input'
	| NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| marain.test.notification.v1 | 93B6A389-A40F-4807-B6EE-AC41F44A3CCD | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	And I have told the user notification store to store the user notification called 'input' and call the result 'result'
	When I tell the user notification store to store the user notification called 'input' and call the result 'result'
	Then a 'UserNotificationStoreConcurrencyException' should be thrown

Scenario: Attempting to store equivalent notifications throws a concurrency exception
	Given I have user notifications
	| Name   | NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| input1 | marain.test.notification.v1 | F88677CE-8A9B-41FC-9E75-A8F71C9470C8 | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	| input2 | marain.test.notification.v1 | F88677CE-8A9B-41FC-9E75-A8F71C9470C8 | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id3", "id4"] |
	And I have told the user notification store to store the user notification called 'input1' and call the result 'result'
	When I tell the user notification store to store the user notification called 'input2' and call the result 'result'
	Then a 'UserNotificationStoreConcurrencyException' should be thrown
