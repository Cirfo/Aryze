using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Constants;
using Random = UnityEngine.Random;

public class Counselor : MonoBehaviour
{
    public int cost;
    public string name;
    public bool isTaken;
    public bool isSpecial;
    public TextMeshPro body_text;
    public TextMeshPro title_text;
    public List<ResourceType> resources;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitCounselor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitCounselor(){
        AssembleText();
    }

    void InitVariables(){
        
    }

    void AssembleText(){
        title_text.text = name;

        if(isSpecial){
            SpecialBuilder();
        }
        if(resources.Count > 0){
                body_text.text = "";
                foreach (ResourceType r in resources)
                {
                    body_text.text += r.ToString().FirstOrDefault() + "  ";
                }
                body_text.text += "\n\n"+cost;
            }
    }

    void SpecialBuilder(){
        cost = Random.Range(special_couns_min_cost, special_couns_max_cost + 1);
        short resourceNum = (short) Random.Range(special_couns_min_res_count, special_couns_max_res_count + 1);
        resources.Clear();
        for (int i = 0; i < resourceNum; i++)
        {
            resources.Add((ResourceType) Random.Range(0, 6));
        }
    }
}
