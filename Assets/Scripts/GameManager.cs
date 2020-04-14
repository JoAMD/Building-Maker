using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

    public List<RoomReferences> _roomReferences;

    public GameObject errorText;

    private int layoutFileCount = 0;

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

    public void SaveLevelData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level" + layoutFileCount + ".layoutdata";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream stream = new FileStream(path, FileMode.Create);

        LayoutData layoutData = new LayoutData();

        formatter.Serialize(stream, layoutData);

        layoutFileCount++;

        Debug.Log("Saved at " + path);

        stream.Close();
    }

    public LayoutData LoadLevelData(int layoutFileCount)
    {
        string path = Application.persistentDataPath + "/level" + layoutFileCount + ".layoutdata";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LayoutData layoutData = formatter.Deserialize(stream) as LayoutData;
            stream.Close();

            return layoutData;
        }
        else
        {
            Debug.LogError("Save File not located at " + path);
            errorText.SetActive(true);
            return null;
        }
    }

    private IEnumerator RemoveErrortext()
    {
        yield return new WaitForSeconds(2);
        errorText.SetActive(false);
    }
}
