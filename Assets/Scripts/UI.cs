using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static bool clicked;
    public static bool started;
    public static bool ended;

    public GameObject winMenu;
    public static UI Instance;


    void Start()
    {
        clicked = false;
        started = false;
        ended = false;
        Instance = this;
    }

    public void startStop(Button button)
    {
        if (!clicked)
        {
            button.GetComponentInChildren<Text>().text = "stop";
            clicked = true;
            started = true;

        }
        else if (clicked)
        {
            button.GetComponentInChildren<Text>().text = "start";
            clicked = false;
            ended = true;
        }
    }

    public void spawnWinMenu()
    {
        GameObject win = Instantiate(winMenu, this.transform);
        
    }


}
