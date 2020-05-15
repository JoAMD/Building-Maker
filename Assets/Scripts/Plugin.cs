using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plugin : MonoBehaviour
{

    public static Plugin instance;

    #region Singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    const string pluginName = "com.example.unity.MyPlugIn";
    public AndroidJavaObject jc;

    public void runner()
    {
        jc = new AndroidJavaObject(pluginName);
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        if (activity != null)
        {
            Debug.Log("set context called");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            jc.Call("setContext", context);
        }
        else
        {
            Debug.LogWarning("Activity is null!");
        }

        //1.get initial on/off state so that app shows initially light on/off
        //use this to set ON OFF state inside UNITY
        //jc.Call("getState");

        //jc.Call<ArrayList>("getStateInit");

        //2.Call this from Unity to change to a diff fan light where str is a string with topic name
        //jc.Call("setTopic",str)

        //3.Call this from Unity to change to a diff fan light
        //jc.Call("publish");
    }

    private void Update()
    {

    }
}
