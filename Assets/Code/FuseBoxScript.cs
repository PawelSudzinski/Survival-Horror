using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuseBox : MonoBehaviour
{
    public GameObject emergencyLightsParent; // Parent zapasowych lamp
    public GameObject normalLightsParent;    // Parent normalnych lamp
    public PlayerInventory playerInventory;  // Referencja do ekwipunku gracza
    public GameObject promptUI;              // UI z podpowiedzi¹
    public Text promptText;                  // Tekst informuj¹cy
    private bool isFixed = false;
    private bool playerInTrigger = false;
    public float lightDelay = 0.3f;          // OpóŸnienie prze³¹czania œwiate³

    private void Update()
    {
        if (playerInTrigger && !isFixed && Input.GetKeyDown(KeyCode.E))
        {
            TryFixFuseBox();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            promptUI.SetActive(true);

            if (!isFixed)
            {
                promptText.text = playerInventory.HasEnoughFuses(2)
                    ? "Naciœnij E, aby naprawiæ skrzynkê."
                    : "Potrzebujesz 2 bezpieczników!";
            }
            else
            {
                promptText.text = "Skrzynka ju¿ naprawiona.";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            promptUI.SetActive(false);
        }
    }

    private void TryFixFuseBox()
    {
        if (playerInventory.HasEnoughFuses(2))
        {
            playerInventory.UseFuses(2);
            StartCoroutine(ActivatePower());
        }
        else
        {
            promptText.text = "Brak wystarczaj¹cej liczby bezpieczników!";
        }
    }

    private IEnumerator ActivatePower()
    {
        isFixed = true;
        promptText.text = "Zasilanie przywrócone!";

        // Stopniowe wy³¹czanie œwiate³ awaryjnych
        foreach (Transform light in emergencyLightsParent.transform)
        {
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(lightDelay);
        }

        // Stopniowe w³¹czanie normalnych œwiate³
        foreach (Transform light in normalLightsParent.transform)
        {
            light.gameObject.SetActive(true);
            yield return new WaitForSeconds(lightDelay);
        }
    }

    public bool IsFixed()
    {
        return isFixed;
    }
}
