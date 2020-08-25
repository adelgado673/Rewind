using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpriteSpawner : MonoBehaviour
{
    public GameObject bottomWall;
    public GameObject topWall;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bottomLeftWall;
    public GameObject bottomRightWall;

    public int moveAmount;
    private bool tilesPlaced = false;
    public LayerMask whatIsTile;
    public LevelGeneration levelGen;

    // Start is called before the first frame update
    void Start()
    {
        levelGen = GameObject.Find("Level Generation").GetComponent<LevelGeneration>();
    }

    private void LateUpdate()
    {
        // This needs to start after all the tiles in the room have been placed
        if (levelGen.stopGeneration && !tilesPlaced)
        {
            tilesPlaced = true;
            PlaceTiles();
        }
    }

    public void PlaceTiles()
    {
        foreach (Transform spawnPoint in transform)
        {
            if (spawnPoint.tag != "Player Spawn" && spawnPoint.GetChild(0).tag == "Wall")
            {
                GameObject instance;
                // Bottom Tiles
                Collider2D tileDetection = Physics2D.OverlapPoint(new Vector2(spawnPoint.position.x, spawnPoint.position.y - moveAmount), whatIsTile);
                if (tileDetection == null)
                {
                    instance = Instantiate(bottomWall, new Vector2(spawnPoint.position.x, spawnPoint.position.y - moveAmount), Quaternion.identity);
                    instance.transform.parent = spawnPoint.transform;
                }

                // Top Tiles
                tileDetection = Physics2D.OverlapPoint(new Vector2(spawnPoint.position.x, spawnPoint.position.y + moveAmount), whatIsTile);
                if (tileDetection == null)
                {
                    instance = Instantiate(topWall, new Vector2(spawnPoint.position.x, spawnPoint.position.y + moveAmount), Quaternion.identity);
                    instance.transform.parent = spawnPoint.transform;
                }

                // Left Tiles
                tileDetection = Physics2D.OverlapPoint(new Vector2(spawnPoint.position.x - moveAmount, spawnPoint.position.y), whatIsTile);
                if (tileDetection == null)
                {
                    instance = Instantiate(leftWall, new Vector2(spawnPoint.position.x - moveAmount, spawnPoint.position.y), Quaternion.identity);
                    instance.transform.parent = spawnPoint.transform;

                    // Bottom Left Corner Tiles
                    Collider2D cornerDetection = Physics2D.OverlapPoint(new Vector2(spawnPoint.position.x - moveAmount, spawnPoint.position.y - moveAmount), whatIsTile);
                    if (cornerDetection == null)
                    {
                        instance = Instantiate(bottomLeftWall, new Vector2(spawnPoint.position.x - moveAmount, spawnPoint.position.y - moveAmount), Quaternion.identity);
                        instance.transform.parent = spawnPoint.transform;
                    }
                }

                // Right Tiles
                tileDetection = Physics2D.OverlapPoint(new Vector2(spawnPoint.position.x + moveAmount, spawnPoint.position.y), whatIsTile);
                if (tileDetection == null)
                {
                    instance = Instantiate(rightWall, new Vector2(spawnPoint.position.x + moveAmount, spawnPoint.position.y), Quaternion.identity);
                    instance.transform.parent = spawnPoint.transform;

                    // Right corner check
                    Collider2D cornerDetection = Physics2D.OverlapPoint(new Vector2(spawnPoint.position.x + moveAmount, spawnPoint.position.y - moveAmount), whatIsTile);
                    if (cornerDetection == null)
                    {
                        instance = Instantiate(bottomRightWall, new Vector2(spawnPoint.position.x + moveAmount, spawnPoint.position.y - moveAmount), Quaternion.identity);
                        instance.transform.parent = spawnPoint.transform;
                    }
                }
            }
        }
    }
}
