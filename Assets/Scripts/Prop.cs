using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    protected Transform prop;
    private Vector3 sc;
    [SerializeField] private bool isDragging = false;
    [SerializeField] protected bool isDraggingAfterDelay = false;
    private IEnumerator MovePropCo;
    protected bool isMouseDown = false;
    public float onDragYCoord = 5.3f;

    protected virtual void Start()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!");
        prop = transform.parent;
        MovePropCo = MovePropAfterDelay();
    }

    public void RotateProp()
    {
        transform.parent.rotation = Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y + 90, transform.parent.eulerAngles.z);
    }

    protected virtual void OnMouseDrag()
    {
        isDragging = true;
        Debug.Log("dragging " + prop.parent.name);
        //StartCoroutine(MovePropCo);
        GameManager.instance.prop_being_held = this;
        MoveProp();
    }

    protected virtual void OnMouseUp()
    {
        isDragging = false;
        isDraggingAfterDelay = false;
        GameManager.instance.prop_being_held = null;
        //    if(MovePropCo != null)
        //    {
        //        StopCoroutine(MovePropCo);
        //    }
        //    MovePropCo = MovePropAfterDelay();
        //}

        //protected virtual IEnumerator TogglePropAfterDelay()
        //{

    }

    private IEnumerator MovePropAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("@@@@@@");
        if (isDragging)
        {
            isDraggingAfterDelay = true;
            GameManager.instance.prop_being_held = this;
        }
        while(isDraggingAfterDelay)
        {
            Debug.Log("######");
            MoveProp();
            yield return null;
        }
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            prop.eulerAngles += new Vector3(0, 90, 0);
        }
    }

    private void MoveProp()
    {
        //light_clone.transform.position = 
        // -------------- FOR ANDROID --------------

        //if (Input.touchCount > 0)
        //{
        //    sc = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        //    sc.y = GameManager.instance.ceiling.position.y + onDragYCoord;

        //    sc.x = Mathf.RoundToInt(sc.x);
        //    sc.z = Mathf.RoundToInt(sc.z);

        //    //Debug.Log(sc);
        //    prop.position = sc;
        //}

        // -------------- FOR EDITOR --------------
        GetInput();

        sc = GameManager.instance._cameraCurr.ScreenToWorldPoint(Input.mousePosition);
        sc.y = GameManager.instance.ceiling.position.y + onDragYCoord;

        sc.x = Mathf.RoundToInt(sc.x);
        sc.z = Mathf.RoundToInt(sc.z);

        //Debug.Log(sc);
        prop.position = sc;
        
    }
}
