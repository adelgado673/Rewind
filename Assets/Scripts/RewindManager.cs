using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public int currentEra;
    public GameObject[] wallTiles = null;
    public GameObject[] floorTiles = null;

    private void Start()
    {
        currentEra = 0;
    }

    // Update is called once per frame
    // Try fixed and regular
    // Regular seems better
    void Update()
    {
        GameObject[] melee = GameObject.FindGameObjectsWithTag("MeleeWeapon");

        // Melee weapon active
        if (melee.Length != 0)
        {
            if (!melee[0].GetComponent<MeleeWeapon>().inAnimation)
            {
                if (Input.GetKeyDown("r"))
                {
                    // Finding all tiles
                    // Do this at some other time - At end of loading

                    if (wallTiles.Length == 0)
                    {
                        wallTiles = GameObject.FindGameObjectsWithTag("Wall");
                        floorTiles = GameObject.FindGameObjectsWithTag("Floor");
                    }

                    currentEra++;
                    if (currentEra > 4)
                        currentEra = 0;

                    foreach (GameObject wall in wallTiles)
                    {
                        wall.GetComponent<WallSprites>().CurrentSprite(currentEra);
                    }

                    foreach (GameObject floor in floorTiles)
                    {
                        floor.GetComponent<WallSprites>().CurrentSprite(currentEra);
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown("r"))
            {
                // Finding all tiles
                // Do this at some other time - At end of loading

                if (wallTiles.Length == 0)
                {
                    wallTiles = GameObject.FindGameObjectsWithTag("Wall");
                    floorTiles = GameObject.FindGameObjectsWithTag("Floor");
                }

                currentEra++;
                if (currentEra > 4)
                    currentEra = 0;

                foreach (GameObject wall in wallTiles)
                {
                    wall.GetComponent<WallSprites>().CurrentSprite(currentEra);
                }

                foreach (GameObject floor in floorTiles)
                {
                    floor.GetComponent<WallSprites>().CurrentSprite(currentEra);
                }
            }
        }
    }
}
