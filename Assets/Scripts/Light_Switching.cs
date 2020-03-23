using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Switching : MonoBehaviour
{

    public Texture tex1;
    public Texture tex0;
    public bool i = true;
    private Material new_mat;
    private MeshRenderer meshRenderer;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        //tex0 = transform.parent.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
        new_mat = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        new_mat.mainTexture = tex0;
        parent = transform.parent;
        meshRenderer = parent.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new_mat;

        i = GameManager.instance.states[GameManager.instance.ctr];
        SwitchLights();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        SwitchLights();
    }

    public void SwitchLights()
    {
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
        Plugin.instance.jc.Call("publish", gameObject.name[2]);
        i = !i;
    }
}
