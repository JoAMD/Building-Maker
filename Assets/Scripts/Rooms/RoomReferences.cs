using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropData
{
    public Vector3 pos;
    public bool isFan;
    public bool isOn;

    public PropData(Vector3 pos, bool isFan, bool isOn)
    {
        this.pos = pos;
        this.isFan = isFan;
        this.isOn = isOn;
    }

}

public class RoomReferences : MonoBehaviour
{
    public List<Transform> props;
    public GameObject _camera;
    public Transform thisRoomCeiling;

    private void Start()
    {
        props = new List<Transform>();
    }
}


