using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    public int selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        SelectItem();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedItem = selectedItem;

        selectedItem = GameObject.Find("RewindManager").GetComponent<RewindManager>().currentEra;

        if (previousSelectedItem != selectedItem)
            SelectItem();
    }

    void SelectItem()
    {
        int i = 0;
        foreach (Transform item in transform)
        {
            if (i == selectedItem)
                item.gameObject.SetActive(true);
            else
                item.gameObject.SetActive(false);
            i++;

        }
    }
}
