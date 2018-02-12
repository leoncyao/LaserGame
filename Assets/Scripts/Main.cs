﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    // SCREENSIDELENGTH will decide the proportion of screen and all objects
    private const float SCREENSIDELENGTH = 7f;
    private const float LASERSPEED = 5f, TILE_SIZE = 1, TILE_OFFSET = 0.5f;

    public Vector2 ballVelocity;
    public bool simulating;
    public bool running;
    public static Main Instance;

    Vector2 startLoc, endLoc;
    public int receiverRequirements;
    private Vector3 cameraPos = new Vector3(SCREENSIDELENGTH/2, SCREENSIDELENGTH/2, -SCREENSIDELENGTH);
    public GameObject blockPrefab, ballPrefab, arrowPrefab, receiverPrefab, blockSprite;
    public GameObject mainLight;
    public GameObject canvas;
    // Might have multiple receivers later
    public GameObject receiver1;

    // List of current game Objects created by PLAYER
    public ArrayList currentObjects;
    // Current in game balls created in simulation
    public ArrayList currentBalls;

    void Start()
    {
        Camera.main.transform.position = cameraPos;
        Camera.main.orthographicSize = SCREENSIDELENGTH/2 + 1;
        mainLight.transform.position = cameraPos;

        simulating = false;
        running = true;
        startLoc = new Vector2(0, 0);
        endLoc = new Vector2(SCREENSIDELENGTH-1, 0);
        receiverRequirements = 1;


        ballVelocity = new Vector2(1*LASERSPEED, 1*LASERSPEED);
        intialConfig();
        Instance = this;

        //TestSpawnBlock();
        //TestSpawnArrow();
        //TestSpawnLaser();
        //TestPhysics();
        //TestReceiver();
        currentObjects = new ArrayList();
        currentBalls = new ArrayList();
    }

    void intialConfig()
    {
        spawnStartArrow(startLoc);
        spawnReceiver(endLoc);
        ////int numBlocks = 10; // make sure number of blocks in initial config is this value
        ////sets level space
        //Vector2[] blocksConfig = new Vector2[numBlocks];
        //for (int i = 0; i < numBlocks;i++)
        //{
        //    blocksConfig[i] = new Vector2(i, 0);
        //}
        //spawnBlocks(blocksConfig);
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

                // If i recieve the signal from end button to stop simulation
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
    }
    void destroyAllObjects()
    {
        foreach (GameObject temp in currentObjects)
        {
            Destroy(temp);
        }
        destroyAllBalls();
    }
    void destroyAllBalls()
    {
        foreach (GameObject temp in currentBalls)
        {
            Destroy(temp);
        }
        currentBalls = new ArrayList();
    }
    void stopSimulation()
    {
        destroyAllBalls();
    }

    bool checkReciever()
    {
        return receiver1.GetComponent<Receiver>().completed;
    }
    private void DrawGrid()
    {
        // If I ever need to scale based on player input
        float scale = 1;
        Vector2 widthLine = Vector2.right * scale * SCREENSIDELENGTH * TILE_SIZE;
        Vector2 heightLine = Vector2.up * scale * SCREENSIDELENGTH * TILE_SIZE;


        for (int i = 0; i <= scale * SCREENSIDELENGTH; i += 1)
        {
            Vector2 start = Vector2.right * i;
            Debug.DrawLine(start, start + heightLine, Color.white, 1, true);
        }

        for (int j = 0; j <= scale * SCREENSIDELENGTH; j += 1)
        {
            Vector2 start = Vector2.up * j;
            Debug.DrawLine(start, start + widthLine, Color.white, 1, true);

        }
    }
    public void spawnStartArrow(Vector2 loc)
    {
        // Spawns Arrow to indicate position and direction of spawned Lasers
        double angle = Tools.findAngle(0, 0, ballVelocity.x, ballVelocity.y);
        Quaternion temp = Quaternion.Euler(-1.0f * (float)angle, 90, 0);

        //offSetVector 
        Vector2 offset = new Vector2(-1.5f, -1.5f);

        GameObject arrow = Instantiate(arrowPrefab, loc + offset, temp);
        arrow.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
    }

    public GameObject spawnBlock(Vector2 position)
    {
        GameObject newBlock = Instantiate(blockPrefab, offSetVector(position), Quaternion.Euler(0, 0, 0));
        currentObjects.Add(newBlock);
        return newBlock;
    }

    public void spawnLaser(Vector2 position)
    {
        GameObject newBall = Instantiate(ballPrefab, offSetVector(position), Quaternion.identity);
        newBall.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        newBall.GetComponent<Laser>().startVelocity = ballVelocity;
        currentBalls.Add(newBall);

    }

    public void spawnReceiver(Vector2 position)
    {
        //GameObject newBlock = Instantiate(receiverPrefab, offSetVector(position), Quaternion.Euler(0, 0, 0));
        receiver1 = Instantiate(receiverPrefab, offSetVector(position), Quaternion.Euler(0, 0, 0));
    }

    public void spawnBlockSprite(Vector3 position)
    {

        GameObject tempSprite = Instantiate(blockSprite, Vector3.zero, Quaternion.identity);
        tempSprite.transform.SetParent(canvas.transform);
        Vector3 UIposition = Camera.main.WorldToScreenPoint(offSetVector(position));
        tempSprite.transform.position = UIposition;
        tempSprite.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        tempSprite.GetComponent<Draggable>().attachedBlock = spawnBlock(position);

        currentObjects.Add(tempSprite);


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

    public void TestSpawnBlock()
    {
        // A block should spawn in center of screen
        spawnBlock(new Vector2(SCREENSIDELENGTH/2, SCREENSIDELENGTH/2));
    }

    public void TestSpawnArrow()
    {
        // An arrow should spawn in center of screen
        spawnStartArrow(new Vector2(SCREENSIDELENGTH / 2, SCREENSIDELENGTH / 2));
    }

    public void TestSpawnLaser() {
        // A "Laser" should spawn in center of screen, moving diagonally top right
        spawnLaser(new Vector2(SCREENSIDELENGTH / 2, SCREENSIDELENGTH / 2));
    }

    public void TestPhysics()
    {
        // spawns 8 balls to bounce off a block at the center of the screen
        // balls do not collide with balls
        // block should remain in place, while eacg ball bounces elastically 
        Vector2 blockLoc;
        blockLoc = new Vector2(SCREENSIDELENGTH / 2, SCREENSIDELENGTH / 2);
        spawnBlock(blockLoc);

        Vector2 ballLoc;
        float a = 2.0f;
        float b = 1.5f;
        Instance.ballVelocity = new Vector2(-1, -1);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2 + a, SCREENSIDELENGTH / 2 + b);
        spawnLaser(ballLoc);

        Instance.ballVelocity = new Vector2(-1, 1);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2 + a, SCREENSIDELENGTH / 2 - b);
        spawnLaser(ballLoc);

        Instance.ballVelocity = new Vector2(1, -1);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2 - a, SCREENSIDELENGTH / 2 + b);
        spawnLaser(ballLoc);

        Instance.ballVelocity = new Vector2(1, 1);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2 - a, SCREENSIDELENGTH / 2 - b);
        spawnLaser(ballLoc);

        Instance.ballVelocity = new Vector2(1, 0);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2 - a, SCREENSIDELENGTH / 2);
        spawnLaser(ballLoc);

        Instance.ballVelocity = new Vector2(-1, 0);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2 + a, SCREENSIDELENGTH / 2);
        spawnLaser(ballLoc);

        Instance.ballVelocity = new Vector2(0, 1);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2, SCREENSIDELENGTH / 2 - b);
        spawnLaser(ballLoc);

        Instance.ballVelocity = new Vector2(0, -1);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2, SCREENSIDELENGTH / 2 + b);
        spawnLaser(ballLoc);

    }

    public void TestReceiver()
    {
        Vector2 ballLoc;
        float a = 2.0f;
        float b = 1.5f;
        Instance.ballVelocity = new Vector2(1, 0);
        ballLoc = new Vector2(SCREENSIDELENGTH / 2, SCREENSIDELENGTH / 2);
        spawnLaser(ballLoc);

        spawnReceiver(new Vector2(SCREENSIDELENGTH / 2 + 3, SCREENSIDELENGTH / 2));
        simulating = true;
    }
}



