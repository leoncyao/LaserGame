using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {
    // SCREENSIDELENGTH will decide the proportion of screen and all objects
    private const float SCREENSIDELENGTH = 7f;
    private Vector3 cameraPos = new Vector3(SCREENSIDELENGTH / 2, SCREENSIDELENGTH / 2, -SCREENSIDELENGTH);
    public GameObject mainLight;
    public GameObject canvas;
    public GameObject levelButtonPrefab;
    public GameObject levelBar;
    // Can probably get num Levels?
    public const int numLevels = 4;

    public ArrayList activeButtons;

    // Use this for initialization
    void Start () {
        activeButtons = new ArrayList();
        Camera.main.transform.position = cameraPos;
        Camera.main.orthographicSize = SCREENSIDELENGTH / 2 + 1;
        mainLight.transform.position = cameraPos;

        for (int i = 1; i < numLevels+1; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, levelBar.transform);

            LevelButton temp = levelButton.GetComponent<LevelButton>();
            temp.level = i;
            temp.levelText.text = "Level " + i;
            activeButtons.Add(levelButton);
        }
    }
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject possibleButton in activeButtons)
        {
            LevelButton temp = possibleButton.GetComponent<LevelButton>();
            if (temp.clicked)
            {
                possibleButton.transform.SetParent(null);
                DontDestroyOnLoad(possibleButton);
                temp.clicked = false;
                goToLevel(temp.level);
            }
        }
	}



    public void goToLevel(int level)
    {
        SceneManager.LoadScene("MainScene");
    }


}
