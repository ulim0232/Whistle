using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappySceneManager : MonoBehaviour
{
    public Michsky.MUIP.NotificationManager notification;

    private void Start()
    {
        notification.Open();
    }

    public void GoTitle()
    {
        UIManager.instance.LoadTitle();
    }
}
