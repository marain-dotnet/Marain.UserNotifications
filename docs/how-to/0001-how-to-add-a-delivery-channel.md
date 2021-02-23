# How to add a new delivery channel

Currently there is no documented process on how to create new delivery channels and new communication channels. Going forward we anticipate this service will be expanded to include many new ways of delivering notifications. This document will describe the process to achieve this.

## Recommended steps to the add a new delivery channel

1. Research the interface provided by the chosen delivery platform

   - Analyse the required fields for authenticating a request to the platform
   - Analyse the object fields for the request content (delivery details, eg mobile number, contents)

2. Create a new entry in the shared Azure Keyvault that contains the secrets for the platform (eg. api keys). Any changes to this will be handled by the consumer `Marain.UserNotifications` as part of tenant configuration. The structure of the Airship keys is like below which is in the `Marain.UserNotifications.ThirdParty.DeliveryChannels.KeyVaultSecretModels`.

```csharp
public class Airship
{
   public string? ApplicationKey { get; set; }
   public string? ApplicationSecret { get; set; }
   public string? MasterSecret { get; set; }
}
```

3. Compare the required request content in the new platform against the existing templates for the communication type(s) being added/used via this platform and make the required adjustments. (This may only require a single field in a single existing template or may require the creation of entirely new template objects)

```csharp
public class WebPushTemplate
{
   public string NotificationType { get; }
   public string? ETag { get; set; }
   public string Body { get; }
   public string Title { get; }
   public string? Image { get; }
   public string? ActionUrl { get; }
}
```

4. Create a new class that inherits from `IDeliveryChannel` in the `Marain.UserNotifications.ThirdParty.DeliveryChannels` project that will be responsible to be the structure of all the properties being passed into the integrated delivery channel.

```csharp
public class AirshipDeliveryChannel : IDeliveryChannel
{
   /// <summary>
   /// The content type that will be used when serializing/deserializing.
   /// </summary>
   private const string RegisteredContentType = "application/vnd.marain.usernotifications.thirdparty.deliverychannels.airship.v1";
   /// <summary>
   /// Initilises the <see cref="AirshipDeliveryChannel"/> with predefined settings.
   /// </summary>
   /// <param name="title">The title of the Airship notification.</param>
   /// <param name="body">The body of the notification.</param>
   /// <param name="actionUrl">The action url where the user should be navigated on click of the notification.</param>
   public AirshipDeliveryChannel(
       string title,
       string body,
       string actionUrl)
   {
       this.Title = title;
       this.Body = body;
       this.ActionUrl = actionUrl;
   }
   /// <summary>
   /// Gets the registered content type used when this object is serialized/deserialized.
   /// </summary>
   public string ContentType => RegisteredContentType;
   /// <summary>
   /// Gets the title of the notification.
   /// </summary>
   public string? Title { get; }
   /// <summary>
   /// Gets the body of the notification.
   /// </summary>
   public string? Body { get; }
   /// <summary>
   /// Gets the action url where the user should be navigated on click of the notification.
   /// </summary>
   public string? ActionUrl { get; }
   }
```

5. Create the new methods required to successfully send a notification following a similar pattern to the pre-existing methods used for other delivery channels in the `Marain.UserNotifications.ThirdParty.DeliveryChannels` shown.

```csharp
public interface IAirshipClient
{
   /// <summary>
   /// Triggers notification to be sent from <see cref="AirshipClient"/>.
   /// </summary>
   /// <param name="namedUser">The unique Id of the targetted user which is being sent this notification.</param>
   /// <param name="notification">The notification object which containts all necessary information about the triggered notificaion.</param>
   /// <returns>Response content returned from the Airship Endpoint.</returns>
   Task<AirshipWebPushResponse?> SendWebPushNotification(string namedUser, Notification notification);
}
```

6. Add the new delivery channel into the routing engine (which takes place in the `DispatchNotificationActivity` activity).

```csharp
switch (deliveryChannelConfigured)
{
   case DeliveryChannel.Airship:
      await this.SendWebPushNotificationAsync(request.Payload.UserId, request.Payload.NotificationType, request.Payload.Id, tenant, notificationTemplate.WebPushTemplate).ConfigureAwait(false);
      break;

    // Write code to send this notification via the new delivery channel
   // Example:
   case DeliveryChannel.SendGrid:
      await this.SendEmailNotificationAsync(request.Payload.UserEmail, request.Payload.NotificationType, request.Payload.Id, tenant, notificationTemplate.EmailTemplate).ConfigureAwait(false);
   break;
}
```
