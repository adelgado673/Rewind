using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSprites : MonoBehaviour
{
    public Sprite[] wallSprites;
    SpriteRenderer spriteRenderer;
    public int currentEra = 0;

    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    public void CurrentSprite(int era)
    {
        currentEra = era;
        spriteRenderer.sprite = wallSprites[era];
    }
}
