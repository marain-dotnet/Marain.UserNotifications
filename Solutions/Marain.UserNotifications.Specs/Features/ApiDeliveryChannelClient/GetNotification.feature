@perFeatureContainer
@useApis
@useTransientTenant

Feature: Get Single Notification by ID via the client library

Scenario: Retrieve a notification by its Id
	Given I have created and stored a notification in the current transient tenant and called the result 'output'
	| NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| marain.test.notification.v1 | 304ABC0E-08AF-4EF5-A9AC-281B67D633F4 | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	And I use the client to send an API delivery request for the user notification with the same Id as the user notification called 'output'
	Then no exception should be thrown
	And the client response status code should be 'OK'
	And the notification in the API delivery channel response should have a 'self' link
	And the notification in the API delivery channel response should have a Notification Type of 'marain.test.notification.v1'
	And the notification in the API delivery channel response should have a User Id of '304ABC0E-08AF-4EF5-A9AC-281B67D633F4'
	And the notification in the API delivery channel response should have a Timestamp of '2012-03-19T07:22Z'
	And the notification in the API delivery channel response should have a Delivery Status of 'true'
	And the notification in the API delivery channel response should have a Read Status of 'false'

Scenario: Request a notification that doesn't exist
	When I use the client to send an API delivery request for the user notification with the Id 'eyJwYXJ0aXRpb25LZXkiOiJ1c2VyMiIsInJvd0tleSI6IjAwOTIyMzM3MDQ0MTQ5MDU2NzgwNy1kWE5sY2pJeE5UazFNelkwTWpBNE1EQXdhbTl1TG5SbGMzUXlMakY3SW5Sb2FXNW5NU0k2SW5aaGJIVmxNU0lzSW5Sb2FXNW5NaUk2SW5aaGJIVmxNaUo5In0='
	Then a 'UserNotificationsApiException' should be thrown
	And the UserNotificationsApiException status code should be 'NotFound'

Scenario: Request a notification with an invalid ID
	When I use the client to send an API delivery request for the user notification with the Id 'BadId'
	Then a 'UserNotificationsApiException' should be thrown
	And the UserNotificationsApiException status code should be 'BadRequest'
