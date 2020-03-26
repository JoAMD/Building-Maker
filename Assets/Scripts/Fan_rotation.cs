using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_rotation : MonoBehaviour
{
    public Transform rotatable_part;
    float spinSpeed = 400.0f;
    public bool isTap = false;

    void OnMouseDown()
    {
        MouseDownHelper();
    }

    public void MouseDownHelper()
    {
        isTap = !isTap;
        // else { transform.Rotate(0, 0, 0); }
        Plugin.instance.jc.Call("publish", gameObject.name[2]);
    }

    void Start()
    {
        if(GameManager.instance.states.Count > GameManager.instance.ctr)
            isTap = !GameManager.instance.states[GameManager.instance.ctr];
        MouseDownHelper();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTap)
        {
            rotatable_part.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }
    }
}
