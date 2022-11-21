using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class NotificationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        var channel = new AndroidNotificationChannel()
        {

            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        

        var notification = new AndroidNotification();
        notification.Title = "You know it is time to be productive!📚";
        notification.Text = "Start the timer now and make progress.";
        notification.FireTime = System.DateTime.Now.AddSeconds(5);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        if(AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
            
        } 

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Starting at Android 8.0 all notifications must be assigned to a notification channel
    
    

    //Starting at Android 13.0 notifications can not be posted without permission.
    public void RequestPermissionNotification()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    //Text notification
    

    








}
