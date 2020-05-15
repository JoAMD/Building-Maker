using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Switching : Prop
{

    public Texture tex1;
    public Texture tex0;
    public bool i = true;
    private Material new_mat;
    private MeshRenderer meshRenderer;
    private Transform parent;

    public override void Start()
    {
        base.Start();
        //tex0 = transform.parent.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
        new_mat = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        new_mat.mainTexture = tex0;
        parent = transform.parent;
        meshRenderer = parent.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new_mat;
        
        if (GameManager.instance.states.Count > GameManager.instance.ctr)
        {
            Debug.Log("isEmulated = " + isEmulated);
            if (isEmulated)
            {
                Debug.Log("Emulated so no need of getting from gm states");
            }
            else
            {
                i = GameManager.instance.states[GameManager.instance.ctr];
            }
        }
        SwitchLights();
        isDoneStart = true;
    }

    public void OnMouseDown()
    {
        StartCoroutine(SwitchLights());
    }

    public IEnumerator SwitchLights()
    {
        yield return new WaitForSeconds(waitTimeForDrag);
        yield return new WaitForEndOfFrame();
        if (!isMouseDragStart)
        {
            yield break;
        }
        
        if (i)
        {
            meshRenderer.sharedMaterial.mainTexture = tex1;
            parent.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            meshRenderer.sharedMaterial.mainTexture = tex0;
            parent.GetChild(0).gameObject.SetActive(true);
        }

        if (Plugin.instance.jc != null)
        {
            Plugin.instance.jc.Call("publish", gameObject.name[2]);
        }
        else
        {
            Debug.LogWarning("jc null!");
        }
        i = !i;
    }
}
