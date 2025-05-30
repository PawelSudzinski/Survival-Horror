using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fuse : MonoBehaviour
{
    public Text hintText;  // Referencja do zwyk³ego pola tekstowego UI
    private bool playerInTrigger = false;
    private bool isCollected = false;  // Czy bezpiecznik zosta³ ju¿ podniesiony?

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            playerInTrigger = true;
            hintText.text = "Naciœnij E, aby podnieœæ bezpiecznik";
            hintText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            hintText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInTrigger && !isCollected && Input.GetKeyDown(KeyCode.E))
        {
            CollectFuse();
        }
    }

    private void CollectFuse()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>(); // Znalezienie ekwipunku gracza
        if (inventory != null)
        {
            inventory.AddFuse(1); // Dodanie 1 bezpiecznika do ekwipunku
        }

        isCollected = true;
        hintText.text = "Bezpiecznik podniesiony!";
        StartCoroutine(HideHint());
        gameObject.SetActive(false);  // Ukrywa bezpiecznik po podniesieniu
    }

    private IEnumerator HideHint()
    {
        yield return new WaitForSeconds(2f);
        hintText.gameObject.SetActive(false);
    }
}