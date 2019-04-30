using System.Collections;
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
    public TMPro.TMP_Text moves;

    public int[] goals;
    public int[] currentStats;

    public int remainingMoves;

    public GameObject won;
    public GameObject lost;

    public int currentLevel;
    public int levelCompletion;

    void Start()
    {
        planet = GameObject.Find("Planet").GetComponent<Planet>();

        goals = GlobalSceneVariables.level.goals;

        remainingMoves = GlobalSceneVariables.level.totalMoves;

        currentLevel = GlobalSceneVariables.level.levelNumber;

        if (PlayerPrefs.HasKey("levelCompletion"))
        {
            levelCompletion = PlayerPrefs.GetInt("levelCompletion");
        }

        for (int i = 0; i < goals.Length; i++)
        {
            stats[i].text = currentStats[i].ToString() + " : " + goals[i];
        }

        moves.text = "Moves: " + remainingMoves.ToString();
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
    //Update Planet Shape every 5 moves.

    public void UpdatePlanetSpecs(int tileType, int numTiles)
    {
        if (remainingMoves <= 0)
        {
            EndGame();
        }
        moves.text = "Moves: " + remainingMoves.ToString();
        for (int i = 0; i < numTiles; i++)
        {
            if (tileType == 0)
            {
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.persistence += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.baseRoughness -= 0.1f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.roughness += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.roughness += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.baseRoughness += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength += 0.02f / 3f;

                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.x+=100;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.y+=100;

                UpdateScore(0, 1);
            }
            else if (tileType == 1)
            {
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.roughness -= 0.1f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.baseRoughness -= 0.05f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness -= 0.05f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.strength -= 0.02f / 3f;

                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.x += 100;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.y += 100;

                UpdateScore(1, 2);
            }
            else if (tileType == 2)
            {
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness -= 0.05f / 3f;
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.persistence -= 0.1f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.roughness -= 0.1f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.baseRoughness -= 0.05f / 3f;

                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.x += 100;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.y += 100;

                UpdateScore(2, 0);
            }
            else if (tileType == 3)
            {
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness += 0.1f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.minValue -= 0.02f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness += 0.4f / 3f;

                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.x += 100;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.y += 100;

                UpdateScore(3, 4);

            }
            else if (tileType == 4)
            {
                planet.shapeSettings.noiseLayers[1].noiseSettings.ridgidNoiseSettings.roughness -= 0.1f / 3f;
                planet.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.baseRoughness -= 0.4f / 3f;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.minValue += 0.02f / 3f;

                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.x += 100;
                planet.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.centre.y += 100;

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
            if (currentStats[i] < goals[i])
            {
                won = false;
            }
        }

        if (won)
        {
            WinGame();
        }
    }

    //Show win screen.
    public void WinGame()
    {
        won.SetActive(true);
        StartCoroutine(LateUpdatePlanet());
        if (currentLevel > levelCompletion)
        {
            levelCompletion++;
            PlayerPrefs.SetInt("levelCompletion", levelCompletion);
        }
    }
    //Show end screen.
    public void EndGame()
    {
        lost.SetActive(true);
        StartCoroutine(LateUpdatePlanet());
    }

    //For when won or lost so player can see what they did to the planet.
    public IEnumerator LateUpdatePlanet()
    {
        yield return new WaitForSeconds(1f);
        planet.GeneratePlanet();
    }
}
