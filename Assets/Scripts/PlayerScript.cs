using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    public PrizeController PrizeController;
    public GhostController GhostController;
    public GameObject gameOverObject;
    public Text levelText;
    public int lastLevelNumberFinished = 0;
    public float speed; // speed of the player (value set to 4 in Inspector)

    void Start()
    {
        StartLevel(1);
    }

    void Update()
    {
        MovePlayer();
    }

    // method for player movement
    void MovePlayer() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector2(h, v) * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Prize"))
        {
            var prize = collider2D.GetComponent<Prize>();
            PrizeController.OnPrizeCollected(prize);
        }
    }

    // method to end the game if player collides with a ghost
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            ShowGameOver(true);
        }
    }
    
    // method to initialize the game
    public void StartLevel(int levelIndex)
    {
        transform.position = Vector3.zero;
        ShowGameOver(false);
        
        GhostController.SetPlayer(this.gameObject);
        GhostController.GenerateGhosts(levelIndex);

        PrizeController.GeneratePrizes(levelIndex);
    }

    // method to display "Game Over" text together with the last level the player has finished
    public void ShowGameOver(bool gameOver)
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            levelText.text = lastLevelNumberFinished.ToString();
        }
        else
            Time.timeScale = 1;
        
        gameOverObject.SetActive(gameOver);
        levelText.gameObject.SetActive(gameOver);
    }
}
