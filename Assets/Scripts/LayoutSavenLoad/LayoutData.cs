using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LayoutData
{
    public int roomCount;

    public float[][] roomsPos;
    public int[] roomType;

    public int[] propCount;
    public PropData[][] propDetails;

    public LayoutData()
    {
        roomCount = 0;

        roomsPos = new float[50][];
        for (int i = 0; i < roomsPos.Length; i++)
        {
            roomsPos[i] = new float[3];
        }
        
        roomType = new int[50];

        propDetails = new PropData[50][];
        propCount = new int[50];
        
        for (int i = 0; i < propDetails.Length; i++)
        {
            propCount[i] = 0;
            propDetails[i] = new PropData[20];
        }
    }

}
