using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public GameObject player;
    public Animator animator;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public float damage = 20f;
    public bool inAnimation;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("attack");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void FixedUpdate()
    {
        // Weapon Direction
        if (!inAnimation)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, player.GetComponent<PlayerMovement>().lookDir);
    }
}
