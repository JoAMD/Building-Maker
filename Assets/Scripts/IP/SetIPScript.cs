using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SetIPScript : MonoBehaviour
{
    private void Start()
    {
        Plugin.instance.runner();
    }

    public void SetIPAndScene(TMP_InputField ip)
    {
        Plugin.instance.jc.Call("GetIPAddress", ip.text);
        SceneManager.LoadSceneAsync(1);
    }
}
