using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static bool clicked, started, ended;


    public static UI Instance;
    public GameObject ItemBar, slot, winMenu;

    public int blockQuantity;
    public GameObject BlockSprite;
    public GameObject triangularBlockSprite;

    public GameObject mainLight;


    void Start()
    {
        clicked = false;
        started = false;
        ended = false;
        Instance = this;

        Vector2 v1 = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
        Vector2 v2 = Camera.main.WorldToScreenPoint(new Vector2(1, 1));

        Vector2 tempScale = v2 - v1;
        Vector2 finalScale = new Vector2(2f*tempScale.x, 8*tempScale.y);

        RectTransform temp = ItemBar.GetComponent<RectTransform>();
        temp.sizeDelta = finalScale;
        temp.transform.position -= new Vector3(tempScale.x, 0, 0);
        
        RectTransform item1 = BlockSprite.GetComponent<RectTransform>();
        item1.sizeDelta = tempScale;

        RectTransform item2 = triangularBlockSprite.GetComponent<RectTransform>();
        item2.sizeDelta = tempScale;
        blockQuantity = Main.Instance.maxBlocks;

        float screenSideLength = Main.Instance.getScreenSideLength();
        Vector3 cameraPos = new Vector3(screenSideLength / 2, screenSideLength / 2, -screenSideLength);
        Camera.main.transform.position = cameraPos;
        Camera.main.orthographicSize = screenSideLength / 2 + 1;
        print(Camera.main.transform.position);
        mainLight.transform.position = cameraPos;
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

    public void returnToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void quit()
    {
        Application.Quit();
    }

    public void spawnWinMenu()
    {
        winMenu.SetActive(true);
        winMenu.transform.SetAsLastSibling();
    }

    void Update()
    {
        slot.transform.GetChild(1).GetComponent<Text>().text = "x" + blockQuantity.ToString();
    }


}
