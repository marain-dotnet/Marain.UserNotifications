# Templating engine for different notification types

## Status

Proposed

## Context

This ADR proposes a method on how to implement custom templating when a certain notification is sent through a communication channels (SMS, email, web push notifications etc).

In a typical notification system, an object will be applied to a basic template to provide an end user with information targeting them. A lot of the basic design/layout of the template will be quite standard, but there will be places for values that will need to be inserted to customise it for the said end user. An example of this would be an birthday email notification, where a lot of the design and formatting of the email/subject will already be very well defined, and then insertion of custom data (eg. email id, name, etc ) is what will be needed by a templater.

The content of this ADR will work off themes from the previous ADR (0002-delivery-channel-creation-and-integration.md).

## Decision

A notification having necessary information needs to be translated into a certain communication object (SMS, Email, etc) before it can be sent via the desired delivery channel (SendGrid, Twillio, Airship).

An example of this would be a SMS, which needs the essential information to be available for it to be converted into an SMS object before being sent to the end user.

```json

    public class SMS {
        public string ToPhoneNumber { get; set; }
        public string Body { get; set; }
    }

```

Each field in the communication object will needs it's own template (property) for the associated notification type. A template for the `ToPhoneNumber` property in the above example would look something like:

```
{{ notification.propertyBag.ToPhoneNumber }}
```

### Creation of templates, how does someone create templates and in what language

A template will be created based on the notification type, the communication type, and the communication object's property key.

A few examples of this would be:

```
(Notification type) -> (Communication type) -> (Communication type property)

New Lead            -> SMS                  -> ToPhoneNumber
New Lead            -> SMS                  -> FromPhoneNumber
New Lead            -> SMS                  -> Body

New Lead            -> Email                -> ToEmail
New Lead            -> Email                -> Subject
New Lead            -> Email                -> Body
```

Every communication type will have a similar communication object, ie, an SMS will use an SMSobject that has certain set fields. Some of those fields would be required without which a notification would be impossible to be sent. Eg. A SMSObject should have `ToPhoneNumber` or EmailObject should have `ToEmail` and without these properties being populated a notification could not be sent. Furthermore, the `Body` for these 2 communication object would be equally important as there would be no point in sending an empty notification.

Creation of templates can be totally flexible and dependent on the organisation which will integrate with `Marain.UserNotifications` thus every tenant will have their own set of templates for SMS and Email etc. Default templates will be provided at the start for the communication types (SMS, email, web push notifications) at start but can be modified and updated which will leave space for the organisation to configure as per their need.

### Storage of templates

Templates need to be stored in such a way where it can be accessed and modified easily on the fly. Thus to have this flexibilty templates will be stored in azure table storage. The template for individual properties (`ToPhoneNumber`, `FromPhoneNumber`, `ToEmail`) would have the following column headers:

- Notification Type
- Communication Type
- Property Name (This will be the property name for the the communication object)
- Template

Eg. The below example shows how the table storage will host the relevant templates mentioned above having a template for every single property name per communication type and notification type.

```
Notification Type       Communication Type         Property Name        Template

New Lead                SMS                        ToPhoneNumber        {{toPhoneNumber}}

New Lead                SMS                        Body                 Hi {{endUserName}},
                                                                        You have been assigned a new lead from {{fromUserName}}.

New Lead                Email                      ToEmail              {{toEmail}}

New Lead                Email                      Body                 Hi {{endUserName}},
                                                                        You have been assigned a new lead from {{fromUserName}} having name {{leadName}}.

```

### Applying a certain template to a property for a communication object

The Management API will be responsible to convert the create notification request into a particular communication object (SMSObject) based on the configured user preferences for the end user. Every communication object has certain required properties and without which a notification cannot be sent as explained in the section above. If we arrive in such a scenario where the essential properties are missing then those can be logged in App Insights detailing which property name was missing for a notification type and communication type and who was the end user who triggered this notification.

The templating engine [`Corvus.DotLiquidAsync`](https://github.com/corvus-dotnet/Corvus.DotLiquidAsync) will be used in the Management API to apply properties from the notification propertyBag into a template before being mapped into the communication objects (SMS, Email). This library is an extension to the [Ruby Liquid templating language](https://shopify.github.io/liquid/).

## Consequences

At start we would be integrating with Airship to be the delivery channel for Push Notifications and thus all notifications would be sent through that delivery channels to the end user. As support for more delivery channels would be made available, further configuration might be required to make the business decision to decide which delivery channel should be default choosen for what particular communication type (SMS, Email, Push Notification).
