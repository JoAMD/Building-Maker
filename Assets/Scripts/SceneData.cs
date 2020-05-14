using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneData : MonoBehaviour
{
    public Button switchViewBtn;

    public GameObject savedText;
    public GameObject errorText;

    public TextMeshProUGUI inputFieldText;

    public GameObject fileExists;
    public Button saveBtnMain;
    public Button loadBtnMain;

    public SpawnController lightSpawner;
    public SpawnController fanSpawner;
    public RoomSpawner roomSpawner;
}
