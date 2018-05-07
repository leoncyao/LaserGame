using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelButton : MonoBehaviour {

    public int level;
    public Text levelText;
    public bool clicked; 

    public Canvas canvas;

    private void Start()
    {
        clicked = false;
    }

    public void thisDoesntMakeSense(Button button)
    {
        clicked = true;
    }


}
