using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public int medkitCount = 0;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 10f;
    public float staminaRegenRate = 5f;
    public float staminaRegenDelay = 2f;

    private bool isSprinting = false;
    private float staminaRegenTimer = 0f;

    [Header("Debug Settings")]
    public bool infiniteStamina = false;
    public bool infiniteHealth = false;

    [Header("UI Elements")]
    public Text healthText;
    public Text staminaText;
    public Text medkitText;
    public GameObject deathScreen; // UI screen for death

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
        UpdateUI();
    }

    private void Update()
    {
        HandleStamina();
        HandleHealing();
        CheckDeath();
    }

    public void TakeDamage(float amount)
    {
        if (!infiniteHealth)
        {
            currentHealth -= amount;
            if (currentHealth < 0) currentHealth = 0;
            UpdateUI();
            CheckDeath();
        }
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
        // Disable player controls (example)
        GetComponent<PlayerMovement>().enabled = false;
    }

    private void HandleHealing()
    {
        if (Input.GetKeyDown(KeyCode.Q) && medkitCount > 0 && currentHealth < maxHealth)
        {
            if (!infiniteHealth)
            {
                currentHealth = Mathf.Min(currentHealth + 50f, maxHealth);
            }
            medkitCount--;
            UpdateUI();
        }
    }

    public void AddMedkit()
    {
        medkitCount++;
        UpdateUI();
    }

    public void UseStamina(float amount)
    {
        if (!infiniteStamina)
        {
            currentStamina -= amount * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
            staminaRegenTimer = 0f;
            UpdateUI();
        }
    }

    private void HandleStamina()
    {
        if (!isSprinting && !infiniteStamina)
        {
            if (currentStamina < maxStamina)
            {
                staminaRegenTimer += Time.deltaTime;
                if (staminaRegenTimer >= staminaRegenDelay)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    if (currentStamina > maxStamina) currentStamina = maxStamina;
                }
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthText.text = "HP: " + currentHealth;
        staminaText.text = "Stamina: " + Mathf.Round(currentStamina);
        medkitText.text = "Medkits: " + medkitCount;
    }

    public void SetSprinting(bool sprinting)
    {
        isSprinting = sprinting;
    }
}
