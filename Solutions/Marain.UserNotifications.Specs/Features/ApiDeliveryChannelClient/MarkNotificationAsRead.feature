@perFeatureContainer
@useApis
@useTransientTenant

Feature: Mark a notification as read

Scenario: Mark a retrieved notification as read
	Given I have created and stored a notification in the current transient tenant for the user with Id 'user100'
	And I have used the client to send an API delivery request for 1 notification for the user with Id 'user100'
	When I use the client to send a request to mark a notification as read using the Url from the notififcation in the client response
	Then no exception should be thrown
	And the client response status code should be 'Accepted'
    And the client response should contain a 'Location' header
	