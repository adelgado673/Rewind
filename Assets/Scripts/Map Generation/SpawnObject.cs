using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    public bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        if (objects[rand].tag != "Player")
        {
            GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, Quaternion.identity);
            instance.transform.parent = transform;
        }
        else
        {
            GameObject.Find("Player").transform.position = transform.position;
        }
        spawned = true;

    }
}
