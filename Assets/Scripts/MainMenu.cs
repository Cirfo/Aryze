using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button mutliplayer_button;
    [SerializeField]
    Button singleplayer_button;
    [SerializeField]
    Button joinbycode_button;
    [SerializeField]
    GameObject joinbycode_obj;
    [SerializeField]
    Button join_button;
    [SerializeField]
    Button joinbylist_button;
    [SerializeField]
    GameObject joinbylist_obj;
    [SerializeField]
    Button create_button;
    [SerializeField]
    GameObject create_obj;
    [SerializeField]
    Button back_button;

    MenuPos posInMenu;

    enum MenuPos{
        Opening,
        Single,
        Multi,
        Searching,
        Waiting,
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posInMenu = MenuPos.Opening;
        InitButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initiate buttons
    void InitButtons(){
        mutliplayer_button.onClick.AddListener(MultiplayerButton);
        back_button.onClick.AddListener(BackButton);
    }

    void LoadGame(){
        SceneManager.LoadScene(1);
    }

    void MultiplayerButton(){
        posInMenu = MenuPos.Multi;
        mutliplayer_button.gameObject.SetActive(false);
        singleplayer_button.gameObject.SetActive(false);
        join_button.gameObject.SetActive(true);
        joinbycode_obj.gameObject.SetActive(true);
        joinbylist_obj.gameObject.SetActive(true);
        create_obj.gameObject.SetActive(true);
    }

    void BackButton(){
        switch(posInMenu){
            case MenuPos.Single:
                break;
            case MenuPos.Multi:
            mutliplayer_button.gameObject.SetActive(true);
            singleplayer_button.gameObject.SetActive(true);
            join_button.gameObject.SetActive(false);
            joinbycode_obj.gameObject.SetActive(false);
            joinbylist_obj.gameObject.SetActive(false);
            create_obj.gameObject.SetActive(false);
                break;
            case MenuPos.Searching:
                break;
            case MenuPos.Waiting:
                break;
        }
    }

}
