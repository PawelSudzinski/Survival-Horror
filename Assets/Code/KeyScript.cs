using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    [Header("Key Settings")]
    public string keyName; // Name of the key
    private bool isPlayerNear = false; // Tracks if the player is near the key

    [Header("UI Settings")]
    public Text interactionText; // Text displayed on the screen
    public string pickupMessage = "Press E to pick up the key"; // Message displayed when near the key

    private void Start()
    {
        // Hide interaction text at the start
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for player input only if the player is near the key
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            PickupKey();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            // Display interaction text
            if (interactionText != null)
            {
                interactionText.text = pickupMessage;
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            // Hide interaction text
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void PickupKey()
    {
        // Add the key to the player's inventory
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddKey(keyName);
                Debug.Log($"Player picked up the key: {keyName}");
            }
        }

        // Hide the interaction text and destroy the key object
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}