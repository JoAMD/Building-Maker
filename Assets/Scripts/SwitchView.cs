﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchView : MonoBehaviour
{

    public List<GameObject> rooms;
    public GameObject ceiling_view;
    private bool i = true;

    public void SwitchViewBtn()
    {
        if (i)
        {
            ToggleRooms(false);

            ToggleAllColliders(true);

            ceiling_view.SetActive(true);
            GameManager.instance._cameraCurr = GameManager.instance._ceilingCamBelow;
        }
        else
        {
            ToggleRooms(true);

            ToggleAllColliders(false);

            ceiling_view.SetActive(false);
            GameManager.instance._cameraCurr = GameManager.instance._roomCam;
        }
        i = !i;
    }

    private void ToggleRooms(bool isEnabled)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetActive(isEnabled);
            rooms[i].transform.parent?.GetComponent<RoomReferences>()?._camera?.SetActive(!isEnabled);
        }
    }

    private void ToggleAllColliders(bool isEnabled)
    {
        foreach (Collider col in FindObjectsOfType<Collider>())
        {
            col.enabled = isEnabled;
            //col.gameObject.SetActive(isEnabled);
        }
    } 

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
