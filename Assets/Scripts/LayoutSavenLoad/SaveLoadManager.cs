using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private int layoutFileCount = 0;
    public GameObject savedText;
    public GameObject errorText;

    [Header("Rooms")]
    public List<GameObject> rooms;
    public GameObject fan, light;

    public TextMeshProUGUI inputFieldText;

    public GameObject fileExists;
    public Button saveBtnMain;
    public Button loadBtnMain;

    public SpawnController lightSpawner;
    public SpawnController fanSpawner;

    private SceneData sd;
    private bool isSceneChangeDone = false;

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("instance = this");
            instance = this;
        }
        else if (instance != this)
        {
            //Debug.Log("instance != this so Destroy(gameObject)");
            //Destroy(gameObject);
        }
        Debug.Log("DontDestroyOnLoad(gameObject)");
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnChangeScene;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnChangeScene;
    }

    public void SaveLayoutData(bool isDelete)
    {
        List<RoomReferences> roomsRefs = GameManager.instance.roomsRefs;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + inputFieldText.text + ".layoutdata";  // + "/blueprint" + layoutFileCount + ".layoutdata";
        Debug.Log("path = " + path);
        if (File.Exists(path))
        {
            if (isDelete)
            {
                saveBtnMain.interactable = true;
                loadBtnMain.interactable = true;
                File.Delete(path);
            }
            else
            {
                Debug.Log("o");
                saveBtnMain.interactable = false;
                loadBtnMain.interactable = false;
                fileExists.SetActive(true);
                return;
            }
        }
        else
        {
            saveBtnMain.interactable = true;
            loadBtnMain.interactable = true;
        }
        Debug.Log("Path = " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        LayoutData layoutData = new LayoutData();

        // -------- Initialise layput data ---------

        Vector3 currRoomPos;
        layoutData.roomCount = roomsRefs.Count;

        for (int i = 0; i < roomsRefs.Count; i++)
        {
            currRoomPos = roomsRefs[i].transform.position;
            layoutData.roomsPos[i] = new float[3] { currRoomPos.x, currRoomPos.y, currRoomPos.z };

            layoutData.roomType[0] = 0;

            layoutData.propCount[i] = roomsRefs[i].props.Count;
            PropData[] propDataArr = new PropData[roomsRefs[i].props.Count];
            for (int j = 0; j < roomsRefs[i].props.Count; j++)
            {
                bool isFan;
                bool isOn;
                if (roomsRefs[i].props[j].CompareTag("Fan"))
                {
                    isFan = true;
                    isOn = roomsRefs[i].props[j].GetChild(1).GetComponent<Fan_rotation>().isTap;
                }
                else
                {
                    isFan = false;
                    isOn = !roomsRefs[i].props[j].GetChild(1).GetComponent<Light_Switching>().i;
                }
                propDataArr[j] = new PropData(roomsRefs[i].props[j].position, isFan, isOn);
            }
            layoutData.propDetails[i] = propDataArr;
        }

        // ------ Done initialise layput data ------

        formatter.Serialize(stream, layoutData);

        layoutFileCount++;

        Debug.Log("Saved at " + path);

        savedText.SetActive(true);
        StartCoroutine(RemoveSavedOrErrorText(true));

        stream.Close();
    }

    private IEnumerator RemoveSavedOrErrorText(bool isSavedText)
    {
        GameObject gb = (isSavedText ? savedText : errorText);
        yield return new WaitForSeconds(isSavedText ? 2 : 3);
        if (gb != null)
        {
            gb.SetActive(false);
        }
    }

    public void LoadLayout()
    {
        StartCoroutine(LoadLayoutData(layoutFileCount - 1));
    }

    public void ReloadSceneAndLoad()
    {
        isSceneChangeDone = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LoadLayout();
    }

    public IEnumerator LoadLayoutData(int layoutFileCount)
    {
        string inputFieldTxt = inputFieldText.text;

        yield return new WaitUntil(() => isSceneChangeDone);

        SceneData newSd = FindObjectOfType<SceneData>();
        newSd.switchViewBtn.interactable = false;
        isSceneChangeDone = false;
        string path = Application.persistentDataPath + "/" + inputFieldTxt + ".layoutdata"; //" / blueprint" + layoutFileCount + ".layoutdata";
        if (File.Exists(path))
        {
            GameManager.instance.roomsRefs = new List<RoomReferences>();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LayoutData layoutData = formatter.Deserialize(stream) as LayoutData;

            Vector3 currRoomPos;
            GameObject currRoom;
            Debug.Log(layoutData.roomCount);
            for (int i = 0; i < layoutData.roomCount; i++)
            {

                currRoomPos = new Vector3(layoutData.roomsPos[i][0], layoutData.roomsPos[i][1], layoutData.roomsPos[i][2]);
                currRoom = Instantiate(rooms[layoutData.roomType[i]], currRoomPos, Quaternion.identity);
                currRoom.transform.SetParent(GameManager.instance.ceiling);
                GameManager.instance.switchViewScript.rooms.Add(currRoom.transform.GetChild(0).gameObject);

                Prop currProp = currRoom.transform.GetChild(1).GetComponent<Prop>();
                currProp.ceiling = GameManager.instance.roomSpawners[0].GetComponent<RoomSpawner>().ceiling;
                currProp.distanceFromWall = GameManager.instance.roomSpawners[0].GetComponent<RoomSpawner>().distanceFromWall;
                //// ------------------ Start Emulating ------------------
                //RoomProp roomPropCurr = currRoom.transform.GetChild(1).GetComponent<RoomProp>();
                //roomSpawner.prop_clone = currRoom.transform;

                //GameManager.instance.lightBtn.GetComponent<SpawnController>()._currentRoomRefs
                //    = GameManager.instance.fanBtn.GetComponent<SpawnController>()._currentRoomRefs
                //    = roomPropCurr.thisRoomRefs;
                //GameManager.instance.lightBtn.GetComponent<SpawnController>().ceiling
                //    = GameManager.instance.fanBtn.GetComponent<SpawnController>().ceiling
                //    = roomPropCurr.thisRoomRefs.thisRoomCeiling;

                ////roomPropCurr.Start();
                //roomPropCurr.isEmulated = true;
                //roomPropCurr.OnMouseDown();
                //float startTime = Time.time;
                //while (true)
                //{
                //    roomPropCurr.OnMouseDrag();
                //    if(Time.time - startTime > 0.5f)
                //    {
                //        break;
                //    }
                //    yield return null;
                //}
                //roomPropCurr.OnMouseUp();
                //roomSpawner.OnMouseUp();
                //roomPropCurr.isEmulated = false;
                //// ------------------ Done Emulating ------------------


                RoomReferences currRoomRefs = currRoom.GetComponent<RoomReferences>();
                Transform roomCeiling = currRoomRefs.thisRoomCeiling;
                Vector3 currPropPos;
                GameObject prop;
                Debug.Log(layoutData.propCount[i]);
                for (int j = 0; j < layoutData.propCount[i]; j++)
                {
                    //Debug.Log(layoutData.propDetails[i][j].)
                    currPropPos = new Vector3(layoutData.propDetails[i][j].pos[0], layoutData.propDetails[i][j].pos[1], layoutData.propDetails[i][j].pos[2]);
                    //currPropPos = layoutData.propDetails[i][j].pos;
                    bool isFan = layoutData.propDetails[i][j].isFan;
                    prop = Instantiate(isFan ? fan : light, currPropPos, Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                    //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.L));
                    currRoomRefs.props.Add(prop.transform);
                    Debug.Log("adding prop to room refs => " + prop.name + " " + currRoomRefs.gameObject.name);
                    currProp = prop.transform.GetChild(1).GetComponent<Prop>();
                    currProp.ceiling = (isFan ? GameManager.instance.fanBtn : GameManager.instance.lightBtn).GetComponent<SpawnController>().ceiling;
                    currProp.distanceFromWall = (isFan ? GameManager.instance.fanBtn : GameManager.instance.lightBtn).GetComponent<SpawnController>().distanceFromWall;
                    //// ----------------------- Start Emulating -----------------------
                    //if (layoutData.propDetails[i][j].isFan)
                    //{
                    //    // ------------------ Emulating ------------------
                    //    Fan_rotation fanScriptCurr = prop.transform.GetChild(1).GetComponent<Fan_rotation>();
                    //    fanSpawner.prop_clone = prop.transform;
                    //    fanScriptCurr.isEmulated = true;
                    //    fanScriptCurr.isTap = !layoutData.propDetails[i][j].isOn;
                    //    Debug.Log("done setting emulated and isOn");
                    //    //roomPropCurr.Start();
                    //    fanScriptCurr.OnMouseDown();
                    //    float startTime2 = Time.time;
                    //    while (true)
                    //    {
                    //        fanScriptCurr.OnMouseDrag();
                    //        if (Time.time - startTime > 0.5f)
                    //        {
                    //            break;
                    //        }
                    //        yield return null;
                    //    }
                    //    fanScriptCurr.OnMouseUp();
                    //    fanSpawner.OnMouseUp();
                    //    yield return new WaitUntil(() => fanScriptCurr.isDoneStart);
                    //    fanScriptCurr.isEmulated = false;
                    //}
                    //else
                    //{
                    //    // ------------------ Emulating ------------------
                    //    Light_Switching lightScriptCurr = prop.transform.GetChild(1).GetComponent<Light_Switching>();
                    //    lightSpawner.prop_clone = prop.transform;
                    //    lightScriptCurr.isEmulated = true;
                    //    lightScriptCurr.i = layoutData.propDetails[i][j].isOn;
                    //    Debug.Log("done setting emulated and isOn");
                    //    //roomPropCurr.Start();
                    //    lightScriptCurr.OnMouseDown();
                    //    float startTime2 = Time.time;
                    //    while (true)
                    //    {
                    //        lightScriptCurr.OnMouseDrag();
                    //        if (Time.time - startTime > 0.5f)
                    //        {
                    //            break;
                    //        }
                    //        yield return null;
                    //    }
                    //    lightScriptCurr.OnMouseUp();
                    //    lightSpawner.OnMouseUp();
                    //    yield return new WaitUntil(() => lightScriptCurr.isDoneStart);
                    //    lightScriptCurr.isEmulated = false;
                    //}

                    //// ----------------------- Done Emulating -----------------------

                    prop.transform.SetParent(roomCeiling);

                }
                GameManager.instance.roomsRefs.Add(currRoomRefs);

            }

            stream.Close();

            //return layoutData;
        }
        else
        {
            Debug.LogError("Save File not located at " + path);
            errorText.SetActive(true);
            StartCoroutine(RemoveSavedOrErrorText(false));
            //layoutData = null;
            //return null;
        }
        newSd.switchViewBtn.interactable = true;
        Destroy(gameObject);
        yield return null;
    }

    private void OnChangeScene(Scene oldScene, Scene newScene)
    {
        if (SceneManager.GetActiveScene().Equals(newScene))
        {
            isSceneChangeDone = true;
        }
        Debug.Log("Scene Change");
        //sd = FindObjectOfType<SceneData>();
        //if(sd == null)
        //{
        //    Debug.LogError("Scene data not found");
        //}
        //else
        //{
        //    savedText = sd.savedText;
        //    errorText = sd.errorText;
        //    inputFieldText = sd.inputFieldText;
        //    fileExists = sd.fileExists;
        //    saveBtnMain = sd.saveBtnMain;
        //    loadBtnMain = sd.loadBtnMain;
        //    lightSpawner = sd.lightSpawner;
        //    fanSpawner = sd.fanSpawner;
        //    roomSpawner = sd.roomSpawner;
        //}
    }
}
