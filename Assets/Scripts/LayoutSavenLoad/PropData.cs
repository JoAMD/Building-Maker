using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PropData
{
    public float[] pos;
    public bool isFan;
    public bool isOn;

    public PropData(Vector3 pos, bool isFan, bool isOn)
    {
        this.pos = new float[] { pos.x, pos.y, pos.z };
        this.isFan = isFan;
        this.isOn = isOn;
    }

}