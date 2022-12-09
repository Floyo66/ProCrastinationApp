using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Android;

public class NotificationManager : MonoBehaviour
{

    public string url;
    // Start is called before the first frame update
    public void Start()
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
        notification.Title = "It is time to be productive!📚";
        notification.Text = "start your timer";
        notification.FireTime = System.DateTime.Now.AddSeconds(4);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        if(AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
            
        } 

        //StartCoroutine(Get(url));


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

     //IEnumerator enables async-like functinality in Unity.
    public IEnumerator Get(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {

                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    var notify = JsonUtility.FromJson<Notification_C>(result);
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
                    Debug.Log(notify.title_1);
                    Debug.Log(notify.title_2);
                    notification.Title = notify.title_1;
                    notification.Text = notify.title_2;
                    notification.FireTime = System.DateTime.Now.AddSeconds(1);

                    var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

                    if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
                    {
                        AndroidNotificationCenter.CancelAllNotifications();
                        AndroidNotificationCenter.SendNotification(notification, "channel_id");

                    }
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }

    }











}
