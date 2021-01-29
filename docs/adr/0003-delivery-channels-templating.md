# Templating engine for different notification types

## Status

Proposed

## Context

This ADR proposes a method on how to implement custom templating when a certain notification is sent through a communication channels (SMS, email, web push notifications etc).

In a typical notification system, an object will be applied to a basic template to provide an end user with information targeting them. A lot of the basic design/layout of the template will be quite standard, but there will be places for values that will need to be inserted to customise it for the said end user. An example of this would be an birthday email notification, where a lot of the design and formatting of the email/subject will already be very well defined, and then insertion of custom data (eg. email id, name, etc ) is what will be needed by a templater.

The content of this ADR will work off themes from a previous ADR (0002-delivery-channel-creation-and-integration.md).

## Decision

A notification needs to be translated into a certain communication object (Web push, Sms, Email etc) before it can be sent via a desired delivery channel (SendGrid, Twillio, Airship, etc).

An example of this would be a web push message, where certain properties from a notification will be used to render a template that has been retrieved from a store in `Marain.UserNotifications` before being sent to the end user.

### Creation and storage of templates

A template is stored on a per tenant basis in a blob using the notification type and communication type as a filename. An example of the name of a blob would be:
`marain.lead.created.v1:WebPush`
and the object stored in the blob would look something like:

```json
{
  "body": "You have been assigned a new lead by {{assignedByUserName}}",
  "title": "New Lead",
  "contentType": "application/vnd.marain.usernotifications.notificationtemplate.webpushtemplate.v1",
  "notificationType": "marain.lead.created.v1",
  "actionUrl": "{{actionUrl}}"
}
```

Creation of templates will be flexible and dependent on the organisation that will integrate with `Marain.UserNotifications` thus every tenant will have their own set of templates for SMS, Email etc.

### Rendering templates

The Management API will be responsible to convert a create notification request into a particular template (WebPush, Email). Certain templates have required fields.

The templating engine [`Corvus.DotLiquidAsync`](https://github.com/corvus-dotnet/Corvus.DotLiquidAsync) will be used in the Management API to apply properties from the notification propertyBag into different stored communication templates (SMS, Email). This library is an extension to the [Ruby Liquid templating language](https://shopify.github.io/liquid/).

## Consequences

The above changes would allow `Marain.UserNotifications` the ability to tailor the notification message using different templates based on the notification type and communication type.
