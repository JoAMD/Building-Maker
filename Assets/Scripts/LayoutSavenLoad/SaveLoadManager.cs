using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class SaveLoadManager : MonoBehaviour
{
    private int layoutFileCount = 0;
    public GameObject savedText;

    [Header("Rooms")]
    public List<GameObject> rooms;
    public GameObject fan, light;

    public TextMeshProUGUI inputFieldText;

    public GameObject fileExists;
    public Button saveBtnMain;

    public SpawnController lightSpawner;
    public SpawnController fanSpawner;
    public RoomSpawner roomSpawner;

    public void SaveLayoutData(bool isDelete)
    {
        List<RoomReferences> roomsRefs = GameManager.instance.roomsRefs;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/blueprint" + inputFieldText.text[9] + ".layoutdata";  // + "/blueprint" + layoutFileCount + ".layoutdata";
        Debug.Log("path = " + path);
        if (File.Exists(path))
        {
            if (isDelete)
            {
                saveBtnMain.interactable = true;
                File.Delete(path);
            }
            else
            {
                Debug.Log("o");
                saveBtnMain.interactable = false;
                fileExists.SetActive(true);
                return;
            }
        }
        else
        {
            saveBtnMain.interactable = true;
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

        stream.Close();
    }

    public void LoadLayout()
    {
        StartCoroutine(LoadLayoutData(layoutFileCount - 1));
    }

    public IEnumerator LoadLayoutData(int layoutFileCount)
    {
        string path = Application.persistentDataPath + "/blueprint" + ((inputFieldText.text.Length>9)?inputFieldText.text[9]:'\0') + ".layoutdata"; //" / blueprint" + layoutFileCount + ".layoutdata";
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
                    prop = Instantiate(layoutData.propDetails[i][j].isFan ? fan : light, currPropPos, Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                    //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.L));
                    currRoomRefs.props.Add(prop.transform);
                    Debug.Log("adding prop to room refs => " + prop.name + " " + currRoomRefs.gameObject.name);
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
                //GameManager.instance.roomsRefs.Add(currRoomRefs);

            }

            stream.Close();

            //return layoutData;
        }
        else
        {
            Debug.LogError("Save File not located at " + path);
            GameManager.instance.errorText.SetActive(true);
            //layoutData = null;
            //return null;
        }
        yield return null;
    }
}
