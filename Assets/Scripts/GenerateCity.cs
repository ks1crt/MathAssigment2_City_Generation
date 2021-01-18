using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCity : MonoBehaviour
{
    [Header("Map Objects")]
    public GameObject[] cityObjects;
    public GameObject xCordinate;
    public GameObject YCordinate;
    public GameObject CrossSection;
    Vector3 variance;
    [Header("Map Data")]
    [Range(0, 250)]
    public int mapWidth = 200;
    [Range(0, 250)]
    public int mapHeight = 200;
    [Range(0, 1000)]
    public float seed = 0;
    [Range(0, 100)]
    public int xRoadNumber = 50;
    [Range(0, 100)]
    public int zRoadNumber = 50;
    [Range(0, 100)]
    public int xRoadMin = 3;
    [Range(0, 100)]
    public int xRoadMax = 3;
    [Range(0, 100)]
    public int zRoadMin = 2;
    [Range(0, 100)]
    public int zRoadMax = 20;
    [Range(0, 5)]
    public int spacing = 4;

    protected bool enableRandomSeed = true;
    [HideInInspector]
    public int[,] map;


    void Start()
    {
        GenaratePerlinNoiseData();
        GenerateRoadData();
        GenerateCityBlocks();
    }

    private void GenerateRoadData()
    {
        int x = 0;
        for (int amt = 0; amt < xRoadNumber; amt++)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                map[x, i] = -1;
            }
            x += Random.Range(xRoadMin, xRoadMax);
            if (x >= mapWidth) break;
        }

        int z = 0;
        for (int i = 0; i < zRoadNumber; i++)
        {
            for (int Width = 0; Width < mapWidth; Width++)
            {
                if (map[Width, z] == -1)
                {
                    map[Width, z] = -3;
                }
                else
                {
                    map[Width, z] = -2;
                }
            }
            z += Random.Range(zRoadMin, zRoadMax);
            if (z >= mapHeight) break;
        }
    }

    public void GenaratePerlinNoiseData()
    {
        map = new int[mapWidth, mapHeight];
        if (enableRandomSeed)
        {
            seed = Random.Range(0, 1000);
        }

        for (int Height = 0; Height < mapHeight; Height++)
        {
            for (int Width = 0; Width < mapWidth; Width++)
            {
                map[Width, Height] = (int)(Mathf.PerlinNoise(Width / 10.0f + seed, Height / 10.0f + seed) * 10);
            }
        }
    }

    public Vector3 RandomVariance()
    {
      var currentVariance = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        return currentVariance;
    }
    private void GenerateCityBlocks()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                int location = map[j, i];
                Vector3 pos = new Vector3(j * spacing, 0, i * spacing);

                if (location < -2) { Instantiate(CrossSection, pos, CrossSection.transform.rotation); }
                else if (location < -1) { Instantiate(xCordinate, pos, xCordinate.transform.rotation); }
                else if (location < 0) { Instantiate(YCordinate, pos, YCordinate.transform.rotation); }
                else if (location < 1) { variance = new Vector3(0, 0, 0); variance = RandomVariance(); Instantiate(cityObjects[0], pos + variance, Quaternion.identity); }
                else if (location < 2) { Quaternion scraperRot = Quaternion.Euler(0, -90, 0); variance = RandomVariance(); Instantiate(cityObjects[1], pos + variance, Quaternion.identity * scraperRot); }
                else if (location < 4) { variance = RandomVariance(); Instantiate(cityObjects[2], pos + variance, Quaternion.identity);}
                else if (location < 6) { variance = RandomVariance(); Instantiate(cityObjects[3], pos + variance, Quaternion.identity);}
                else if (location < 7) { variance = RandomVariance(); Instantiate(cityObjects[4], pos + variance, Quaternion.identity);}
                else if (location < 10) { variance = RandomVariance(); Instantiate(cityObjects[5], pos + variance, Quaternion.identity);}
            }
        }
    }
}
