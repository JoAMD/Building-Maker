using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float onDragYCoordSetting = 1.32f;

    public Button _zoomBtn;

    public GameObject lightBtn, fanBtn;

    public List<RoomReferences> roomsRefs;
    public List<Button> roomSpawners;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        roomsRefs = new List<RoomReferences>();
        //Plugin.instance.runner();

        states = new List<bool>();
        List<string> ar=new List<string>();
        //List<string> x=new List<string>() ;
        int x;
        try
        { 
            Debug.Log("jc = " + Plugin.instance.jc);
            //Debug.Log("ar = " + ar);
             x = Plugin.instance.jc.Call<int>("getLength");
            Debug.Log("\n x=" + x);
            if (x != 0) {
                for (int i = 0; i < x; i++)
                {
                    Debug.Log("\n ar=" + i.ToString());
                    Debug.Log("\n" + Plugin.instance.jc.Call<string>("getStateInit", i.ToString()));
                    ar.Add(Plugin.instance.jc.Call<string>("getStateInit",i.ToString()));
                }
                Debug.Log("\n arLength=" + ar.IndexOf("sss"));
            }
            
            //ar = Plugin.instance.jc.Call<ArrayList>("getStateInit");
        }
        catch (Exception e)
        {
            Debug.LogWarning("couldn't call getStateInit! Error stack trace => " + e.StackTrace);
        }

        //if (Plugin.instance.jc != null)
        //{
        //    ar = Plugin.instance.jc.Call<ArrayList>("getStateInit");
        //}


        if(ar == null)
        {
            ar = new List<string>();
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

       /* try
        {
            Plugin.instance.jc.Call("unsubscribe");
        }
        catch(Exception e)
        {
            Debug.LogWarning("couldn't call unsubscribe! Error stack trace => " + e.StackTrace);
        }*/

        //if (Plugin.instance.jc != null)
        //{
        //    Plugin.instance.jc.Call("unsubscribe");
        //}
        //else
        //{
        //    Debug.LogWarning("jc null!");
        //}
    }

    public void RotateProp()
    {
        if(prop_being_held != null)
        {
            prop_being_held.RotateProp();
        }
    }

    public void DisableAllRoomBoxColliders(bool isEnabled)
    {
        for (int i = 0; i < roomsRefs.Count; i++)
        {
            roomsRefs[i].boxCollider.enabled = isEnabled;
        }
    }

    public void ToggleAllRoomSpawnerBtns(bool isInteractable)
    {
        for (int i = 0; i < roomSpawners.Count; i++)
        {
            //roomSpawners[i].interactable = isInteractable;
            roomSpawners[i].gameObject.SetActive(isInteractable);
        }
    }

}
