using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{


    // screenSideLength will decide the proportion of screen and all objects (not sure if needs to be a float)
    private float screenSideLength = 10f;
    private const float LASERSPEED = 5f, TILE_SIZE = 1, TILE_OFFSET = 0.5f;

    public Vector2 ballVelocity;
    public bool simulating, running;


    // For moving data around, as I don't want to keep static methods 
    public static Main Instance; 

    Vector2 startLoc, endLoc;
    public int receiverRequirements;
    private Vector3 cameraPos;
    public GameObject blockPrefab, ballPrefab, arrowPrefab, receiverPrefab, triangularBlockPrefab, triangularBlockSprite, blockSprite;

    // Might have multiple receivers later

    public GameObject receiver1;
    public Material lineRendererMaterial;

    public int maxBlocks;


    // List of current game Objects created by PLAYER
    public ArrayList currentObjects;
    public ArrayList currentSprites;
    // Current in game balls created in simulation
    public ArrayList currentBalls;

    public GameObject canvas;

    void Start()
    {


        simulating = false;
        running = true;
        Instance = this;

        currentObjects = new ArrayList();
        currentSprites = new ArrayList();
        currentBalls = new ArrayList();

        GameObject temp = GameObject.FindGameObjectWithTag("Level");
        if (temp != null)
        {
            int levelNumber = temp.GetComponent<LevelButton>().level;
            Hashtable levelInfo = LevelConfigurations.getLevelConfig(levelNumber);
            setLevelValues(levelInfo);
            
        }
        Destroy(temp);

        //TestSpawnBlock();
        //TestSpawnArrow();
        //TestSpawnLaser();
        //TestPhysics();
        //TestReceiver();
        //TestTriangularBlock();

        intialConfig();
        setUpGrid();

    } 

    void setLevelValues(Hashtable levelValues)
    {
        this.startLoc = new Vector2(0, 0);
        this.endLoc = new Vector2(0, 0);
        // not sure if I should have a default value for this
        this.maxBlocks = 1;
        this.screenSideLength = 7f;
        this.ballVelocity = new Vector2(1, 1);

        if (levelValues.ContainsKey("startLoc"))
            this.startLoc = (Vector2) levelValues["startLoc"];

        if (levelValues.ContainsKey("endLoc"))
        {
            this.endLoc = (Vector2)levelValues["endLoc"];
        }

        if (levelValues.ContainsKey("maxBlocks"))
        {
            this.maxBlocks = (int)levelValues["maxBlocks"];
        }

        if (levelValues.ContainsKey("screenSideLength"))
        {
            this.screenSideLength = (float) levelValues["screenSideLength"];
        }

        if (levelValues.ContainsKey("ballVelocity"))
        {
            this.ballVelocity = (Vector2)levelValues["ballVelocity"];
        }
        ballVelocity *= LASERSPEED;

        if (levelValues.ContainsKey("BlockLocations"))
        {
            //print(levelValues["BlockLocations"]);    
            foreach (Vector2 temp in (ArrayList) levelValues["BlockLocations"])
            {
                spawnBlock(temp);
            }
        }
        
        
        if (levelValues.ContainsKey("triangularBlockLocations"))
        {
            print("check");
            foreach (System.Tuple<Vector2,float> temp in (ArrayList)levelValues["triangularBlockLocations"])
            {
                spawnTriangularBlock(temp.Item1, temp.Item2);
            }
        }

        //values to set Later
        this.receiverRequirements = 1;
    }

    void intialConfig()
    {
        spawnStartArrow(startLoc);
        spawnReceiver(endLoc);
    }

    void Update()
    {
        if (running)
        {
            DrawGrid();
            // If I recieve the signal from start button to start simulation
            if (UI.started)
            {
                UI.started = false;
                startSimulation();
                simulating = true; // might want to put this inside start simulation
            }
            if (simulating)
            {
                // If the receivers conditions are met
                if (checkReciever())
                {
                    stopSimulation();
                    UI.Instance.spawnWinMenu();
                    running = false;
                }

                // If i recieve the signal from END button to stop simulation
                if (UI.ended)
                {
                    UI.ended = false;
                    stopSimulation();
                    simulating = false;
                }
            }
        }
    }

    void startSimulation()
    {
        //makes a laser spawn from start Location
        spawnLaser(startLoc);
        // will initate moving blocks 
    }

    private void destroyEverything()
    {
        destroyAllObjects();
        destroyAllBalls();
        destroyAllSprites();
    }
    void destroyAllObjects()
    {
        foreach (GameObject temp in currentObjects)
        {
            Destroy(temp);
        }
        currentObjects = new ArrayList();
    }
    void destroyAllBalls()
    {
        foreach (GameObject temp in currentBalls)
        {
            Destroy(temp);
        }
        currentBalls = new ArrayList();
    }
    void destroyAllSprites()
    {
        foreach (GameObject temp in currentSprites)
        {
            Destroy(temp);
        }
        currentSprites = new ArrayList();
    }
    void stopSimulation()
    {
        // Will reset config if needed
        // reset receiver requirements
        destroyAllBalls();
    }
    bool checkReciever()
    {
        // Eventually will change to a foreach loop that checks all receivers in a List<receivers>
        return receiver1.GetComponent<Receiver>().completed;
    }

    private void DrawGrid()
    {
        // If I ever need to scale based on player view range
        float scale = 1;
        Vector2 widthLine = Vector2.right * scale * screenSideLength * TILE_SIZE;
        Vector2 heightLine = Vector2.up * scale * screenSideLength * TILE_SIZE;

        
        for (int i = 0; i <= scale * screenSideLength; i += 1)
        {
            Vector2 start = Vector2.right * i;
            Debug.DrawLine(start, start + heightLine, Color.white, 1, true);
        }

        for (int j = 0; j <= scale * screenSideLength; j += 1)
        {
            Vector2 start = Vector2.up * j;
            Debug.DrawLine(start, start + widthLine, Color.white, 1, true);
        }
    }

    private void setUpGrid()
    {
        // If I ever need to scale based on player input
        float scale = 1;
        Vector2 widthLine = Vector2.right * scale * screenSideLength * TILE_SIZE;
        Vector2 heightLine = Vector2.up * scale * screenSideLength * TILE_SIZE;

        GameObject line = new GameObject();
        line.name = "LineMaker";
        line.AddComponent<LineRenderer>();
        LineRenderer temp = line.GetComponent<LineRenderer>();
        temp.startWidth = 0.1f;
        temp.endWidth = 0.1f;
        temp.positionCount = (int)screenSideLength*10;

        temp.material = lineRendererMaterial;
        int j = 0;
        float a = 0.2f;
        float b = 0.0f;
        float c = 0.2f;
        AnimationCurve curve = new AnimationCurve();
        for (int i = 0; i <= scale * screenSideLength; i += 1)
        {
            Vector2 start = Vector2.right * i;
            temp.SetPosition(j, start);
            temp.SetPosition(j+1, start + heightLine);
            temp.SetPosition(j+2, start);
            curve.AddKey(j, a);
            curve.AddKey(j, b);
            curve.AddKey(j, c);
            j += 3;
        }
        j += 1;
        for (int k = 0; k <= scale * screenSideLength; k += 1)
        {
            Vector2 start = Vector2.up * k;
            temp.SetPosition(j, start);
            temp.SetPosition(j + 1, start + widthLine);
            temp.SetPosition(j + 2, start);
            j += 3;
            curve.AddKey(j, a);
            curve.AddKey(j, b);
            curve.AddKey(j, c);
        }
        //temp.widthCurve = curve;
    }

    public void spawnStartArrow(Vector2 loc)
    {
        // Spawns Arrow to indicate position and direction of spawned Lasers
        double angle = Tools.findAngle(0, 0, ballVelocity.x, ballVelocity.y);
        Quaternion temp = Quaternion.Euler(-1.0f * (float)angle, 90, 0);

        //offSetVector 
        Vector2 offset = new Vector2(-1.5f, -1.5f);
        offset = Vector2.zero;

        GameObject arrow = Instantiate(arrowPrefab, offSetVector(loc), temp);
        arrow.transform.localScale = new Vector3(0.1225f, 0.2f, 1f);
    }

    public void spawnLaser(Vector2 position)
    {
        GameObject newBall = Instantiate(ballPrefab, offSetVector(position), Quaternion.identity);
        newBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        newBall.GetComponent<Laser>().startVelocity = ballVelocity;
        currentBalls.Add(newBall);
    }

    public void spawnReceiver(Vector2 position)
    {
        receiver1 = Instantiate(receiverPrefab, offSetVector(position), Quaternion.Euler(0, 0, 0));
    }

    public GameObject spawnBlock(Vector2 position)
    {
        GameObject newBlock = Instantiate(blockPrefab, offSetVector(position), Quaternion.Euler(0, 0, 0));
        currentObjects.Add(newBlock);
        return newBlock;
    }

    public void spawnBlockSprite(Vector3 position)
    {
        GameObject tempSprite = Instantiate(blockSprite, Camera.main.WorldToScreenPoint(offSetVector(position)), Quaternion.identity);
        tempSprite.transform.SetParent(canvas.transform);
        Vector3 UIposition = Camera.main.WorldToScreenPoint(offSetVector(position));
        tempSprite.transform.position = UIposition;
        tempSprite.GetComponent<Draggable>().attachedBlock = spawnBlock(position);
        RectTransform item2 = tempSprite.GetComponent<RectTransform>();
        Vector2 v1 = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
        Vector2 v2 = Camera.main.WorldToScreenPoint(new Vector2(1, 1));
        Vector2 tempScale = v2 - v1;
        item2.sizeDelta = tempScale;
        currentSprites.Add(tempSprite);
    }

    public void spawnTriangularBlockSprite(Vector2 position, float angle, Quaternion orientation)
    {
        GameObject tempSprite = Instantiate(triangularBlockSprite, Camera.main.WorldToScreenPoint(offSetVector(position)), orientation);
        tempSprite.transform.SetParent(canvas.transform);
        Vector3 UIposition = Camera.main.WorldToScreenPoint(offSetVector(position));
        tempSprite.transform.position = UIposition;
        tempSprite.GetComponent<DraggableTriangularBlock>().attachedBlock = spawnTriangularBlock(position, angle);
        tempSprite.GetComponent<DraggableTriangularBlock>().attachedBlock.GetComponent<TriangularBlock>().attachedSprite = tempSprite;
        RectTransform item2 = tempSprite.GetComponent<RectTransform>();
        Vector2 v1 = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
        Vector2 v2 = Camera.main.WorldToScreenPoint(new Vector2(1, 1));
        Vector2 tempScale = v2 - v1;
        item2.sizeDelta = tempScale;
        currentSprites.Add(tempSprite);
    }
    public GameObject spawnTriangularBlock(Vector2 position, float test)
    {
        GameObject newTriangularBlock = Instantiate(triangularBlockPrefab, offSetVector(position), Quaternion.Euler(test, 90, 90));

        // I wish I could understand Quaternions
        newTriangularBlock.transform.rotation = Quaternion.identity;
        newTriangularBlock.transform.rotation *= Quaternion.Euler(0, 0, 90);
        newTriangularBlock.transform.rotation *= Quaternion.Euler(90, 0, 0);
        newTriangularBlock.transform.rotation *= Quaternion.Euler(0, -test, 0);
        newTriangularBlock.transform.localScale = new Vector3(0.5f, 1, 0.5f);
        newTriangularBlock.GetComponent<TriangularBlock>().rotationAngle = test;
        currentObjects.Add(newTriangularBlock);
        return newTriangularBlock;
    }
    public int getNumObjects()
    {
        return currentObjects.Count;
    }

    public void destroyObject(GameObject temp) 
    {
        Destroy(temp);
    }

    private Vector2 offSetVector(Vector2 temp)
    {
        Vector2 origin = Vector2.zero;
        origin.x = temp.x * TILE_SIZE + TILE_OFFSET;
        origin.y = temp.y * TILE_SIZE + TILE_OFFSET;
        return origin;
    }

    public float getScreenSideLength()
    {
        return screenSideLength;
    }

    public void TestSpawnBlock()
    {
        // A block should spawn in center of screen
        spawnBlock(new Vector2(screenSideLength/2, screenSideLength/2));
    }

    public void TestSpawnArrow()
    {
        // An arrow should spawn in center of screen
        spawnStartArrow(new Vector2(screenSideLength / 2, screenSideLength / 2));
    }

    public void TestSpawnLaser(float centerX, float centerY) {
        Vector2 ballLoc;

        float a = 2.0f;
        float b = 1.5f;

        //print("NorthEast");
        //Instance.ballVelocity = new Vector2(-1, -1);
        //ballLoc = new Vector2(centerX + a, centerY + b);
        //spawnLaser(ballLoc);

        //print("SouthEast");
        //Instance.ballVelocity = new Vector2(-1, 1);
        //ballLoc = new Vector2(centerX + a, centerY - b);
        //spawnLaser(ballLoc);

        //print("NorthWest");
        //Instance.ballVelocity = new Vector2(1, -1);
        //ballLoc = new Vector2(centerX - a, centerY + b);
        //spawnLaser(ballLoc);

        //print("SouthWest");
        //Instance.ballVelocity = new Vector2(1, 1);
        //ballLoc = new Vector2(centerX - a, centerY - b);
        //spawnLaser(ballLoc);

        //print("North");
        //Instance.ballVelocity = new Vector2(0, -1);
        //ballLoc = new Vector2(centerX, centerY + b);
        //spawnLaser(ballLoc);

        //print("South");
        //Instance.ballVelocity = new Vector2(0, 1);
        //ballLoc = new Vector2(centerX, centerY - b);
        //spawnLaser(ballLoc);

        //print("West");
        //Instance.ballVelocity = new Vector2(1, 0);
        //ballLoc = new Vector2(centerX - a, centerY);
        //spawnLaser(ballLoc);

        print("East");
        Instance.ballVelocity = new Vector2(-1, 0);
        ballLoc = new Vector2(centerX + a, centerY);
        spawnLaser(ballLoc);
    }

    public void TestPhysics()
    {
        // spawns 8 balls to bounce off a block at the center of the screen
        // balls do not collide with balls
        // block should remain in place, while eacg ball bounces elastically 
        Vector2 blockLoc;
        blockLoc = new Vector2(screenSideLength / 2, screenSideLength / 2);
        spawnBlock(blockLoc);

        TestSpawnLaser(screenSideLength / 2, screenSideLength / 2);

    }

    public void TestReceiver()
    {
        Vector2 ballLoc;
        Instance.ballVelocity = new Vector2(1, 0);
        ballLoc = new Vector2(screenSideLength / 2, screenSideLength / 2);
        spawnLaser(ballLoc);

        spawnReceiver(new Vector2(screenSideLength / 2 + 3, screenSideLength / 2));
        simulating = true;
    }
    
    public void TestTriangularBlock()
    {
        Camera.main.orthographicSize = 6;
        //receiver1.SetActive(false);
        Vector2 blockLoc;
        float a, b, c;

        a = -3;
        b = -3;
        c = screenSideLength / 2;
        blockLoc = new Vector2(c + a, c + b);
        spawnTriangularBlock(blockLoc, 0);
        TestSpawnLaser(c + a, c + b);

        //a = 3;
        //b = 3;
        //c = screenSideLength / 2;
        //blockLoc = new Vector2(c + a, c + b);
        //spawnTriangularBlock(blockLoc, 180);
        //TestSpawnLaser(c + a, c + b);

        //a = -3;
        //b = 3;
        //c = screenSideLength / 2;
        //blockLoc = new Vector2(c + a, c + b);
        //spawnTriangularBlock(blockLoc, 90);
        //TestSpawnLaser(c + a, c + b);

        //a = 3;
        //b = -3;
        //c = screenSideLength / 2;
        //blockLoc = new Vector2(c + a, c + b);
        //spawnTriangularBlock(blockLoc, 270);
        //TestSpawnLaser(c + a, c + b);

    }

}



