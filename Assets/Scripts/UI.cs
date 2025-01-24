using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject playerListHolder;

    [SerializeField]
    Button trial_button;
    [SerializeField]
    GameObject trialCube;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initiate buttons
    void InitButtons(){
        trial_button.onClick.AddListener(SpawnCube);
    }

    void SpawnCube(){
        Instantiate(trialCube, new Vector3(0, 4, 0), Quaternion.identity);
    }
}
