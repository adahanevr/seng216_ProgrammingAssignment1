using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider2D;
    
    // method to associate prizes with the corresponding sprites
    public void SetPrize(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
    
    // method to enable or disable the colliders of prizes
    public void ToggleCollider(bool flag)
    {
        collider2D.enabled = flag;
    }
}
