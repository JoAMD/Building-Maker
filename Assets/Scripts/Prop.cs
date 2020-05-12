using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    protected Transform prop;
    private Vector3 sc;
    //[SerializeField] private bool isDragging = false;
    //[SerializeField] protected bool isDraggingAfterDelay = false;
    //private IEnumerator MovePropCo;
    public float onDragYCoord = 5.3f;
    protected bool isMouseDragStart = true;
    private float mouseDragStartTime;
    protected float waitTimeForDrag = 0.12f;
    public bool isEmulated;
    public bool isDoneStart = false;

    public virtual void Start()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!");
        prop = transform.parent;
        //MovePropCo = MovePropAfterDelay();
    }

    public void RotateProp()
    {
        transform.parent.rotation = Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y + 90, transform.parent.eulerAngles.z);
    }

    public virtual void OnMouseDrag()
    {
        //isDragging = true;
        //Debug.Log("dragging " + prop.parent.name);
        //StartCoroutine(MovePropCo);
        if (isMouseDragStart)
        {
            mouseDragStartTime = Time.time;
            isMouseDragStart = false;
        }
        if(Time.time > mouseDragStartTime + waitTimeForDrag)
        {
            if (!isEmulated)
            {
                GameManager.instance.prop_being_held = this;
                MoveProp();
            }
        }
    }

    public virtual void OnMouseUp()
    {
        //isDragging = false;
        //isDraggingAfterDelay = false;
        GameManager.instance.prop_being_held = null;
        isMouseDragStart = true;
        //prop.position = new Vector3(prop.position.x, GameManager.instance.ceiling.position.y + onDragYCoord, prop.position.y);
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
        //if (isDragging)
        //{
        //    isDraggingAfterDelay = true;
        //    GameManager.instance.prop_being_held = this;
        //}
        //while(isDraggingAfterDelay)
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

        //if (Input.touchCount > 0) // no need
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
