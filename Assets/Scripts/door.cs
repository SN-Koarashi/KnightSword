using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        Scene currentScene = gameObject.scene;

        if(currentScene.name == "boss"){
            SceneManager.LoadScene("main");
        }
        else{
            SceneManager.LoadScene("boss");
        }
    }
}
