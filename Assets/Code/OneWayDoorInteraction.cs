using UnityEngine;
using UnityEngine.UI;

public class OneWayDoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public Animator doorAnimator;
    public string openAnimationName = "DoorOpen";
    public string closeAnimationName = "DoorClose";
    private bool isDoorOpen = false;
    private bool isPlayerNear = false;
    private bool hasBeenOpened = false; // Czy drzwi by³y ju¿ otwierane

    [Header("One-Way Settings")]
    public Transform allowedSide; // Punkt okreœlaj¹cy, z której strony mo¿na otworzyæ drzwi pierwszy raz

    [Header("Key Settings")]
    public bool requireKey = false;
    public string requiredKeyName;
    private PlayerInventory playerInventory;

    [Header("UI Settings")]
    public Text interactionText;
    public string openMessage = "Press E to open";
    public string closeMessage = "Press E to close";
    public string missingKeyMessage = "You need the correct key to open this door.";
    public string wrongSideMessage = "You cannot open the door from this side.";

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }

        if (requireKey && playerInventory == null)
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
        if (!hasBeenOpened)
        {
            // Sprawdza, czy gracz jest po dobrej stronie drzwi
            if (!IsPlayerOnAllowedSide())
            {
                ShowMessage(wrongSideMessage);
                return;
            }
        }

        if (!requireKey || (playerInventory != null && playerInventory.HasKey(requiredKeyName)))
        {
            ToggleDoor();
            hasBeenOpened = true; // Po pierwszym otwarciu mo¿na korzystaæ normalnie
        }
        else
        {
            ShowMessage(missingKeyMessage);
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
        }

        isDoorOpen = !isDoorOpen;

        if (interactionText != null)
        {
            interactionText.text = isDoorOpen ? closeMessage : openMessage;
        }
    }

    private bool IsPlayerOnAllowedSide()
    {
        if (allowedSide == null)
        {
            return true; // Jeœli nie ma okreœlonej strony, otwórz normalnie
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        float playerDistanceToAllowedSide = Vector3.Distance(player.transform.position, allowedSide.position);
        float doorDistanceToAllowedSide = Vector3.Distance(transform.position, allowedSide.position);

        return playerDistanceToAllowedSide < doorDistanceToAllowedSide;
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

    private void ShowMessage(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
            Invoke(nameof(HideInteractionText), 2f);
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
