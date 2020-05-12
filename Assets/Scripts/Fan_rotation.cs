using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_rotation : Prop
{
    public Transform rotatable_part;
    float spinSpeed = 400.0f;
    public bool isTap = false;

    public void OnMouseDown()
    {
        StartCoroutine(MouseDownHelper());
    }

    public IEnumerator MouseDownHelper()
    {
        yield return new WaitForSeconds(waitTimeForDrag);
        yield return new WaitForEndOfFrame();
        if (!isMouseDragStart)
        {
            yield break;
        }
        
        isTap = !isTap;
        // else { transform.Rotate(0, 0, 0); }
        //Plugin.instance.jc.Call("publish", gameObject.name[2]);
    }

    public override void Start()
    {
        base.Start();
        if(GameManager.instance.states.Count > GameManager.instance.ctr)
        {
            Debug.Log("isEmulated = " + isEmulated);
            if (isEmulated)
            {
                Debug.Log("Emulated so no need of getting from gm states");
            }
            else
            {
                isTap = !GameManager.instance.states[GameManager.instance.ctr];
            }
        }
        MouseDownHelper();
        isDoneStart = true;
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
