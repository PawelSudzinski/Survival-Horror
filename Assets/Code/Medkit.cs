using UnityEngine;
using UnityEngine.UI;

public class Medkit : MonoBehaviour
{
    public string pickupMessage = "Press E to pick up the medkit";
    private bool isPlayerInRange = false;
    private PlayerStats playerStats;

    [Header("UI Settings")]
    public Text interactionText; // UI Text for interaction message

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerStats = other.GetComponent<PlayerStats>();

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
            isPlayerInRange = false;
            playerStats = null;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerStats != null)
            {
                playerStats.AddMedkit();
                Destroy(gameObject);
            }

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
