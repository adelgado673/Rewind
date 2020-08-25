using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Camera cam;
    public Animator animator;

    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    Vector2 movement;
    Vector2 mousePos;
    public Vector2 lookDir;
    public int currentEra;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    // For input
    void Update()
    {
        currentEra = GameObject.Find("RewindManager").GetComponent<RewindManager>().currentEra;

        // movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // aim input
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Sprite Switch
        spriteRenderer.sprite = sprites[currentEra];
        animator.SetInteger("CurrentEra", currentEra);

        if (movement != Vector2.zero)
            animator.SetInteger("Speed", 1);
        else
            animator.SetInteger("Speed", -1);
    }

    // Actual movement
    void FixedUpdate()
    {
        // Character Position
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        // Weapon Direction
        lookDir = mousePos - rb.position;

        if (lookDir.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}
