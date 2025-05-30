using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public GameObject emergencyLightsParent; // Parent zapasowych lamp
    public GameObject normalLightsParent;    // Parent normalnych lamp
    public Pump pump;                        // Referencja do pompy
    public GameObject promptUI;              // UI z podpowiedzi¹ (ca³y panel)
    public Text uiText;                      // Pole tekstowe dla komunikatu
    private bool isPowered = false;
    private bool playerInTrigger = false;
    public float lightDelay = 0.3f;          // OpóŸnienie prze³¹czania œwiate³
    public GameObject spawnerObject;
    

    private void Update()
    {
        if (playerInTrigger && !isPowered && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ActivateGenerator());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            promptUI.SetActive(true);

            if (!isPowered)
            {
                uiText.text = "Naciœnij E, aby w³¹czyæ generator";
            }
            else
            {
                uiText.text = "Generator jest ju¿ w³¹czony";
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

    private IEnumerator ActivateGenerator()
    {
        isPowered = true;
        promptUI.SetActive(false);
        ActivateSpawner();

        // Pobranie wszystkich œwiate³ awaryjnych i ich stopniowe wy³¹czanie
        foreach (Transform light in emergencyLightsParent.transform)
        {
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(lightDelay);
        }

        // Pobranie wszystkich normalnych œwiate³ i ich stopniowe w³¹czanie
        foreach (Transform light in normalLightsParent.transform)
        {
            light.gameObject.SetActive(true);
            yield return new WaitForSeconds(lightDelay);
        }

        // Aktywacja pompy
        //pump.StartDrain();
        
    }

    public bool IsPowered()
    {
        return isPowered;
    }

    private void ActivateSpawner()
    {
        if (spawnerObject != null)
        {
            spawnerObject.SetActive(true);
        }
    }
}
