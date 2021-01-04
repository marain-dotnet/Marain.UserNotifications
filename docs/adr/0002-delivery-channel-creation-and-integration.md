# New delivery channel creation and integration

## Table of definitions

| Word     | Definition                                                                                                                            |
|----------|---------------------------------------------------------------------------------------------------------------------------------------|
| Consumer | A service that uses the Marain.UserNotifications service                                                                              |
| User     | The individual who is using an application that consumes the Marain.UserNotifications service                                         |

## Status

Proposed

## Context

Delivering notifications through different communication types (email, sms, web push notifications etc) is necessary in an extendable notifications service. Organizations should be able to choose which delivery channels (twilio, sendgrid, airship etc) they want to use for the associated communication type(s) as these said platforms could have different features or costs. Sometimes using different communication types through multiple delivery channels could save costs rather than having multiple communication types through a single delivery channel.

## Decision
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

The management api should have knowledge of what delivery channels are available and where certain notification types should be sent. 

When a new notification for a user is sent to the management api, the management api will read the user preference for the said notification type and see which communication types the user has 'subscribed' to. Following this, the notification delivered through the appropriate delivery channel.

### User notifications and user settings

A user should be able to choose what kind of communication types they receive based on the notification type. There are two ways of approaching this, either:
1. Send different types of notifications through all channels that are provided by the consumer of this service, and leave the communication type and user configuration for the consumer of the Marain.UserNotifications service. (ie, the consumer of the Marain.UserNotifications service should specify which delivery channels they want to send notifications via an api call when a notification is created)
2. Store user preferences somewhere that is accessible to the management api, then when a new notification with a certain notification type comes through, check what type of communication types  the user has 'subscribed' to and then send a message through the delivery channel.

The latter option was chosen as it provides a user the ability to choose what notifications they want out of the box, if the former is chosen, all users/businesses integrating with this service will then need to write their own notifications user management service.



Settings for which communication channel a user can receive a notification through will be stored inside a User Notifications Settings table which will be accessed by the Management Api. Configuration in this table will include the Notification Type and the appropriate communication types associated to that notification type. This table will have an associated crud api that will allow a consumer of `Marain.UserNotifications` to view/change the stored settings.

### Management Api and usable Delivery Channels

The management API will host configuration per tenant for what communication types can be sent through which delivery channels in the following format:

```json
[
  {
    "id": "dc1",
    "displayName": "Airship",
    "communicationTypes": ["email", "sms", "web-push"]
  }, 
  {
    "id": "dc2",
    "displayName": "Twilio",
    "communicationTypes": ["sms"]
  }
]
```
where 'communicationTypes' specifies what communication types are supported in this Delivery Channel. This allows for consumers of Marain.UserNotifications to specify if they want to only use one kind communication type from a certain Delivery Channel that supports multiple communication types, eg: an organisation using `Marain.UserNotifications` can specify that they want to use sms's from twilio and only communication types from that delivery channel.

A delivery channel will have a generic api that can be consumed by the Management Api and be able to transform data provided in the Notification into a format that is specific to that communication type (this will be discussed in a sequential adr). The result of this call will be a callback uri/null depending on how the delivery channel has been implemented.

### Delivery Channel configuration 

A delivery channel will also need the configuration of the underlying platform to be stored somewhere (api keys, secrets, etc). This will be stored in the `Marain.UserNotifications` keyvault. If a delivery channel needs to be used, there are two things that need to be added/modified:
1. Modify the `Marain.UserNotifications` key vault secrets setion with the correct key/value pair (this is described in more details below).
2. Update the configuration for the delivery channel in the management api to specify which communication types will use the delivery channel. 

Keys for the different platforms in the delivery channels will be stored in a format as follows:
`<TenantId>-<key_name>`
 an example with this format would look like:
`75b9261673c2714681f14c97bc0439fb00000000000000000000000000000000-ApiMasterSecret`

A key vault will be associated to each channel in the `Marain.UserNotifications` These key vaults will be created with the following format:
`deliverychannel-<ChannelName>`
an example with this format would be:
`deliverychannel-airship`

The Delivery Channel will also have to have it's own kind of configuration specifying the supported Communication Types. These will be hardcoded values and created when the coding of the new delivery channel is done. 

### Delivery Channel usage

The delivery channel will need to transform an object into the equivalent api model used to send a message by the chosen platform (twilio, airship, send grid etc). Some notifications will have different headings, bodys, generic templates that will have to be transformed into a format that is suitable and also might have different ways of targetting certain users depending on how the users are registered with the platforms. (eg. twilio this will be a number, for web push this could be a mixture of business id and email, etc). 

A solution to this would be to create a generic type object for each commucication type (eg. smsobject, emailobject, webpushobject) that is provided by the management api to the delivery channel, and then the delivery channel can map a generic communication type object into the required api model. This will prove to be simpler when integrating new delivery channels in the future as most of the information will already be there.