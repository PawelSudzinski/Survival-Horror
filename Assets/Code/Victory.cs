using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public GameObject victoryScreen; // UI ekranu zwyci�stwa
    public Text victoryText; // Tekst na ekranie zwyci�stwa
    public string winMessage = "Gratulacje! Wygra�e�!"; // Wiadomo�� do wy�wietlenia

    private bool gamePaused = false;

    void Start()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false); // Ukryj ekran na start
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gamePaused)
        {
            ShowVictoryScreen();
        }
    }

    void ShowVictoryScreen()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }

        if (victoryText != null)
        {
            victoryText.text = winMessage;
        }

        Time.timeScale = 0f; // Pauza gry
        gamePaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Wznowienie gry
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }
    }
}
