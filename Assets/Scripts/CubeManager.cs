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

    public GameObject CubePrefab;
    public GameObject GreenCubePrefab;
    public GameObject YellowCubePrefab;
    public GameObject[] cubes;

    public int width = 10;
    public int height = 10;
    // Start is called before the first frame update
    void Start()
    {
        _play.onClick.AddListener(() => { ClickPlay(1); });
        _playCubesCombination.onClick.AddListener(() => { ClickPlay(2); });
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
