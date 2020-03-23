using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int ctr = 0;

    public List<bool> states;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Plugin.instance.runner();

        states = new List<bool>();
        ArrayList ar = Plugin.instance.jc.Call<ArrayList>("getStateInit");

        // To Test in Unity
        //Comment this block "unity" out when testing with rpi/hardware/mqtt
        #region unity
/*
        ar = new ArrayList();
        ar.Add("1");
        ar.Add("0");
        ar.Add("1");
        ar.Add("0");
        ar.Add("1");
        ar.Add("0");
        ar.Add("1");
        ar.Add("0");
        ar.Add("1");
        ar.Add("0");
        ar.Add("1");
        ar.Add("0");
*/
        #endregion

        for (int i = 0; i < ar.Count; i++)
        {
            states.Add(int.Parse(ar[i].ToString()) == 1 ? true : false);
        }
        
        for (int i = 0; i < states.Count; i++)
            Debug.Log(states[i]);

            Plugin.instance.jc.Call("unsubscribe");
    }

    void Update()
    {

    }
}
