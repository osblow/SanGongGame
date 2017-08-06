using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeixinHandler 
{
    public static void ShareToFriend(string shareTitle, string shareUrl, string shareDescribe)
    {
        Debug.Log("好友 ");

#if UNITY_EDITOR

#elif UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                                                                                  
        jo.Call("shareToFriend", shareTitle, shareUrl);
#endif
    }

    public static void ShareToTimeline(string shareTitle, string shareUrl, string shareDescribe)
    {
        Debug.Log("朋友圈 ");

#if UNITY_EDITOR

#elif UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                                                                                  
        jo.Call("shareToTimeLine", shareTitle, shareUrl, shareDescribe);
#endif
    }

    public static void RequestRoomNumber()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                                                                                  
        Osblow.App.Globals.Instance.RoomNumber =  jo.Call<string>("GetRoomNumber");
#endif
    }
}
