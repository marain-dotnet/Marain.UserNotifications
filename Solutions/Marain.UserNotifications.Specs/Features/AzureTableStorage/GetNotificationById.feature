@perScenarioContainer
@withUserNotificationTableStorage

Feature: Get Notification By Id

Scenario: Retrieve a notification by its Id
	Given I have a user notification called 'input'
	| NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| marain.test.notification.v1 | 304ABC0E-08AF-4EF5-A9AC-281B67D633F4 | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	And I have told the user notification store to store the user notification called 'input' and call the result 'output'
	When I ask the user notification store for the user notification with the same Id as the user notification called 'output' and call it 'result'
	Then the notification called 'result' should be the same as the notification called 'output'
