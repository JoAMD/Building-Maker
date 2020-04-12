using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int ctr = 0;
    public int currRoomCtr = 0;

    public List<bool> states;

    public Transform ceiling;

    public Prop prop_being_held;

    public SwitchView switchViewScript;

    public Camera _cameraCurr;
    public Camera _ceilingCamBelow;
    public Camera _roomCam;

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

        if(ar == null)
        {
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
        }
        
        for (int i = 0; i < ar.Count; i++)
        {
            states.Add(int.Parse(ar[i].ToString()) == 1 ? true : false);
        }

        for (int i = 0; i < states.Count; i++)
            Debug.Log(states[i]);

            Plugin.instance.jc.Call("unsubscribe");
    }

    public void RotateProp()
    {
        if(prop_being_held != null)
        {
            prop_being_held.RotateProp();
        }
    }
}
