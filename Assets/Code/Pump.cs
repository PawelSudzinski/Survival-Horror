using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pump : MonoBehaviour
{
    public GameObject water;   // Obiekt reprezentuj�cy wod�
    public float drainSpeed = 2f;
    public GameObject promptUI; // UI z podpowiedzi� (ca�y panel)
    public Text uiText;        // Tekst informuj�cy o stanie pompy i generatora
    public Door[] doors;       // Referencje do �luz
    public Generator generator; // Referencja do generatora
    private bool isDraining = false;
    private bool playerInTrigger = false;
    private bool hasDrained = false; // Flaga, czy pompa by�a ju� u�yta
    public GameObject spawnerObject;
    private void Update()
    {
        if (playerInTrigger && !isDraining && !hasDrained && generator.IsPowered() && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartDrain());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            promptUI.SetActive(true);

            if (hasDrained)
            {
                uiText.text = "Pompa zosta�a ju� u�yta";
            }
            else if (!generator.IsPowered())
            {
                uiText.text = "Najpierw uruchom generator!";
            }
            else
            {
                uiText.text = "Naci�nij E, aby uruchomi� pomp�";
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

    private IEnumerator StartDrain()
    {
        isDraining = true;
        hasDrained = true;
        promptUI.SetActive(false);

        while (water.transform.position.y > -5f) // Przesuwanie wody w d�
        {
            water.transform.position += Vector3.down * drainSpeed * Time.deltaTime;
            yield return null;
        }
        ActivateSpawner();
        UnlockDoors();
        
    }

    private void UnlockDoors()
    {
        
        foreach (Door door in doors)
        {
            door.UnlockDoor();

        }
    }
    private void ActivateSpawner()
    {
        if (spawnerObject != null)
        {
            spawnerObject.SetActive(true);
        }
    }
}
