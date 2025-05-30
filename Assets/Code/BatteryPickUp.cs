using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPickup : MonoBehaviour
{
    public string pickupMessage = "Press E to pick up the battery"; // Message to display
    private bool isPlayerInRange = false; // Check if the player is in range
    private PlayerInventory playerInventory; // Reference to the player's inventory

    [Header("UI Settings")]
    public Text interactionText; // Text displayed on the screen

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>(); // Get the player's inventory
            if (interactionText != null)
            {
                interactionText.text = pickupMessage;
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerInventory = null; // Clear reference to the player's inventory
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Check if the player is in range and presses the E key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null)
            {
                playerInventory.AddBattery(); // Add battery to inventory
                Destroy(gameObject); // Destroy the battery pickup
            }
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}