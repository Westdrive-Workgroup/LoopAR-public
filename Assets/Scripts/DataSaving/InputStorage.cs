using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputStorage : MonoBehaviour
{

    
    
    public void OnClicked(Button button)
    {
        Debug.Log(button.name);
    }
    
    
    //records all Inputs 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        string keyPressed = Input.inputString;

        Debug.Log(keyPressed);
   
    }
}
