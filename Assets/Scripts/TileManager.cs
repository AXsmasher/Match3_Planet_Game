﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public GameObject currentlySelected;

    public GameObject tile;

    int frameCountF;
    int frameCount;

    public bool moving;

    public Planet planet;

    public TMPro.TMP_Text[] stats;

    public int[] goals;
    public int[] currentStats;

    void Start()
    {
        planet = GameObject.Find("Planet").GetComponent<Planet>();

        goals = GlobalSceneVariables.level.goals;

        for (int i = 0; i < goals.Length; i++)
        {
            stats[i].text = currentStats[i].ToString() + " : " + goals[i];
        }
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
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength += 0.005f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength += 0.2f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.roughness += 0.2f / 3f;

                UpdateScore(0, 1);
            }
            else if (tileType == 1)
            {
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength -= 0.0025f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength -= 0.2f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.roughness -= 0.2f / 3f;

                UpdateScore(1, 2);
            }
            else if (tileType == 2)
            {
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength -= 0.01f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength += 0.4f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness -= 0.4f / 3f;

                UpdateScore(2, 0);
            }
            else if (tileType == 3)
            {
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness += 0.2f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.minValue -= 0.02f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.strength += 0.4f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness += 0.4f / 3f;

                UpdateScore(3, 4);

            }
            else if (tileType == 4)
            {
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness -= 0.4f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.minValue += 0.01f / 3f;

                UpdateScore(4, 3);
            }
        }
    }

    //Update score
    public void UpdateScore(int increase, int decrease)
    {
        currentStats[increase] = Mathf.Clamp(currentStats[increase] + 1, 0, 100);
        currentStats[decrease] = Mathf.Clamp(currentStats[decrease] - 1, 0, 100);
        stats[increase].text = currentStats[increase].ToString() + " : " + goals[increase];
        stats[decrease].text = currentStats[decrease].ToString() + " : " + goals[decrease];

        //Check if player has beaten the level

        bool won = true;

        for (int i = 0; i < currentStats.Length; i++)
        {
            if (currentStats[i] <= goals[i])
            {
                won = false;
            }
        }

        if (won)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        //Do Shit.
    }
}
