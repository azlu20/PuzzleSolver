using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[,] grid = new GameObject[6, 5];
    public GameObject prefab;
    private float x_offset = -3;
    private float y_offset = -2;
    void Start()
    {

   /*     for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[i, y] = (GameObject)Instantiate(prefab, new Vector3(i + x_offset, y + y_offset, 0), Quaternion.identity);
            }
        }
   */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
