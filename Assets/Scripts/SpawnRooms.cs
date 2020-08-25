using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRooms : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public GameObject solidRoom;
    public LevelGeneration levelGen;

    // Update is called once per frame
    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (roomDetection == null && levelGen.stopGeneration)
        {
            // Spawn solidRoom
            GameObject instance = (GameObject)Instantiate(solidRoom, transform.position, Quaternion.identity);
            instance.transform.parent = levelGen.transform.GetChild(1).transform;
            Destroy(gameObject);
        }
    }
}
