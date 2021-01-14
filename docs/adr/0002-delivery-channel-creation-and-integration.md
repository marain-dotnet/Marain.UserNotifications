# Tenant Configuration and Routing of Notifications in the Marain.UserNotifications service

## Table of definitions

| Word               | Definition                                                                                    |
| ------------------ | --------------------------------------------------------------------------------------------- |
| Consumer           | A service that uses the Marain.UserNotifications service                                      |
| User               | The individual who is using an application that consumes the Marain.UserNotifications service |
| Delivery Channel   | A third party service used to send a notification. Eg. Twilio, Airship, Sendgrid etc.         |
| Communication Type | The mechanism of the delivery of a notification eg. Sms, Email, WebPush                       |

## Status

Proposed

## Context

Delivering notifications through different communication types (email, sms, web push notifications etc) is necessary in an extendable notifications service. Organizations should be able to choose which delivery channels (twilio, sendgrid, airship etc) they want to use for the associated communication type(s) as these said delivery channel could have different features or costs. This allows a consumer of `Marain.UserNotifications` to optimise their costs by determining the specific delivery channel for each communication type.

## Decision

Currently the `Marain.UserNotifications` service does not have any notification delivery functionality and is only used as a repository for notifications. This proposal will see the addition of delivery channel infrastructure to intergrate with third parties including but not limited to Airship, Twilio, SendGrid. Existing functionality will not be removed.

This will see a new api method being added to this service which will handle the delivery of a notification via predefined delivery channels.

```
                 +------------+
                 |            |
                 | Management |
                 | API        |
                 |            |
                 |            |
                 ++--+------+-+
                  ^  ^      ^
                  |  |      |
         +--------+  |      +--------------+
         |           |                     |
         v           v                     v
+--------+-+      +--+-------+           +-+--------+
|          |      |          |           |          |
| Delivery |      | Delivery |           | Delivery |
| Channel  |      | Channel  | +-+  +-+  | Channel  |
| 1        |      | 2        |           | N        |
|          |      |          |           |          |
|          |      |          |           |          |
+----------+      +----------+           +----------+
```

The management api should have knowledge of what delivery channels are available and which communication types each channel supports.

Configuration (api keys) for each delivery channel will be stored against the calling tenant in the property bag associated with that tenant inside the Marain Tenancy Service. All user settings and logic determining the communication types, delivery channel etc. for each notification will be handled by the consuming service.

### Delivery Channel configuration

A delivery channel will need some configuration of the underlying platform (Airship, Twilio etc) to be stored. This configuration will be used for api keys, secrets, certificates etc.

Below is an example of the configuration keys for [Airship](https://docs.airship.com/reference/security/app-keys-secrets/) that would be stored in the Keyvault.

```json
{
  "ApplicationKey": "notSoRandomApplicationKey1",
  "ApplicationSecret": "notSoRandomApplicationSecret",
  "MasterSecret": "notSoRandomMasterSecret"
}
```

The Keyvault is owned and managed by the consumer of `Marain.UserNotifications`.

### Delivery Channel usage

The `Marain.UserNotifications` service will use the routing information passed in to identify which delivery channel and communication type should be used and then extract information from the property bag to populate the appropriate template. Templates would be created for each communication type, eg, an Email Template.
