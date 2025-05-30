using UnityEngine;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public Animator doorAnimator; // Animator controlling the door's animations
    public string openAnimationName = "DoorOpen"; // Name of the door open animation
    public string closeAnimationName = "DoorClose"; // Name of the door close animation
    private bool isDoorOpen = false; // Tracks if the door is open
    private bool isPlayerNear = false; // Tracks if the player is near the door

    [Header("Key Settings")]
    public bool requireKey = true; // If false, the door can be opened without a key
    public string requiredKeyName; // Name of the key required to open the door
    private PlayerInventory playerInventory; // Reference to the player's inventory

    [Header("UI Settings")]
    public Text interactionText; // Text displayed on the screen
    public string openMessage = "Press E to open"; // Message when the door is closed
    public string closeMessage = "Press E to close"; // Message when the door is open
    public string missingKeyMessage = "You need the correct key to open this door."; // Message when the player is missing the key

    [Header("Jumpscare Settings")]
    public bool enableJumpscare = false; // Enable or disable jumpscare
    public GameObject jumpscareObject; // Reference to the jumpscare object
    public float jumpscareDuration = 1.5f; // How long the jumpscare lasts
    private bool hasTriggeredJumpscare = false; // Ensures jumpscare happens only once

    [Header("Destroy Settings")]
    public bool destroyAfterOpen = false; // If true, destroys the door after opening

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory component not found on the Player.");
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            AttemptToToggleDoor();
        }
    }

    private void AttemptToToggleDoor()
    {
        if (!requireKey || (playerInventory != null && playerInventory.HasKey(requiredKeyName)))
        {
            ToggleDoor();
        }
        else
        {
            if (interactionText != null)
            {
                interactionText.text = missingKeyMessage;
                Invoke(nameof(HideInteractionText), 2f);
            }
        }
    }

    public void ToggleDoor()
    {
        if (isDoorOpen)
        {
            doorAnimator.Play(closeAnimationName);
        }
        else
        {
            doorAnimator.Play(openAnimationName);

            if (enableJumpscare && !hasTriggeredJumpscare)
            {
                TriggerJumpscare();
                hasTriggeredJumpscare = true;
            }

            if (destroyAfterOpen)
            {
                Invoke(nameof(DestroyDoor), 1f); // Destroys the door after animation delay
            }
        }

        isDoorOpen = !isDoorOpen;

        if (interactionText != null)
        {
            interactionText.text = isDoorOpen ? closeMessage : openMessage;
        }
    }

    private void DestroyDoor()
    {
        Destroy(gameObject);
    }

    private void TriggerJumpscare()
    {
        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(true);
            Invoke(nameof(HideJumpscare), jumpscareDuration);
        }
    }

    private void HideJumpscare()
    {
        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (interactionText != null)
            {
                interactionText.text = isDoorOpen ? closeMessage : openMessage;
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    private void HideInteractionText()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
