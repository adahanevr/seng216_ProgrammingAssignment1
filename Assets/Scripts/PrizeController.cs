using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PrizeController : MonoBehaviour
{
    public PlayerScript playerScript;
    public GameObject prizePrefab;
    
    // float variables to set random positions for prizes
    [SerializeField] private List<Sprite> prizeSprites; // list to store number sprites (assigned in Inspector)
    [SerializeField] private float xLimit; // xLimit = 8 (defined in Inspector panel)
    [SerializeField] private float yLimit; // yLimit = 4 (defined in Inspector panel)
    [SerializeField] private float exclusiveRange; // exclusiveRange = 3 (defined in Inspector)

    private List<Prize> _prizes; // list to store prize numbers
    private int _collectedPrizesCount;
    
    // method to generate prizes at random positions
    public void GeneratePrizes(int levelIndex)
    {
        if (prizeSprites == null || prizeSprites.Count == 0) return;

        _collectedPrizesCount = 0;
        _prizes = new List<Prize>();
        var numberOfPrizes = prizeSprites.Count;
        
        if (levelIndex > numberOfPrizes)
            levelIndex = numberOfPrizes;
        
        for (int i = 0; i < levelIndex; i++)
        {
            float x = Random.Range(-xLimit, xLimit);
            float y = Random.Range(-yLimit, yLimit);

            // generate until the initial position of the prize is outside of the exclusive range
            while (Math.Abs(x) <= exclusiveRange)
            {
                x = Random.Range(-xLimit, xLimit);
            }
            while (Math.Abs(y) <= exclusiveRange)
            {
                y = Random.Range(-yLimit, yLimit);
            }

            var pos = new Vector3(x, y, 0); // initial position of prizes
            var prizeObj = Instantiate(prizePrefab, pos, Quaternion.identity);
            
            Prize prize = prizeObj.GetComponent<Prize>();
            prize.SetPrize(prizeSprites[i]);
            prize.ToggleCollider(false);
            _prizes.Add(prize);
            
            if (i == 0) // if it's the first prize (0), enable collider
            {
                prize.ToggleCollider(true);
            }
        }
    }

    public void OnPrizeCollected(Prize prize)
    {
        // when player collects a prize, disable the collider and deactivate the prize
        prize.ToggleCollider(false);
        prize.gameObject.SetActive(false);
        _collectedPrizesCount++;

        if (_collectedPrizesCount > _prizes.Count) 
            return;

        // if the player collects all prizes in the current level, pass to the next level
        if (_collectedPrizesCount == _prizes.Count)
        {
            playerScript.lastLevelNumberFinished++;
            
            if (playerScript.lastLevelNumberFinished < 5) 
                playerScript.StartLevel(playerScript.lastLevelNumberFinished + 1);
            else
            {
                playerScript.ShowGameOver(true);
                return;
            }
        }
        
        // when a prize is collected, enable the collider of the next prize
        if (_prizes[_collectedPrizesCount] != null)
        {
            _prizes[_collectedPrizesCount].ToggleCollider(true);
        }
        
    }
}
