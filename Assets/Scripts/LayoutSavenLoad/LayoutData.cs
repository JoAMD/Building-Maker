using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LayoutData
{
    public float[][] roomsPos;
    public int[] roomType;
    public PropData[][] propDetails;

    public LayoutData()
    {
        roomsPos = new float[50][];
        for (int i = 0; i < roomsPos.Length; i++)
        {
            roomsPos[i] = new float[3];
        }
        
        roomType = new int[50];

        propDetails = new PropData[50][];
        for (int i = 0; i < propDetails.Length; i++)
        {
            propDetails[i] = new PropData[20];
        }
    }

}
