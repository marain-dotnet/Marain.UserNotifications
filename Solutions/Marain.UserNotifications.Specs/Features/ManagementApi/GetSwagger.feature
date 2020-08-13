@perFeatureContainer
@useApis

Feature: Get swagger
	In order to understand the management API
	As a developer
	I want to retrieve the Swagger definition of the API

Scenario: Get swagger
	When I request the Swagger definition for the management API
	Then the response status code should be 'OK'