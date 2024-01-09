using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public Button _play;
    
    public GameObject CubePrefab;
    public int width = 10;
    public int height = 10;
    // Start is called before the first frame update
    void Start()
    {
        _play.onClick.AddListener(ClickPlay);
    }

    void ClickPlay()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                CubePrefab.name = "Cube " + (x * width + z);
                GameObject cube = Instantiate(CubePrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
