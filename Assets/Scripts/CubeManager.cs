using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public Button _play;
    public Button _playCubesCombination;
    public Button _playGenerateWalls;

    public GameObject CubePrefab;
    public GameObject GreenCubePrefab;
    public GameObject YellowCubePrefab;
    public GameObject[] cubes;

    public int width = 10;
    public int height = 10;
    public float emptyCellProbability = 0.05f;
    public float adjacentWallProbability = 0.25f;
    public GameObject[,] walls;
    // Start is called before the first frame update
    void Start()
    {
        _play.onClick.AddListener(() => { ClickPlay(1); });
        _playCubesCombination.onClick.AddListener(() => { ClickPlay(2); });
        _playGenerateWalls.onClick.AddListener(GenerateWalls);
    }

    void InitializeWalls()
    {
        walls = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                walls[x, y] = null;
            }
        }
    }

    void GenerateWalls()
    {
        InitializeWalls();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                bool hasAdjacentWall = false;

                for (int cx = -1; cx <= 1; cx++)
                {
                    for (int cy = -1; cy <= 1; cy++)
                    {
                        if (cx == 0 && cy == 0)
                        {
                            continue;
                        }

                        int checkX = x + cx;
                        int checkY = z + cy;

                        if (checkX < 0 || checkX >= width || checkY < 0 || checkY >= height)
                        {
                            continue;
                        }

                        if (walls[checkX, checkY] != null)
                        {
                            hasAdjacentWall = true;
                            break;
                        }
                    }
                }

                if (hasAdjacentWall)  
                {
                    if (Random.value < adjacentWallProbability)
                    {
                        GameObject wall = Instantiate(CubePrefab, new Vector3(x * 1.1f, 1.1f, z * 1.1f), Quaternion.identity);
                        walls[x, z] = wall;
                    }
                } else if (Random.value < emptyCellProbability)
                {
                    GameObject wall = Instantiate(CubePrefab, new Vector3(x * 1.1f, 1.1f, z * 1.1f), Quaternion.identity);
                    walls[x, z] = wall;
                }
            }
        }
    }

    void ClickPlay(int flag)
    {
        if (cubes.Length > 0)
        {
            foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject CubeInst = CubePrefab;

                if (flag == 2)
                {
                    float randomValue = Random.value;

                    if (randomValue <= 0.7f)
                    {
                        CubeInst = GreenCubePrefab;
                    }
                    else
                    {
                        CubeInst = YellowCubePrefab;
                    }
                }
                CubeInst.name = "Cube" + (x * width + z);
                CubeInst.tag = "Cube";
                GameObject cube = Instantiate(CubeInst, new Vector3(x * 1.1f, 0, z * 1.1f), Quaternion.identity);
            }
        }
        cubes = GameObject.FindGameObjectsWithTag("Cube");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
