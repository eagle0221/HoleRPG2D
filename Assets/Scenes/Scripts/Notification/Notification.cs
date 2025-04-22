using System.Collections.Generic;

[System.Serializable]
public class Notification
{
    public int notificationNo;
    public string notificationTitle;
    public string notification;
    public string startDate;
    public string endDate;
}

[System.Serializable]
public class NotificationList
{
    public List<Notification> list;
}