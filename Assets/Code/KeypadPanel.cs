using UnityEngine;
using TMPro;

public class KeypadPanel : MonoBehaviour
{
    public TMP_Text codeDisplay;   // Wyświetlacz kodu (TextMeshPro)
    public TMP_Text hintText;      // Pole tekstowe dla podpowiedzi
    public string correctCode = "1234";  // Prawidłowy kod
    private string enteredCode = "";
    private bool playerInTrigger = false;
    private bool isUnlocked = false;
    public FuseBox fuseBox;  // Referencja do skrzynki z bezpiecznikami
    public Door door;  // Drzwi do otwarcia
    public GameObject spawnerObject;

    private void Start()
    {
        UpdateDisplay();
        hintText.text = "Napraw skrzynkę z bezpiecznikami!";
    }

    private void Update()
    {
        if (playerInTrigger && !isUnlocked)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) && enteredCode.Length < 4)
                {
                    enteredCode += c;
                    UpdateDisplay();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && enteredCode == correctCode)
            {
                UnlockDoor();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) && enteredCode.Length > 0)
            {
                enteredCode = enteredCode.Substring(0, enteredCode.Length - 1);
                UpdateDisplay();
            }
        }
    }

    private void UpdateDisplay()
    {
        if (fuseBox.IsFixed())
        {
            if (enteredCode == "")
                codeDisplay.text = "0000";
            else
                codeDisplay.text = enteredCode;

            hintText.text = playerInTrigger ? "Wpisz kod i naciśnij Enter" : "";
        }
        else
        {
            codeDisplay.text = "----";
            hintText.text = "Napraw skrzynkę z bezpiecznikami!";
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;
        codeDisplay.text = "✅";
        hintText.text = "Drzwi otwarte!";
        door.UnlockDoor();
        ActivateSpawner();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (fuseBox.IsFixed())
            {
                hintText.text = "Wpisz kod i naciśnij Enter";
            }
            else
            {
                hintText.text = "Najpierw napraw skrzynkę z bezpiecznikami!";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (!isUnlocked)
            {
                hintText.text = "";
            }
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
