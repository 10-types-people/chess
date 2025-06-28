using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    string gameObjectName;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PrintLog()
    {
        Debug.Log(gameObjectName+"button was Clicked");
    }
    
}
