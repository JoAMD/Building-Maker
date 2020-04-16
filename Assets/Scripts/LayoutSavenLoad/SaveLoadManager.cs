using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SaveLoadManager : MonoBehaviour
{
    private int layoutFileCount = 0;
    public GameObject savedText;

    [Header("Rooms")]
    public List<GameObject> rooms;
    public GameObject fan, light;

    public void SaveLayoutData()
    {
        List<RoomReferences> roomsRefs = GameManager.instance.roomsRefs;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/blueprint" + layoutFileCount + ".layoutdata";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        Debug.Log("Path = " + path);
        FileStream stream = new FileStream(path, FileMode.Create);

        LayoutData layoutData = new LayoutData();

        // -------- Initialise layput data ---------

        Vector3 currRoomPos;
        for (int i = 0; i < roomsRefs.Count; i++)
        {
            currRoomPos = roomsRefs[i].transform.position;
            layoutData.roomsPos[i] = new float[3] { currRoomPos.x, currRoomPos.y, currRoomPos.z };

            layoutData.roomType[0] = 0;

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
        LoadLayoutData(layoutFileCount - 1);
    }

    public LayoutData LoadLayoutData(int layoutFileCount)
    {
        string path = Application.persistentDataPath + "/blueprint" + layoutFileCount + ".layoutdata";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LayoutData layoutData = formatter.Deserialize(stream) as LayoutData;

            Vector3 currRoomPos;
            GameObject currRoom;
            for (int i = 0; i < layoutData.roomType.Length; i++)
            {
                currRoomPos = new Vector3(layoutData.roomsPos[i][0], layoutData.roomsPos[i][1], layoutData.roomsPos[i][2]);
                currRoom = Instantiate(rooms[layoutData.roomType[i]], currRoomPos, Quaternion.identity);
                currRoom.transform.SetParent(GameManager.instance.ceiling);

                Transform roomCeiling = currRoom.GetComponent<RoomReferences>().thisRoomCeiling;
                Vector3 currPropPos;
                GameObject prop;
                for (int j = 0; j < layoutData.propDetails[i].Length; j++)
                {
                    currPropPos = new Vector3(layoutData.propDetails[i][j].pos[0], layoutData.propDetails[i][j].pos[1], layoutData.propDetails[i][j].pos[2]);
                    //currPropPos = layoutData.propDetails[i][j].pos;
                    prop = Instantiate(layoutData.propDetails[i][j].isFan ? fan : light, currPropPos, Quaternion.identity);
                    prop.transform.SetParent(roomCeiling);
                }

            }

            stream.Close();

            return layoutData;
        }
        else
        {
            Debug.LogError("Save File not located at " + path);
            GameManager.instance.errorText.SetActive(true);
            return null;
        }
    }
}
