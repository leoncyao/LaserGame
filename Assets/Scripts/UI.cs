using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static bool clicked;
    public static bool started;
    public static bool ended;


    void Start()
    {
        clicked = false;
        started = false;
        ended = false;

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


}
