using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class SetIPScript : MonoBehaviour
{
    private string ip;
    private void Start()
    {
        Plugin.instance.runner();
    }

    public void SetIP(TMP_InputField ip)
    {
        this.ip = ip.text;
    }
    
    public void LoadMainScene()
    {

        try
        {
            Plugin.instance.jc.Call("GetIPAddress", ip);
        }
        catch (Exception e)
        {
            Debug.LogWarning("couldn't call GetIPAddress! Error stack trace => " + e.StackTrace);
        }

        //Plugin.instance.jc.Call("GetIPAddress", ip);
        SceneManager.LoadSceneAsync(1);
    }

}
