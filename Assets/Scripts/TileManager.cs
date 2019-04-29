using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject currentlySelected;

    public GameObject tile;

    int frameCountF;
    int frameCount;

    public bool moving;

    public Planet planet;

    void Start()
    {
        planet = GameObject.Find("Planet").GetComponent<Planet>();
    }

    //Periodically checks for new matches, very janky but couldn't be bothered making a system to check for recently fallen tiles.
    //This just makes sure subsequent matches from new tiles falling are destroyed.
    void Update()
    {
        frameCount++;
        if (frameCount > 20)
        {
            frameCount = 0;
            if (tile == null)
            {
                tile = transform.GetChild(0).gameObject;
            }
            
            tile.GetComponent<TileSwapper>().CheckAllTiles();
        }
    }

    //Updates the falling variable.
    void FixedUpdate()
    {
        frameCountF++;
        if (frameCountF > 20)
        {
            frameCountF = 0;
            moving = false;
        }
    }


    //Change the specs of the planet, everything is / 3 because originally this was called per row but then 
    //I changed it to reward the amount of tiles destroyed and couldn't be bothered dividing all the values by 3 manually ;).
    public void UpdatePlanetSpecs(int tileType, int numTiles)
    {
        for (int i = 0; i < numTiles; i++)
        {
            if (tileType == 0)
            {
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength += 0.01f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength += 0.2f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.roughness += 0.2f / 3f;
            }
            else if (tileType == 1)
            {
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength -= 0.01f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength -= 0.2f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.roughness -= 0.2f / 3f;
            }
            else if (tileType == 2)
            {
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength -= 0.01f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength += 0.4f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness -= 0.4f / 3f;
            }
            else if (tileType == 3)
            {
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness += 0.2f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.minValue -= 0.02f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength += 0.4f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness += 0.4f / 3f;

            }
            else if (tileType == 4)
            {
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness -= 0.4f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.minValue += 0.01f / 3f;
            }
        }
    }
}
