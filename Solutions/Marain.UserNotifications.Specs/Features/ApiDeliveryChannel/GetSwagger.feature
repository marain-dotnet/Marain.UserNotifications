@useApis

Feature: Get swagger
	In order to understand the API delivery channel
	As a developer
	I want to retrieve the Swagger definition of the API

Scenario: Get swagger
	When I request the Swagger definition for the API delivery channel
	Then the response status code should be 'OK'