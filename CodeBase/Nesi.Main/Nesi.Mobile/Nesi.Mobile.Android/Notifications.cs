using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Nesi.Mobile.Droid;
using Nesi.Mobile.Models;

[assembly: Xamarin.Forms.Dependency(typeof(Notifications))]
namespace Nesi.Mobile.Droid
{
    public class Notifications : INotification
    {
        static readonly string CHANNEL_ID = "id_notification";
        internal static readonly string COUNT_KEY = "count";
        Context thisContext = Application.Context;

        public void ShowNotification(string title, string text)
        {

            
            
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(thisContext, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(thisContext, pendingIntentId, intent, PendingIntentFlags.OneShot);

            // Instantiate the builder and set notification elements, including pending intent:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(thisContext, CHANNEL_ID)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(text)
                .SetSmallIcon(Resource.Drawable.nesiWhite);


            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                thisContext.GetSystemService(Context.NotificationService) as NotificationManager;

                
            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }

        public void CancelNotification()
        {
            NotificationManager notificationManager =
                thisContext.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Cancel(0);
        }

    }
}