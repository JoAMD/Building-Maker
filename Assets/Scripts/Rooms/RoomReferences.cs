using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomReferences : MonoBehaviour
{
    public List<Transform> props;
    public GameObject _camera;
    public Transform thisRoomCeiling;
    public BoxCollider boxCollider;

    private void Start()
    {
        props = new List<Transform>();
    }
}


