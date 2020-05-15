using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class SpawnController : MonoBehaviour
{

    public GameObject prop_prefab;
    private Vector3 sc;
    public Transform prop_clone;
    public Transform ceiling;
    public float distanceFromWall;
    public GameObject errorText;
    protected float onDragYCoord = 5.3f;
    public RoomReferences _currentRoomRefs;
    public bool isFan;

    public virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            prop_clone.eulerAngles += new Vector3(0, 90, 0);
        }
    }

    protected virtual void SpawnProp()
    {

        prop_clone = Instantiate(prop_prefab).transform;

        prop_clone.name = "sw" + GameManager.instance.ctr;
        Debug.Log(Plugin.instance.jc);
        if (Plugin.instance.jc != null)
        {
            Plugin.instance.jc.Call("subscribeNewSwitch");
        }
        else
        {
            Debug.LogWarning("jc null!");
        }

        Debug.Log("ctr = " + GameManager.instance.ctr);
        Debug.Log("GameManager.instance.states.Count = " + GameManager.instance.states.Count);
        if (GameManager.instance.states.Count > GameManager.instance.ctr)
            Debug.Log("states[ctr] = " + GameManager.instance.states[GameManager.instance.ctr]);

        prop_clone.parent = ceiling;

        GameManager.instance.prop_being_held = prop_clone.GetChild(1).GetComponent<Prop>();
        GameManager.instance.prop_being_held.ceiling = ceiling;
        GameManager.instance.prop_being_held.distanceFromWall = distanceFromWall;
    }

    public void OnMouseDown()
    {
        SpawnProp();
    }

    public void OnMouseDrag()
    {
        //light_clone.transform.position = 
        // -------------- FOR ANDROID --------------
        //if (Input.touchCount > 0)
        //{
        //    sc = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        //    sc.y = ceiling.position.y + 5.3f;

        //    sc.x = Mathf.RoundToInt(sc.x);
        //    sc.z = Mathf.RoundToInt(sc.z);

        //    //Debug.Log(sc);
        //    prop_clone.position = sc;
        //}
        // -------------- FOR EDITOR --------------
        GetInput();

        sc = GameManager.instance._cameraCurr.ScreenToWorldPoint(Input.mousePosition);
        sc.y = ceiling.position.y + onDragYCoord;

        sc.x = Mathf.RoundToInt(sc.x);
        sc.z = Mathf.RoundToInt(sc.z);

        //Debug.Log(sc);
        prop_clone.position = sc;

    }

    public virtual void OnMouseUp()
    {
        Debug.Log(prop_clone.localPosition.x + " and " + (ceiling.GetChild(0).localScale.x / 2 - distanceFromWall));
        Debug.Log(prop_clone.localPosition.z + " and " + (ceiling.GetChild(0).localScale.z / 2 - distanceFromWall));
        if( Mathf.Abs(prop_clone.localPosition.x) > Mathf.Abs(ceiling.GetChild(0).localScale.x / 2 - distanceFromWall) ||
            Mathf.Abs(prop_clone.localPosition.z) > Mathf.Abs(ceiling.GetChild(0).localScale.z / 2 - distanceFromWall))
        {
            //Instead give an error sign plox
            errorText.SetActive(true);
            StartCoroutine(RemoveErrortext());

            Destroy(prop_clone.gameObject); //use polling system later if needed
        }
        else
        {
            //_currentRoomRefs.propDetails.Add(new PropData(Vector3.zero, isFan, false)); // except arg 2 the rest are inaccurate and have to updated
            _currentRoomRefs.props.Add(prop_clone);
            GameManager.instance.ctr++;
            prop_clone.localPosition = new Vector3(prop_clone.localPosition.x, onDragYCoord, prop_clone.localPosition.z);
        }
        GameManager.instance.prop_being_held = null;
    }

    protected IEnumerator RemoveErrortext()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("YO WTF PLACE IT WELL BIATCH!");
        //float t = 0;
        //TextMeshProUGUI text = errorText.GetComponent<TextMeshProUGUI>();
        //Color startColor = text.color;
        //Color endColor = startColor;
        //endColor.a = 0;

        //while (t < 1)
        //{
        //    text.color -= new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, endColor.a, t));
        //    t += Time.deltaTime / 1000;
        //    yield return new WaitForSeconds(0.05f);
        //}
        errorText.SetActive(false);
        //text.color = startColor;
    }

}
