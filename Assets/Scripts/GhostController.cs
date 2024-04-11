using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostController : MonoBehaviour
{
    public GameObject ghostPrefab;
    public GameObject ghostsParent;
    
    // float variables to set random positions for ghosts
    [SerializeField] private float xLimit; // xLimit = 8 (defined in Inspector panel)
    [SerializeField] private float yLimit; // yLimit = 4 (defined in Inspector panel)
    [SerializeField] private float exclusiveRange; // exclusiveRange = 3 (defined in Inspector)
    
    [HideInInspector] public GameObject player;
    
    public List<Ghost> ghosts = new List<Ghost>();

    // method to generate ghosts at random positions
    public void GenerateGhosts(int levelIndex)
    {
        ClearGhosts(); // clear the ghosts remaining from the last level before generating new ones for the current level

        for (int i = 0; i < levelIndex; i++)
        {
            float x = Random.Range(-xLimit, xLimit);
            float y = Random.Range(-yLimit, yLimit);

            // generate until the initial position of the ghost is outside of the exclusive range
            while (Math.Abs(x) <= exclusiveRange)
            {
                x = Random.Range(-xLimit, xLimit);
            }
            while (Math.Abs(y) <= exclusiveRange)
            {
                y = Random.Range(-yLimit, yLimit);
            }

            var pos = new Vector3(x, y, 0); // initial position of ghosts
            var ghostObj = Instantiate(ghostPrefab, pos, Quaternion.identity);
            ghostObj.transform.SetParent(ghostsParent.transform);
            Ghost ghost = ghostObj.GetComponent<Ghost>();
            ghost.SetRandomColor();
            ghost.player = player; // assign player for ghosts (this will be useful for FollowPlayer() method in Ghost.cs)
            ghosts.Add(ghost);
            
            if (i == 0) // if it's the first level (also means the first ghost), make the ghost follow the player 
            {
                ghost.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                ghost.FollowPlayer();
            }
            else // else (for further levels), make the ghosts move randomly
            {
                ghost.isMovingRandomly = true;
                ghost.MoveRandomly();
            }

            ghost.isMovingEnabled = true;
        }
    }
    
    private void ClearGhosts()
    {
        if (ghosts == null || ghosts.Count == 0)
            return;
        
        for (int i = 0; i < ghostsParent.transform.childCount; i++) // destroy ghosts one by one
        {
            var ghost = ghostsParent.transform.GetChild(i);
            Destroy(ghost.gameObject);
        }
        
        ghosts.Clear(); // clear the list
    }

    public void SetPlayer(GameObject playerObj)
    {
        player = playerObj;
    }
}
