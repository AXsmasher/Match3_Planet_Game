using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSwapper : MonoBehaviour
{
    public Material highlight;
    public Material darkHighlight;
    public Material normal;

    public Rigidbody r;

    public int tileType;

    private Renderer rend;

    public TileManager manager;
    public PlaceTiles placeTiles;
    public PredictMatches predictMatches;

    public bool isSelected;
    public bool isHighlighted;

    private RaycastHit hit;

    private int layerMask = 1 << 9;

    private GameObject pastTile;

    public bool switched;

    //Gets refrences on start.
    IEnumerator Start()
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();
        yield return new WaitForEndOfFrame();
        if (transform.parent)
        {
            manager = transform.parent.GetComponent<TileManager>();
            placeTiles = transform.parent.GetComponent<PlaceTiles>();
            predictMatches = transform.parent.GetComponent<PredictMatches>();
        }
    }

    //Check if any tiles are moving. If so player cant do anything.
    void FixedUpdate()
    {
        if (r.velocity.magnitude > 0.01)
        {
            if (manager)
            {
                manager.moving = true;
            }
        }
    }

    //Next three functions just fire appropriate functions.

    void OnMouseOver() //Take this out for mobile builds.
    {
        if (!GlobalSceneVariables.paused)
        {
            Highlight();
        }
    }

    void OnMouseExit()
    {
        DeHighlight();
    }

    void OnMouseDown()
    {
        if (!GlobalSceneVariables.paused)
        {
            if (manager.currentlySelected)
            {
                manager.currentlySelected.GetComponent<TileSwapper>().StartCoroutine("Deselect");
            }
            Select();
        }
    }

    //Guess what, it dehighlights a tile.
    public void DeHighlight()
    {
        if (isHighlighted && !isSelected)
        {
            rend.material = normal;
            isHighlighted = false;
        }
    }

    //Deselect a tile, kind of self explanatory amirite?
    IEnumerator Deselect()
    {
        rend.material = normal;
        yield return new WaitForEndOfFrame();
        isSelected = false;
        yield return new WaitForEndOfFrame();
        rend.material = normal;
    }

    //Highlights a tile duh.
    public void Highlight()
    {
        if (normal == null)
        {
            normal = rend.material;
        }
        if (!isHighlighted)
        {
            rend.material = highlight;
            isHighlighted = true;
        }
    }

    //Selects a tile obviously. Make sure player cant select if blocks are falling
    public void Select()
    {
        if (!manager.moving)
        {
            manager.currentlySelected = gameObject;

            if (normal == null)
            {
                normal = rend.material;
            }

            isSelected = true;

            switched = false;

            CheckSwitch(Vector3.right);
            CheckSwitch(Vector3.left);
            CheckSwitch(Vector3.up);
            CheckSwitch(Vector3.down);

            rend.material = darkHighlight;
        }
    }

    //Basically just checks if two tiles are switchable then if they are it switches and checks for matches.
    public void CheckSwitch(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, dir, out hit, 0.7f, layerMask))
        {
            if (hit.collider.GetComponent<TileSwapper>().isSelected && !switched)
            {
                pastTile = hit.collider.gameObject;
                Switch();
                switched = true;
                StartCoroutine(CheckMatch(dir));
            }
        }
    }

    //Checks for matches and then if no matches it just switches the tiles back and deselects.
    IEnumerator CheckMatch(Vector3 dir)
    {
        yield return new WaitForSeconds(0.1f);
        CheckAllTiles();
        manager.remainingMoves--;

        yield return new WaitForSeconds(1f);

        if (pastTile != null)
        {
            Switch();
            manager.remainingMoves++;
        }
        else if (manager.remainingMoves % 5 == 0)
        {
            StartCoroutine(manager.LateUpdatePlanet());
        }
        manager.currentlySelected.transform.GetChild(0).GetComponent<Renderer>().material = manager.currentlySelected.GetComponent<TileSwapper>().normal;
        manager.currentlySelected.GetComponent<TileSwapper>().DeHighlight();
        DeHighlight();
        Deselect();
    }

    //Check whole board for matches.
    public void CheckAllTiles()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            CheckDestroy(Vector3.right, transform.parent.GetChild(i).gameObject);
            CheckDestroy(Vector3.up, transform.parent.GetChild(i).gameObject);
        }
    }

    //Check if a tile and it's associated row can be destroyed.
    //Basically just add tiles to a list, if list is 3 or bigger, destroy tiles.
    public void CheckDestroy(Vector3 dirMatched, GameObject newTile)
    {
        List<GameObject> tiles = new List<GameObject>();
        tiles.Add(newTile);

        while (Physics.Raycast(newTile.transform.position, dirMatched, out hit, 0.7f, layerMask))
        {
            if (hit.collider.gameObject.GetInstanceID() == newTile.GetInstanceID())
            {
                break;
            }
            else if (hit.collider.GetComponent<TileSwapper>().tileType == newTile.GetComponent<TileSwapper>().tileType)
            {
                newTile = hit.collider.gameObject;
                tiles.Add(newTile);
            }
            else
            {
                break;
            }
        }

        if (tiles.Count > 2 && !manager.moving)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                Destroy(tiles[i]);
            }
            predictMatches.countDownTimer = 800;

            //Make planet look fancy and update amounts.
            manager.UpdatePlanetSpecs(tiles[0].GetComponent<TileSwapper>().tileType, tiles.Count);
        }
    }

    //Switch two tiles.
    public void Switch()
    {
        Vector3 oldPos = transform.position;
        transform.position = pastTile.transform.position;
        pastTile.transform.position = oldPos;

        StartCoroutine("Deselect");
    }
}
