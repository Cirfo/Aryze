using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Hierarchy & objects
    public GameObject mainCamera;
    public GameObject boardHolder;
    public GameObject dicesHolder;
    public GameObject counselorHolder;
    public GameObject armyHolder;
    public GameObject seasonHolder;
    public GameObject solHolder;


    [HideInInspector] public CameraController cameraController;

    // Code variables
    public static bool gameRunning = true;
    [HideInInspector] public GameObject turnPlayer; // current turn's player
    [HideInInspector] public List<GameObject> playersList;

    void Awake()
    {
        cameraController = mainCamera.GetComponent<CameraController>();
    }

    void Start()
    {
        InitGame();
    }

    void InitGame(){
        InitPlayers();
        InitArmy();
    }

    void InitPlayers(){

    }

    void InitArmy(){
        int i = -2;
        foreach (Transform army in armyHolder.transform)
        {
            army.GetChild(0).GetComponent<TextMeshPro>().text = i.ToString();
            i++;
        }
    }

    void InitVariables()
    {
        
    }

    void LoadSettings()
    {
        
    }
}