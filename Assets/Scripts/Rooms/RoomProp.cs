using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProp : Prop
{
    public Vector3 _roomSizeForCamZoom = new Vector3(15, 15, 15);
    public float _roomSizeOrthographic = 5;
    public Transform _roomToZoomCentre; // find
    [SerializeField] private Vector3 _camZoomedOutPos;
    [SerializeField] private float _camZoomedOrthographicSize;
    public RoomReferences thisRoomRefs;

    protected override void Start()
    {
        base.Start();
        onDragYCoord = GameManager.instance.onDragYCoordSetting;
        GameManager.instance.roomsRefs.Add(thisRoomRefs);
    }

    private void OnMouseDown()
    {

        //GameManager.instance._zoomBtn.onClick.AddListener(() => FocusCameraOnGameObject());

        //Vector3 mp = Input.mousePosition;
        //Ray ray = GameManager.instance._cameraCurr.ScreenPointToRay(mp);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.CompareTag("Room"))
        //{
        //  FocusCameraOnGameObject();
        //}
        //if (Input.touches.Length > 0)
        //{
        //    if (Input.touches[0].tapCount > 2)
        //    {
        //FocusCameraOnGameObject();
        //    }
        //}
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
        //if (Input.touches[0].tapCount > 2)
        if (Input.GetKeyDown(KeyCode.F))
        {
            FocusCameraOnGameObject(true);
            Debug.Log("Added listener to " + GameManager.instance._zoomBtn.name);
            GameManager.instance._zoomBtn.onClick.AddListener(() => FocusCameraOnGameObject(false));
            //return; //instead use a bool to not run code in OnMouseDrag in base class and this class
        }
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();

        //GameManager.instance._zoomBtn.onClick.RemoveAllListeners();//RemoveListener(() => FocusCameraOnGameObject());

    }

    private void FocusCameraOnGameObject(bool isZoomingIn)
    {
        //for ceiling scene orthographic camera
        Vector3 pos;
        float orthographicSize;
        if (isZoomingIn)
        {
            GameManager.instance.DisableAllRoomBoxColliders(false);
            GameManager.instance._zoomBtn.gameObject.SetActive(true);

            GameManager.instance.lightBtn.SetActive(true);
            GameManager.instance.fanBtn.SetActive(true);
            GameManager.instance.lightBtn.GetComponent<SpawnController>()._currentRoomRefs 
                = GameManager.instance.fanBtn.GetComponent<SpawnController>()._currentRoomRefs
                = thisRoomRefs;
            GameManager.instance.lightBtn.GetComponent<SpawnController>().ceiling 
                = GameManager.instance.fanBtn.GetComponent<SpawnController>().ceiling
                = thisRoomRefs.thisRoomCeiling;

            _camZoomedOutPos = GameManager.instance._cameraCurr.transform.position;
            _camZoomedOrthographicSize = GameManager.instance._cameraCurr.orthographicSize;
            Debug.Log(_camZoomedOrthographicSize + " = _camZoomedOrthographicSize");
            Debug.Log(_camZoomedOutPos + " = _camZoomedOutPos");
            pos = _roomToZoomCentre.position;
            pos.y = GameManager.instance._cameraCurr.transform.position.y;
            orthographicSize = _roomSizeOrthographic;
        }
        else
        {
            GameManager.instance._zoomBtn.onClick.RemoveAllListeners();
            GameManager.instance._zoomBtn.gameObject.SetActive(false);

            GameManager.instance.DisableAllRoomBoxColliders(true);
            GameManager.instance.lightBtn.SetActive(false);
            GameManager.instance.fanBtn.SetActive(false);
            GameManager.instance.lightBtn.GetComponent<SpawnController>()._currentRoomRefs
                = GameManager.instance.fanBtn.GetComponent<SpawnController>()._currentRoomRefs
                = thisRoomRefs;
            GameManager.instance.lightBtn.GetComponent<SpawnController>().ceiling
                = GameManager.instance.fanBtn.GetComponent<SpawnController>().ceiling
                = thisRoomRefs.thisRoomCeiling;

            pos = _camZoomedOutPos;
            orthographicSize = _camZoomedOrthographicSize;
        }
        GameManager.instance._cameraCurr.transform.position = pos;
        GameManager.instance._cameraCurr.orthographicSize = orthographicSize;
    }

    private void FocusCameraOnGameObjectRoomCam()
    {
        Vector3 max = _roomSizeForCamZoom;
        float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
        float dist = radius / (Mathf.Sin(GameManager.instance._cameraCurr.fieldOfView * Mathf.Deg2Rad / 2f));
        Debug.Log("Radius = " + radius + " dist = " + dist);
        Vector3 pos = /*Random.onUnitSphere */ -Vector3.up * dist + _roomToZoomCentre.position;
        GameManager.instance._cameraCurr.transform.position = pos;
        GameManager.instance._cameraCurr.transform.LookAt(_roomToZoomCentre.position);
    }

}
