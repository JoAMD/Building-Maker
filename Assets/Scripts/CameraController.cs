using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _centre;
    public Vector3 _targetH;
    public Vector3 _targetV;
    public Vector3 _radius;
    public float _radiusMagitude;
    public Vector3 _perpendicularToRadius;
    public Vector3 _centrePos;

    private void Start()
    {
        //_maxRoomSize = new Vector3(15, 15, 15);
        _centrePos = _centre.position;
        _centrePos.y = transform.position.y;
        _radius = transform.position - _centrePos;
        _radiusMagitude = _radius.magnitude;
        GatherInput();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            GatherInput();
        }
        MoveCam();
    }

    private void GatherInput()
    {
        float h = 45;// Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        _radius = transform.position - _centrePos;
        //Debug.Log("_centrePos = " + _centrePos);
        _targetH = _radius = Quaternion.Euler(0, h, 0) * _radius;
        //_radius = Vector3.ClampMagnitude(_radius, _radiusMagitude);
        Debug.Log("radius mag = " + _radius.magnitude);
        //Debug.Log("Radius : " + _radius);
        //Debug.Log("Transform.position : " + transform.position);
        //_perpendicularToRadius = Quaternion.Euler(0, 90, 0) * _radius;
        //_targetV = _radius = Quaternion.AngleAxis(v, _perpendicularToRadius) * _radius;

    }

    private void MoveCam()
    {
        transform.position = Vector3.Slerp(transform.position, _targetH, Time.deltaTime);
        transform.LookAt(_centre, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.position = new Vector3(transform.position.x, _centrePos.y, transform.position.z);
    }

}
