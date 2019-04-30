using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTiles : MonoBehaviour
{
    //2d array holding board data.
    public List<List<GameObject>> tiles = new List<List<GameObject>>(); 
    //All tile types.
    public int[] tileTypes; 
    //Holds available tiles for next tile spawn.
    public List<int> currentAvailableTiles = new List<int>(); 

    //Board Dimensions.
    public int width; 
    public int height;

    public GameObject currentTile;

    //List of all tile types.
    public GameObject[] models;

    void Start()
    {
        //Iterate through board and spawn a random tile at each slot.
        for (int x = 0; x < width; x++)
        {
            tiles.Add(new List<GameObject>());
            for (int y = 0; y < height; y++)
            {
                //Ensure no two tiles of the same type spawn next to eachother.
                currentAvailableTiles.Clear();
                for (int i = 0; i < tileTypes.Length; i++)
                {
                    if (x == 0 || tiles[Mathf.Clamp(x - 1, 0, 5)][y].GetComponent<TileSwapper>().tileType != i)
                    {
                        if (y == 0 || tiles[x][Mathf.Clamp(y - 1, 0, 5)].GetComponent<TileSwapper>().tileType != i)
                        {
                            currentAvailableTiles.Add(tileTypes[i]);
                        }
                    }
                }

                //Assign tile type to tile.
                int v = currentAvailableTiles[Random.Range(0, currentAvailableTiles.Count)];
                tiles[x].Add(Instantiate(models[v], new Vector3(x - (width / 2 - 0.5f), y, 0), Quaternion.identity));
                tiles[x][y].transform.parent = transform;
            }
        }
    }

    void Update()
    {
        //Replace destroyed tiles by checking top row.
        for (float i = -2.5f; i < 3; i++)
        {
            //Raycast for top tile, if it's not there then spawn a new one.
            if (!Physics.Raycast(new Vector3(i, 1f, 0), Vector3.down, 1f) && !Physics.Raycast(new Vector3(i, 0f, 0), Vector3.up, 1f)) 
            {
                int v = Random.Range(0, tileTypes.Length);
                currentTile = Instantiate(models[v], new Vector3(i, 0.5f, 0), Quaternion.identity);
                currentTile.transform.parent = transform;
            }
        }
    }
}
