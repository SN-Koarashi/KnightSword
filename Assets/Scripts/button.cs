using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onRestart()
    {
        SceneManager.LoadScene("start");
        GameManager.Instance.isDeath = false;
        GameManager.Instance.isWin = false;
        GameManager.Instance.canDamage = false;
        GameManager.Instance.SetPause(false);
    }

    public void onStart()
    {
        SceneManager.LoadScene("main");
    }

    public void onExit()
    {
        Application.Quit();
    }
}
