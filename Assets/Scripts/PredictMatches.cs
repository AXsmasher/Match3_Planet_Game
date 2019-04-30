using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictMatches : MonoBehaviour
{
    public int countDownTimer = 800;

    public Material highlight;

    int layerMask = 1 << 9;

    RaycastHit hit;
    RaycastHit hitInternal;

    List<List<GameObject>> possibleMatches = new List<List<GameObject>>();
    List<GameObject> currentPossibleMatch = new List<GameObject>();

    public TileManager manager;

    //Check for possible matches on start, if none, reshuffle. (To reshuffle I just delete all tiles and let the game automatically fill it back up.)
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        CheckPredict();
        if (possibleMatches.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    //Wait for 800 frames and then show a possible match to help player.
    void Update()
    {
        if (countDownTimer <= 0)
        {
            possibleMatches.Clear();
            CheckPredict();
            countDownTimer = 300;
            StartCoroutine(HelpPlayer());
        }
        else
        {
            countDownTimer--;
        }
    }
    
    //Check every tile for matches.
    public void CheckPredict()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            TilePredict(Vector3.right, transform.GetChild(i).gameObject, transform.GetChild(i).position, transform.GetChild(i).GetComponent<TileSwapper>().tileType);
            TilePredict(Vector3.up, transform.GetChild(i).gameObject, transform.GetChild(i).position, transform.GetChild(i).GetComponent<TileSwapper>().tileType);
            TilePredict(Vector3.left, transform.GetChild(i).gameObject, transform.GetChild(i).position, transform.GetChild(i).GetComponent<TileSwapper>().tileType);
            TilePredict(Vector3.down, transform.GetChild(i).gameObject, transform.GetChild(i).position, transform.GetChild(i).GetComponent<TileSwapper>().tileType);
        }
    }

    //Find possible matches for a tile and add to 2d list.
    //Kind of janky but basically just raycasts for the right patterns and if it finds a pattern it will add to list.
    public void TilePredict(Vector3 dir, GameObject currentTile, Vector3 pos, int tileType)
    {
        currentPossibleMatch.Clear();
        if (Physics.Raycast(pos, dir, out hit, 1f, layerMask))
        {
            if (hit.collider.GetComponent<TileSwapper>().tileType == tileType)
            {
                currentPossibleMatch.Add(currentTile);
                currentPossibleMatch.Add(hit.collider.gameObject);

                if (Physics.Raycast(currentPossibleMatch[1].transform.position, dir, out hit, 1f, layerMask))
                {
                    if (Physics.Raycast(hit.collider.transform.position, dir, out hitInternal, 1f, layerMask))
                    {
                        if (hitInternal.collider.gameObject.GetComponent<TileSwapper>().tileType == tileType)
                        {
                            currentPossibleMatch.Add(hitInternal.collider.gameObject);
                            possibleMatches.Add(new List<GameObject>());
                            for (int i = 0; i < currentPossibleMatch.Count; i++)
                            {
                                possibleMatches[possibleMatches.Count - 1].Add(currentPossibleMatch[i]);
                            }
                            return;
                        }
                    }
                    if (Physics.Raycast(hit.collider.transform.position, Quaternion.Euler(new Vector3(0, 0, -90)) * dir, out hitInternal, 1f, layerMask))
                    {
                        if (hitInternal.collider.gameObject.GetComponent<TileSwapper>().tileType == tileType)
                        {
                            currentPossibleMatch.Add(hitInternal.collider.gameObject);
                            possibleMatches.Add(new List<GameObject>());
                            for (int i = 0; i < currentPossibleMatch.Count; i++)
                            {
                                possibleMatches[possibleMatches.Count - 1].Add(currentPossibleMatch[i]);
                            }
                            return;
                        }
                    }
                    if (Physics.Raycast(hit.collider.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)) * dir, out hitInternal, 1f, layerMask))
                    {
                        if (hitInternal.collider.gameObject.GetComponent<TileSwapper>().tileType == tileType)
                        {
                            currentPossibleMatch.Add(hitInternal.collider.gameObject);
                            possibleMatches.Add(new List<GameObject>());
                            for (int i = 0; i < currentPossibleMatch.Count; i++)
                            {
                                possibleMatches[possibleMatches.Count - 1].Add(currentPossibleMatch[i]);
                            }
                            return;
                        }
                    }
                }
            }
            else if(Physics.Raycast(hit.collider.transform.position, dir, out hit, 1f, layerMask))
            {
                if (hit.collider.GetComponent<TileSwapper>().tileType == tileType)
                {
                    currentPossibleMatch.Add(currentTile);
                    currentPossibleMatch.Add(hit.collider.gameObject);
                    if (Physics.Raycast(hit.collider.transform.position, -dir, out hit, 1f, layerMask))
                    {
                        if (Physics.Raycast(hit.collider.transform.position, Quaternion.Euler(0, 0, -90) * dir, out hitInternal, 1f, layerMask))
                        {
                            if (hitInternal.collider.gameObject.GetComponent<TileSwapper>().tileType == tileType)
                            {
                                currentPossibleMatch.Add(hitInternal.collider.gameObject);
                                possibleMatches.Add(new List<GameObject>());
                                for (int i = 0; i < currentPossibleMatch.Count; i++)
                                {
                                    possibleMatches[possibleMatches.Count - 1].Add(currentPossibleMatch[i]);
                                }
                                return;
                            }
                        }
                        if (Physics.Raycast(hit.collider.transform.position, Quaternion.Euler(0, 0, 90) * dir, out hitInternal, 1f, layerMask))
                        {
                            if (hitInternal.collider.gameObject.GetComponent<TileSwapper>().tileType == tileType)
                            {
                                currentPossibleMatch.Add(hitInternal.collider.gameObject);
                                possibleMatches.Add(new List<GameObject>());
                                for (int i = 0; i < currentPossibleMatch.Count; i++)
                                {
                                    possibleMatches[possibleMatches.Count - 1].Add(currentPossibleMatch[i]);
                                }
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    //Highlight a match to help player.
    IEnumerator HelpPlayer()
    {
        if (possibleMatches.Count == 0)
        {
            manager.EndGame();
        }
        int chosenMatch = Random.Range(0, possibleMatches.Count);
        for (int i = 0; i < possibleMatches[chosenMatch].Count; i++)
        {
            possibleMatches[chosenMatch][i].GetComponent<TileSwapper>().Highlight();
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < possibleMatches[chosenMatch].Count; i++)
        {
            possibleMatches[chosenMatch][i].GetComponent<TileSwapper>().DeHighlight();
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < possibleMatches[chosenMatch].Count; i++)
        {
            possibleMatches[chosenMatch][i].GetComponent<TileSwapper>().Highlight();
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < possibleMatches[chosenMatch].Count; i++)
        {
            possibleMatches[chosenMatch][i].GetComponent<TileSwapper>().DeHighlight();
        }
    }
}
