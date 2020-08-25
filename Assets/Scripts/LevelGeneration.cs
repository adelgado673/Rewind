using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;
    // index 0 -> LB, index 1 -> LR, index 2 -> LT, index 3 -> RB, index 4 -> RT, index 5 -> TB,
    // index 6 -> L, index 7 -> R, index 8 -> T

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.0f;

    public float minX;
    public float maxX;
    public float maxY;
    public bool stopGeneration;

    // Start is called before the first frame update
    void Start()
    {
        // Pick starting position
        int randStartinPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartinPos].position;

        // Pick next direction
        direction = Random.Range(1, 6);

        // Pick room based off direction
        int pick = 0;
        if (direction == 1 || direction == 2)
        {
            if (transform.position.x < maxX)
                pick = 7;
            else
            {
                direction = 3;
                pick = 6;
            }
        }
        else if (direction == 3 || direction == 4)
        {
            if (transform.position.x > minX)
                pick = 6;
            else
            {
                direction = 1;
                pick = 7;
            }
        }
        else if (direction == 5)
        {
            pick = 8;
        }

        Instantiate(rooms[pick], transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) // Move Right
        {
            if (transform.position.x < maxX)
            {
                // Move current position
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                // Decide the next direction - can't be left
                direction = Random.Range(1, 6);
                if (direction == 3)
                    direction = 2;
                else if (direction == 4)
                    direction = 5;

                // Pick based off next direction
                int pick = 0;
                if (direction == 1 || direction == 2)
                {
                    // If we can go right, pick LR
                    if (transform.position.x < maxX)
                        pick = 1;
                    else
                    {
                        direction = 5;
                        pick = 2;
                    }
                }
                else if (direction == 5)
                {
                    // Going up next so pick LT
                    pick = 2;
                }

                Instantiate(rooms[pick], transform.position, Quaternion.identity);
            }
            else
            {
                // If you can't go right, go up
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) // Move Left
        {
            if (transform.position.x > minX)
            {
                // Move current position
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                // Decide the next direction - cant't be Right
                direction = Random.Range(3, 6);

                // Pick based off next direction
                int pick = 0;
                if (direction == 3 || direction == 4)
                {
                    // If we can go left, pick LR
                    if (transform.position.x > minX)
                        pick = 1;
                    else
                    {
                        pick = 4;
                        direction = 5;
                    }
                }
                else if (direction == 5)
                {
                    // Going up next so pick RT
                    pick = 4;
                }

                Instantiate(rooms[pick], transform.position, Quaternion.identity);
            }
            else
            {
                // If you can't go right, go up
                direction = 5;
            }

        }
        else if (direction == 5) // Move Up
        {
            if (transform.position.y < maxY)
            {
                // Move current position
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
                transform.position = newPos;

                // Decide next direction - Go wherever
                direction = Random.Range(1, 6);

                // Pick based off next direction
                int pick = 0;
                if (direction == 1 || direction == 2)
                {
                    // If we can go right, pick LR
                    if (transform.position.x < maxX)
                        pick = 3;
                    else
                    {
                        pick = 5;
                        direction = 5;
                    }

                }
                else if (direction == 3 || direction == 4)
                {
                    // If we can go left, pick LR
                    if (transform.position.x > minX)
                        pick = 0;
                    else
                    {
                        pick = 5;
                        direction = 5;
                    }
                }
                else if (direction == 5)
                {
                    // Going up next so pick TB
                    pick = 5;
                }

                Instantiate(rooms[pick], transform.position, Quaternion.identity);
            }
            else
            {
                // If you can't go up, stop
                stopGeneration = true;
            }
        }
    }
}
