using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class RangedWeapon : MonoBehaviour
{
    public Transform firePointRight;
    public Transform firePointLeft;
    public GameObject bulletPrefab;
    public GameObject player;
    private SpriteRenderer spriteRenderer;

    public float bulletForce = 20f;
    public float ammoSize = 20f;
    public float currentAmmo;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public float reloadTime = 0f;
    private bool reloading = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAmmo = ammoSize;
    }

    private void Update()
    {
        if (reloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // Actual movement
    void FixedUpdate()
    {
        // Weapon Direction
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.GetComponent<PlayerMovement>().lookDir);

        if (player.GetComponent<PlayerMovement>().lookDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnEnable()
    {
        reloading = false;
    }

    public void Shoot()
    {
        // Add delay and potentially reload here
        currentAmmo--;

        if (player.GetComponent<PlayerMovement>().lookDir.x < 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePointLeft.position, firePointLeft.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePointLeft.up * bulletForce, ForceMode2D.Impulse);
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, firePointRight.position, firePointRight.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePointRight.up * bulletForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator Reload()
    {
        reloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = ammoSize;
        reloading = false;
    }
}
