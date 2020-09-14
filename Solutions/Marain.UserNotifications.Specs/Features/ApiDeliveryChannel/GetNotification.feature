@perFeatureContainer
@useApis
@useTransientTenant

Feature: Get Single Notification by ID

Scenario: Retrieve a notification by its Id
	Given I have created and stored a notification in the current transient tenant and called the result 'output'
	| NotificationType            | UserId                               | Timestamp         | PropertiesJson      | CorrelationIds |
	| marain.test.notification.v1 | 304ABC0E-08AF-4EF5-A9AC-281B67D633F4 | 2012-03-19T07:22Z | { "prop1": "val1" } | ["id1", "id2"] |
	And I send an API delivery request for the user notification with the same Id as the user notification called 'output'
	Then the response status code should be 'OK'
	And the response content should have a property called '_links.self'
	And the response content should have a string property called 'userId' with value '304ABC0E-08AF-4EF5-A9AC-281B67D633F4'
	And the response content should have a string property called 'notificationType' with value 'marain.test.notification.v1'
	And the response content should have a date-time property called 'timestamp' with value '2012-03-19T07:22Z'
	And the response content should have a string property called 'properties.prop1' with value 'val1'

Scenario: Request a notification that doesn't exist
	When I send an API delivery request for the user notification with the Id 'eyJwYXJ0aXRpb25LZXkiOiJ1c2VyMiIsInJvd0tleSI6IjAwOTIyMzM3MDQ0MTQ5MDU2NzgwNy1kWE5sY2pJeE5UazFNelkwTWpBNE1EQXdhbTl1TG5SbGMzUXlMakY3SW5Sb2FXNW5NU0k2SW5aaGJIVmxNU0lzSW5Sb2FXNW5NaUk2SW5aaGJIVmxNaUo5In0='
	Then the response status code should be 'NotFound'

Scenario: Request a notification with an invalid ID
	When I send an API delivery request for the user notification with the Id 'BadId'
	Then the response status code should be 'BadRequest'
