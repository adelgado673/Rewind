using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Some Notes
// - Nuclear throne uses completely different values for different floors
// - Somehow clean this up in the future to read numbers from a excel file or something
// - Fields would include different sizes of possible floor spawns (1x1, 2x2, 3x3, maybe bigger)
// - Different potential turn chances and directions
// - Different rate of walker spawn and deletion

public class NewLevelGeneration : MonoBehaviour
{
    // Values to tweak
    public float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
    public float chanceWalkerDestroy = 0.05f;
    public int maxWalkers = 10;
    public float percentToFill = 0.2f;
    public GameObject wallObj, floorObj, playerSpawn;
    public float size = 30f;
    public bool removeSingleWalls;
    public float worldUnitsInOneGridCell = 1;

    enum gridSpace {empty, floor, wall};
    gridSpace[,] grid;
    int roomHeight, roomWidth;
    Vector2 roomSizeWorldUnits;
    private bool playerSpawned;
    
    struct Walker
    {
        public Vector2 dir;
        // Make this a list
        public Vector2 pos;
        public int size;
    }
    List<Walker> walkers;

    // Start is called before the first frame update
    void Start()
    {
        roomSizeWorldUnits = new Vector2(size, size);

        Setup();
        CreateFloors();
        CreateWalls();
        if (removeSingleWalls)
            RemoveSingleWalls();
        SpawnLevel();
    }

    void Setup()
    {
        // Find grid size
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);

        // Create grid
        grid = new gridSpace[roomWidth, roomHeight];

        // Set grid's default state
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                // Make every cell 'empty'
                grid[x, y] = gridSpace.empty;
            }
        }

        // Set first walker
        // init list
        walkers = new List<Walker>();

        // Create a walker
        Walker newWalker = new Walker();
        newWalker.dir = RandomDirection();

        // Find center of grid
        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2.0f), Mathf.RoundToInt(roomHeight / 2.0f));
        newWalker.pos = spawnPos;

        // Start first walker with size 1
        newWalker.size = 1;

        // Add walker to the list
        walkers.Add(newWalker);
    }

    void CreateFloors()
    {
        int iterations = 0; // Loop will not run forever
        do
        {
            // Create floor at postion of every walker
            for (int i = 0; i < walkers.Count; i++)
            {
                Walker thisWalker = walkers[i];

                // 50/50 2x2 or 1x1 for desert
                if (Random.value < 0.5f)
                {

                    // Change to for loop of positions in walker
                    // Need to make sure these are in bounds
                    // 2x2
                    if (thisWalker.pos.x + 1 < roomWidth - 2 && thisWalker.pos.y + 1 < roomHeight - 2)
                    {
                        thisWalker.size = 2;
                        grid[(int)thisWalker.pos.x, (int)thisWalker.pos.y] = gridSpace.floor;
                        grid[(int)thisWalker.pos.x + 1, (int)thisWalker.pos.y] = gridSpace.floor;
                        grid[(int)thisWalker.pos.x, (int)thisWalker.pos.y + 1] = gridSpace.floor;
                        grid[(int)thisWalker.pos.x + 1, (int)thisWalker.pos.y + 1] = gridSpace.floor;
                    }
                    else
                    {
                        // Please clean this up
                        // This will get very nasty very fast
                        // 1x1
                        thisWalker.size = 1;
                        grid[(int)thisWalker.pos.x, (int)thisWalker.pos.y] = gridSpace.floor;
                    }
                }
                else
                {
                    // 1x1
                    thisWalker.size = 1;
                    grid[(int)thisWalker.pos.x, (int)thisWalker.pos.y] = gridSpace.floor;
                }

                walkers[i] = thisWalker;
            }

            // Chance: destroy walker
            int numberChecks = walkers.Count; //might modify count while in this loop
            for (int i = 0; i < numberChecks; i++)
            {
                // Only if its not the only one, and at a low chance
                if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                    walkers.RemoveAt(i);
                break; // Only destroy one per iteration
            }

            // Chance: walker pick new direction
            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
                {
                    Walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }

            // Chance: spawn new walker
            numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                // Only if # of walkers < max and at a low chance
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    // Create a walker
                    Walker newWalker = new Walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            }

            // Move walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                Walker thisWalker = walkers[i];
                thisWalker.pos += thisWalker.dir;
                walkers[i] = thisWalker;
            }

            // Avoid border of grid
            for (int i = 0; i < walkers.Count; i++)
            {
                Walker thisWalker = walkers[i];
                // Clamp x, y to leave a 1 space border: leave room for walls
                if (thisWalker.size == 1)
                {
                    thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                    thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
                }
                else if (thisWalker.size == 2)
                {
                    thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                    thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
                }
                walkers[i] = thisWalker;
            }

            // Check to exit loop
            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }
            iterations++;
        } while (iterations < 10000);
    }

    void CreateWalls()
    {
        // Lopp through every grid space
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                // If there's a floor, check the spaces around it
                if (grid[x, y] == gridSpace.floor)
                {
                    if (grid[x, y + 1] == gridSpace.empty)
                    {
                        grid[x, y + 1] = gridSpace.wall;
                    }
                    if (grid[x, y - 1] == gridSpace.empty)
                    {
                        grid[x, y - 1] = gridSpace.wall;
                    }
                    if (grid[x + 1, y] == gridSpace.empty)
                    {
                        grid[x + 1, y] = gridSpace.wall;
                    }
                    if (grid[x - 1, y] == gridSpace.empty)
                    {
                        grid[x - 1, y] = gridSpace.wall;
                    }
                }
            }
        }
    }

    void RemoveSingleWalls()
    {
        // Loop through every grid space
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                // If there's a wall, check the spaces around it
                if (grid[x, y] == gridSpace.wall)
                {
                    // Assume all space around wall are floors
                    bool allFloors = true;

                    // Check each side to see if they are all floors
                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > roomWidth - 1 || y + checkY < 0 || y + checkY > roomHeight - 1)
                            {
                                // Skip checks that are out of range
                                continue;
                            }
                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                            {
                                // Skip corners and center
                                continue;
                            }
                            if (grid[x + checkX, y + checkY] != gridSpace.floor)
                            {
                                allFloors = false;
                            }
                        }
                    }
                    if (allFloors)
                        grid[x, y] = gridSpace.floor;
                }
            }
        }
    }

    void SpawnLevel()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                switch (grid[x, y])
                {
                    case gridSpace.empty:
                        break;
                    case gridSpace.floor:
                        Spawn(x, y, floorObj);
                        break;
                    case gridSpace.wall:
                        Spawn(x, y, wallObj);
                        break;
                }
            }
        }
    }

    void Spawn(float x, float y, GameObject toSpawn)
    {
        // Find the position to spawn
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;

        // Spawn object
        var newObj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
        newObj.transform.parent = gameObject.transform;
    }

    int NumberOfFloors()
    {
        int count = 0;
        foreach (gridSpace space in grid)
        {
            if (space == gridSpace.floor)
                count++;
        }
        return count;
    }

    void Room(float x)
    {

    }

    Vector2 RandomDirection()
    {
        // Pick random int between 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);

        // Use that int to choose a direction
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }
}
