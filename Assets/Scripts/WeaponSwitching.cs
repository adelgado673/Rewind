using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    // 0 and 1 - Ranged and Melee
    // Maybe swap
    public int selectedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] melee = GameObject.FindGameObjectsWithTag("MeleeWeapon");

        // Melee weapon active
        if (melee.Length != 0)
        {
            if (!melee[0].GetComponent<MeleeWeapon>().inAnimation)
            {
                Scroll();
            }
        }
        else
        {
            Scroll();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;

        }
    }

    void Scroll()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
}
