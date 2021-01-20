# How to add a new delivery channel

## Status

Proposed

## Context

Currently there is no documented process on how to create new delivery channels and new communication channels. Going forward we anticipate this service will be expanded to include many new ways of delivering notifications. This document will describe the process to achieve this.

## Decision

A recommended approach to adding a new delivery channel is as follows:

1. Research the interface provided by the chosen delivery platform
   - Analyse the required fields for authenticating a request to the platform
   - Analyse the object fields for the request content (delivery details, eg mobile number, contents)
2. Create a new entry in the shared Azure Keyvault that contains the secrets for the platform (eg. api keys). Any changes to this will be handled by the consumer `Marain.UserNotifications` as part of tenant configuration.
3. Compare the required request content in the new platform against the existing templates for the communication type(s) being added/used via this platform and make the required adjustments. (This may only require a single field in a single existing template or may require the creation of entirely new template objects)
4. Create a new class that inherits from `IDeliveryChannel` and create the new methods required to successfully send a notification following a similar pattern to the pre-existing methods used for other delivery channels in the Marain.UserNotifications.ThirdParty.DeliveryChannelshow.
5. Add the new delivery channel into the routing engine (which takes place in the `DispatchNotificationActivity` activity).
