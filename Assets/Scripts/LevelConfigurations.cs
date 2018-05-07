using System.Collections;
using UnityEngine;


public class LevelConfigurations : MonoBehaviour {

    public static Hashtable levelConfig;

    public static Hashtable getLevelConfig(int level)
    {
        levelConfig = new Hashtable();
        float sideLength;

        ArrayList BlockLocations;
        ArrayList triangularBlockLocations;

        // LEVEL1
        sideLength = 9.0f;
        levelConfig.Add(1, makeLevelDict(new Vector2(0, 0), new Vector2(sideLength - 1, 0), 1, sideLength, new Vector2(1, 1)));

        // LEVEL2
        sideLength = 5.0f;
        BlockLocations = new ArrayList();
        triangularBlockLocations = new ArrayList();

        for (int j = 0; j < (int)sideLength - 1; j++)
        {
            BlockLocations.Add(new Vector2(sideLength-2, j));
        }
       
        levelConfig.Add(2, makeLevelDict(new Vector2(0, 0), new Vector2(sideLength - 1, 0), 2, sideLength, new Vector2(0, 1), BlockLocations, triangularBlockLocations));

        // LEVEL3
        sideLength = 9.0f;
        BlockLocations = new ArrayList();
        triangularBlockLocations = new ArrayList();
        BlockLocations.Add(new Vector2((int)sideLength / 2, (int)sideLength / 2));
        levelConfig.Add(3, makeLevelDict(new Vector2(0, (int)sideLength / 2), new Vector2(sideLength - 1, (int)sideLength / 2), 3, sideLength, new Vector2(1, 0), BlockLocations, triangularBlockLocations));

        // LEVEL4
        sideLength = 7.0f;
        BlockLocations = new ArrayList();
        triangularBlockLocations = new ArrayList();
        for (int k = 1; k < (int)sideLength - 1; k++)
        {
            triangularBlockLocations.Add(System.Tuple.Create<Vector2, float>(new Vector2(k, k), 90));
        }

        for (int k = 1; k < (int)sideLength - 1; k++)
        {
            triangularBlockLocations.Add(System.Tuple.Create<Vector2, float>(new Vector2(k+1, k-1), 270));
        }

        levelConfig.Add(4, makeLevelDict(new Vector2(0, 0), new Vector2(0, sideLength - 1), 3, sideLength, new Vector2(1, 0), BlockLocations, triangularBlockLocations));

        return (Hashtable) levelConfig[level];
    }

    public static Hashtable makeLevelDict(Vector2 startLoc, Vector2 endLoc, int maxBlocks, float screenSideLength, Vector2 ballVelocity)
    {
        Hashtable levelInfo = new Hashtable();

        levelInfo.Add("startLoc", startLoc);
        levelInfo.Add("endLoc", endLoc);
        levelInfo.Add("maxBlocks", maxBlocks);
        levelInfo.Add("screenSideLength", screenSideLength);
        levelInfo.Add("ballVelocity", ballVelocity);
        return levelInfo;
    }

    public static Hashtable makeLevelDict(Vector2 startLoc, Vector2 endLoc, int maxBlocks, float screenSideLength, Vector2 ballVelocity, ArrayList BlockLocations, ArrayList triangularBlockLocations)
    {
        Hashtable levelInfo = makeLevelDict(startLoc, endLoc, maxBlocks, screenSideLength, ballVelocity);
        levelInfo.Add("BlockLocations", BlockLocations);
        levelInfo.Add("triangularBlockLocations", triangularBlockLocations);
        return levelInfo;
    }



}
