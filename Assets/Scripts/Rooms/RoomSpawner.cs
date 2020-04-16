using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : SpawnController
{
    public GameObject roomZoomBtn;

    protected override void Start()
    {
        base.Start();
        onDragYCoord = GameManager.instance.onDragYCoordSetting;
    }

    protected override void SpawnProp()
    {
        prop_clone = Instantiate(prop_prefab).transform;
        prop_clone.name = "R" + GameManager.instance.currRoomCtr;

        Debug.Log("currRoomCtr = " + GameManager.instance.currRoomCtr);

        prop_clone.parent = ceiling;

        GameManager.instance.prop_being_held = prop_clone.GetChild(1).GetComponent<Prop>();

        //GameManager.instance._cameraController._roomToZoomCentre = prop_clone.GetChild(2);
        //GameManager.instance._cameraControllerRoom._roomToZoomCentre = prop_clone.GetChild(2);
    }

    public override void OnMouseUp()
    {
        Debug.Log(prop_clone.localPosition.x + " and " + (ceiling.GetChild(0).localScale.x / 2 - distanceFromWall));
        Debug.Log(prop_clone.localPosition.z + " and " + (ceiling.GetChild(0).localScale.z / 2 - distanceFromWall));
        if (Mathf.Abs(prop_clone.localPosition.x) > Mathf.Abs(ceiling.GetChild(0).localScale.x / 2 - distanceFromWall) ||
            Mathf.Abs(prop_clone.localPosition.z) > Mathf.Abs(ceiling.GetChild(0).localScale.z / 2 - distanceFromWall))
        {
            //Instead give an error sign plox
            errorText.SetActive(true);
            StartCoroutine(RemoveErrortext());

            Destroy(prop_clone.gameObject); //use polling system later if needed
            //GameManager.instance._cameraController._roomToZoomCentre = null;
            //GameManager.instance._cameraControllerRoom._roomToZoomCentre = null;
        }
        else
        {
            GameManager.instance.currRoomCtr++;
            GameManager.instance.switchViewScript.rooms.Add(prop_clone.GetChild(0).gameObject);
            prop_clone.localPosition = new Vector3(prop_clone.localPosition.x, onDragYCoord, prop_clone.localPosition.z);
        }
        GameManager.instance.prop_being_held = null;
    }

    public void RoomZoom()
    {

    }

}
