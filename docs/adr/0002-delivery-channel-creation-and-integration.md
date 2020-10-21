# New delivery channel creation and integration

## Status

Proposed

## Context

Delivering notifications through different communication types (email, sms, web push notifications etc) is necessary in an extendable notifications service. Users should be able to choose which delivery channels (twilio, sendgrid, airship etc) they want to use for the associated communication type(s) as these said platforms could have different features or costs and if combined could provide less setup configuration/cost, which leaves the configuration totally up to the end user. 

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

The management api should have knowledge of what delivery channels are available and where certain notification types should be sent. As we send notifications through on a per user per tenant basis, user settings for which communication type(s) a user has allowed is read and a notification is sent to the available delivery channel(s). 

### User notifications and user settings

A user should be able to choose what kind of communication types they receive based on the notification type. There are two ways of approaching this, either:
1. Send different types of notifications through all channels that are provided by the consumer of this service, and leave the communication type and user configuration for the consumer of the Marain.UserNotifications service. (ie, the consumer of the Marain.UserNotifications service should specify which channels they want to send notifications via an api call when a notification is created)
2. Store user preferences somewhere that is accessible to the management api, then when a new type of notifications comes through, check what type of communications types  the user has agreed to use and then send a message through the delivery channel .

The latter option was chosen as it provides a user the ability to choose what notifications they want out of the box, if the former is chosen, all users/businesses integrating with this service will then need to write their own notifications user management service.

User settings for which notifications will go to what communication channels for each user will be stored inside a User Notifications Settings table which will be accessed by the Management Api. Configuration in this table will include the Notification Type and the appropriate communication types associated to that notification type. This table will have an associated crud api that will allow a user to view/change the stored settings.

### Delivery Channels and Notification Types

A delivery channel will need two types of setup, the first being the integration of the service with Marain and the second being configuration added in by a user to how this channel will be used in the current implementation (which notification types are permitted to which channels, templating, api keys etc).

The management API will host configuration for what notification types can be sent through which delivery channels in the following format:

```json
[
  {
    "id": "",
    "displayName": "",
    "communicationTypes": [""],
    "isConfigured": false
  }
]
```
where 'communicationTypes' specifies what communication types are supported in this Delivery Channel. This allows for consumers of Marain.UserNotifications to specify if they want to only use one kind communication type from a certain Delivery Channel that supports multiple types.

A delivery channel will have a generic api that can be consumed by the Management Api and be able to transform data provided in the Notification into a format that is specific to that communication type (this will be discussed in a sequential adr). The result of this call will be a callback uri/null depending on how the delivery channel has been implemented.

### Delivery Channel configuration 

A delivery channel will also need the configuration of the underlying platform to be stored somewhere (api keys, secrets, etc). This will be stored in a generic manner so if a supported delivery channel needs to be used, an api key and slight modification of the settings in the management api will only be required. 

Keys for the different platforms in the delivery channels will be stored in a format as follows:
- `Marain:DeliveryChannel:<Platform_name>:<key_name>`
- `Marain:DeliveryChannel:Airship:ApiMasterSecret`

The Delivery Channel will also have to have it's own kind of configuration specifying the supported Communication Types. These will be hardcoded values and created when the coding of the new delivery channel is done. 

### Delivery Channel usage

The delivery channel will need to transform an object into the equivalent api model  used to send a message by the chosen platform (twilio, airship, send grid etc). Some notifications will have different headings, bodys, generic templates that will have to be transformed into a format that is suitable and also might have different ways of targetting certain users depending on how the users are registered with the platforms. (for twilio this will be a number, for web push this could be a mixture of business id and email, etc). 

A solution to this would be to create a generic communication type object (smsobject, emailobject, webpushobject) that is provided by the management api to the delivery channel, and then the delivery channel can map a generic communication type object into the required api model. This might prove to be simpler when integrating new delivery channels in the future as most of the information will already be there.