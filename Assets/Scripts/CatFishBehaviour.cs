using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFishBehaviour : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject

    void Start()
    {
        // Find the GameObject with the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Check if player is found
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    void Update()
    {

        // Ensure player reference is not null
        if (player != null)
        {// Calculate the direction from this object to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // Ignore changes in pitch or roll by setting the direction's y component to zero
            directionToPlayer.y = 0f;

            // Rotate this object to face the player only on the y-axis
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }
    }
}
