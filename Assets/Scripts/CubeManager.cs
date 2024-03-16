using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public Button _play;
    public Button _playCubesCombination;
    public Button _playGenerateWalls;
    public Button _personageSpawn;

    public GameObject CubePrefab;
    public GameObject GreenCubePrefab;
    public GameObject YellowCubePrefab;
    public GameObject PersonageCapsulePrefab;
    public GameObject[] cubes;
    public GameObject[,] walls;
    public GameObject capsule;

    public GameObject navMeshSurface;

    public int width = 10;
    public int height = 10;
    public float emptyCellProbability = 0.05f;
    public float adjacentWallProbability = 0.25f;

    public float grassSpeed = 1f;
    public float sandSpeed = 0.5f; 

    private int capsuleX = -1;
    private int capsuleZ = -1;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(navMeshSurface);
        _play.onClick.AddListener(() => { GenerateMap(false); });
        _playCubesCombination.onClick.AddListener(() => { GenerateMap(true); });
        _playGenerateWalls.onClick.AddListener(GenerateWalls);
        _personageSpawn.onClick.AddListener(GeneratePersonageSpawn);
    }

    void GeneratePersonageSpawn()
    {
        if (capsule != null)
        {
            Destroy(capsule);
        }
        capsuleX = UnityEngine.Random.Range(0, width);
        capsuleZ = UnityEngine.Random.Range(0, height);
        if (walls == null || 
            walls[capsuleX, capsuleZ] == null)
        {
            PersonageCapsulePrefab.name = "Capsule";
            capsule = Instantiate(PersonageCapsulePrefab, new Vector3(capsuleX * 1.1f, 1.5f, capsuleZ * 1.1f), Quaternion.identity);
            capsule.AddComponent<Mover>();
        } else {
            GeneratePersonageSpawn();
        }
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
        if (walls != null)
        {
            foreach (GameObject cube in walls)
            {
                Destroy(cube);
            }
        }
        
        InitializeWalls();

        //int i = 0;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (capsuleZ == z || capsuleX == x) continue;
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

                float randomValue = UnityEngine.Random.value;
                if (hasAdjacentWall)  
                {
                    if (randomValue < adjacentWallProbability)
                    {
                        CubePrefab.name = "Wall";
                        GameObject wall = Instantiate(CubePrefab, new Vector3(x * 1.1f, 1.1f, z * 1.1f), Quaternion.identity);
                        wall.AddComponent<BoxCollider>();
                        wall.AddComponent<NavMeshObstacle>();
                        wall.GetComponent<NavMeshObstacle>().carving = true;
                        walls[x, z] = wall;
                    }
                } else if (randomValue < emptyCellProbability)
                {
                    CubePrefab.name = "Wall";
                    GameObject wall = Instantiate(CubePrefab, new Vector3(x * 1.1f, 1.1f, z * 1.1f), Quaternion.identity);
                    wall.AddComponent<BoxCollider>();
                    wall.AddComponent<NavMeshObstacle>();
                    wall.GetComponent<NavMeshObstacle>().carving = true;
                    walls[x, z] = wall;
                }
            }
        }
    }

    void GenerateMap(bool useDifferentBinomes = false)
    {
        if (cubes != null && cubes.Length > 0)
        {
            foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
        }

        cubes = new GameObject[width * height];

        int i = 0;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject CubeInst = CubePrefab;

                if (useDifferentBinomes)
                {
                    float randomValue = UnityEngine.Random.value;

                    if (randomValue <= 0.7f)
                    {
                        CubeInst = GreenCubePrefab;
                    }
                    else
                    {
                        CubeInst = YellowCubePrefab;
                    }
                }
                CubeInst.name = "Cube " + (i);
                CubeInst.tag = "Terra";
                GameObject cube = Instantiate(CubeInst, new Vector3(x * 1.1f, 0, z * 1.1f), Quaternion.identity);
                cube.transform.SetParent(navMeshSurface.transform);
                if (CubeInst == GreenCubePrefab)
                {
                    cube.layer = LayerMask.NameToLayer("Grass");
                } else
                {
                    cube.layer = LayerMask.NameToLayer("Sand");
                }
                
                cubes[i++] = cube;
            }
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetMouseButtonDown(0)) // При клике левой кнопкой мыши
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Проверяем, что кликнули на ландшафт
                if (hit.collider.CompareTag("Terra"))
                {
                    // Получаем точку, на которую кликнули
                    Vector3 targetPoint = hit.point;

                    // Вычисляем расстояние до точки
                    float distance = Vector3.Distance(transform.position, targetPoint);

                    // Выбираем скорость в зависимости от типа поверхности (трава или песок)
                    //float speed = hit.collider.CompareTag("Grass") ? grassSpeed : sandSpeed;
                    float speed = 1f; 

                    // Перемещаем персонажа к точке
                    transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
                }
            }
        }
    }*/
}

