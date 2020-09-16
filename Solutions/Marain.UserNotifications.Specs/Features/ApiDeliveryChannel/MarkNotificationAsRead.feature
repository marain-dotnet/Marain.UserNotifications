@perFeatureContainer
@useApis
@useTransientTenant

Feature: Mark a notification as read

Scenario: Mark a retrieved notification as read
	Given I have created and stored a notification in the current transient tenant for the user with Id 'user100'
	And I have sent an API delivery request for notifications for the user with Id 'user100'
	When I send a request to mark a notification as read using the Url from the response property '_embedded.items._links.mark-read.href'
	Then the response status code should be 'Accepted'
    And the response should contain a 'Location' header
    And the long running operation whose Url is in the response Location header should not have a 'status' of 'NotStarted' within 10 seconds
    And the long running operation whose Url is in the response Location header should have a 'status' of 'Succeeded' within 30 seconds
	And the first notification stored in the transient tenant for the user with Id 'user200' has the read status 'Read' for the delivery channel with Id 'marain.usernotifications.deliverychannels.api'
	