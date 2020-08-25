using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject aStar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GridSpawn());
    }

    IEnumerator GridSpawn()
    {
        yield return new WaitForSeconds(1f);

        aStar.SetActive(true);
    }

}
