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

    protected override void Start()
    {
        base.Start();
        onDragYCoord = GameManager.instance.onDragYCoordSetting;
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
            GameManager.instance._zoomBtn.onClick.AddListener(() => FocusCameraOnGameObject(false));
        }
    }

    protected override void OnMouseUp()
    {
        //base.OnMouseUp();

        //GameManager.instance._zoomBtn.onClick.RemoveAllListeners();//RemoveListener(() => FocusCameraOnGameObject());

    }

    void FocusCameraOnGameObject(bool isZoomingIn)
    {
        //for ceiling scene orthographic camera
        Vector3 pos;
        float orthographicSize;
        if (isZoomingIn)
        {
            _camZoomedOutPos = GameManager.instance._cameraCurr.transform.position;
            _camZoomedOrthographicSize = GameManager.instance._cameraCurr.orthographicSize;
            pos = _roomToZoomCentre.position;
            pos.y = GameManager.instance._cameraCurr.transform.position.y;
            orthographicSize = _roomSizeOrthographic;
        }
        else
        {
            pos = _camZoomedOutPos;
            orthographicSize = _camZoomedOrthographicSize;
        }
        GameManager.instance._cameraCurr.transform.position = pos;
        GameManager.instance._cameraCurr.orthographicSize = orthographicSize;
    }

    void FocusCameraOnGameObjectRoomCam()
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
