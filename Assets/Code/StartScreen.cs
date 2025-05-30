using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public GameObject startScreenUI; // Panel z UI ekranu startowego
    public GameObject gameplayUI; // Panel z UI rozgrywki (opcjonalnie)
    public Image fadeImage; // Obraz do efektu fade out
    public float fadeDuration = 1f; // Czas trwania efektu fade out
    public PlayerMovement playerMovement; // Odniesienie do skryptu gracza

    private bool gameStarted = false;

    void Start()
    {
        // Na pocz¹tku poka¿ ekran startowy i zablokuj ruch gracza
        startScreenUI.SetActive(true);
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }

        // Zablokuj ruch gracza
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        // Ustaw fadeImage jako ca³kowicie czarne (dla zanikania)
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1);
        }
    }

    void Update()
    {
        // SprawdŸ, czy gracz nacisn¹³ spacjê
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        gameStarted = true;

        // Efekt fade out
        if (fadeImage != null)
        {
            float elapsedTime = 0f;
            Color color = fadeImage.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }

            fadeImage.gameObject.SetActive(false); // Wy³¹cz obraz po zakoñczeniu fade out
        }

        // Ukryj ekran startowy i poka¿ UI gameplayu
        startScreenUI.SetActive(false);
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(true);
        }

        // Odblokuj ruch gracza
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }
}
