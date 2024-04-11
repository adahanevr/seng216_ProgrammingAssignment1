using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class Ghost : MonoBehaviour
{
    public float followSpeed; // speed of the ghost following the player (value set to 3 in Inspector)
    public float randomSpeed; // speed of the ghosts moving randomly (value set to 2 in Inspector)
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;
    [HideInInspector] public bool isMovingRandomly = false;
    [HideInInspector] public bool isMovingEnabled = false;
    [HideInInspector] public GameObject player;
   
    void Update()
    {
        if (!isMovingEnabled) return;

        if (!isMovingRandomly)
            FollowPlayer();
    }
    
    public void MoveRandomly()
    {
        Vector3 randomDirection = Random.onUnitSphere; // set random direction
        randomDirection.z = 0;
        _rb.AddForce(randomDirection * 100f * randomSpeed, ForceMode2D.Force);
    }
    
    public void FollowPlayer()
    {
        var position = transform.position;
        Vector3 direction = player.transform.position - position; // set direction according to player's position
        direction.Normalize();
        position += direction * (followSpeed * Time.deltaTime);
        transform.position = position;
    }

    public void SetRandomColor()
    {
        _spriteRenderer.color = GetRandomColor();
    }
    
    private Color GetRandomColor() {
        Color[] colors = {Color.red, Color.blue, Color.yellow};
        return colors[Random.Range(0, colors.Length)];
    }
}
