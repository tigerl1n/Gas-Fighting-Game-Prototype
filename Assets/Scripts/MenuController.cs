using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play(){
        SceneManager.LoadScene(1);
    }

    public void quit(){
        Application.Quit();
    }
}
