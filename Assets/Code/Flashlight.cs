using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;                     // Obiekt œwiat³a
    public KeyCode toggleKey = KeyCode.E;        // Klawisz w³¹czania/wy³¹czania latarki
    public float maxBatteryLife = 100f;          // Maksymalny poziom baterii
    public float batteryDrainRate = 10f;         // Tempo zu¿ywania baterii
    public Transform playerCamera;               // Kamera gracza
    public float flashlightRotationSpeed = 5f;   // Szybkoœæ pod¹¿ania latarki za kamer¹

    private float currentBatteryLife;            // Obecny poziom baterii
    private bool isFlashlightOn = false;
    private PlayerInventory playerInventory;     // Odniesienie do skryptu ekwipunku

    // Publiczna w³aœciwoœæ, aby inne skrypty mog³y odczytaæ poziom baterii
    public float CurrentBatteryLife
    {
        get { return currentBatteryLife; }
    }

    void Start()
    {
        currentBatteryLife = maxBatteryLife;     // Ustaw bateriê na maksymalny poziom na start
        flashlight.enabled = false;             // Latarka domyœlnie wy³¹czona

        // ZnajdŸ komponent PlayerInventory na obiekcie gracza
        playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory not found in the scene!");
        }
    }

    void Update()
    {
        if (isFlashlightOn)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 10f))
            {
                EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.ApplyFlashlightDamage(Time.deltaTime);
                }
            }
        }
        // Obs³uga w³¹czania/wy³¹czania latarki
        if (Input.GetKeyDown(toggleKey) && currentBatteryLife > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;
        }

        // Zu¿ycie baterii, gdy latarka jest w³¹czona
        if (isFlashlightOn)
        {
            currentBatteryLife -= batteryDrainRate * Time.deltaTime;

            if (currentBatteryLife <= 0f)
            {
                currentBatteryLife = 0f;
                isFlashlightOn = false;
                flashlight.enabled = false;
            }
        }

        // Rotacja latarki pod¹¿aj¹ca za kamer¹ gracza
        RotateFlashlight();

        // Obs³uga prze³adowania baterii
        if (Input.GetMouseButtonDown(2)) // Middle mouse button
        {
            ReloadBattery();
        }
    }

    private void RotateFlashlight()
    {
        if (playerCamera != null)
        {
            Quaternion targetRotation = playerCamera.rotation;
            flashlight.transform.rotation = Quaternion.Lerp(
                flashlight.transform.rotation,
                targetRotation,
                Time.deltaTime * flashlightRotationSpeed
            );
        }
    }

    private void ReloadBattery()
    {
        // Sprawdzenie, czy jest mo¿liwoœæ zu¿ycia baterii z ekwipunku
        if (playerInventory != null && currentBatteryLife < maxBatteryLife)
        {
            if (playerInventory.ConsumeBattery()) // Zu¿yj bateriê
            {
                currentBatteryLife = maxBatteryLife; // Ustaw poziom baterii na maksymalny
                Debug.Log("Battery reloaded!");
            }
            else
            {
                Debug.Log("No batteries in inventory!");
            }
        }
    }

    // Wyœwietlanie informacji na ekranie
    void OnGUI()
    {
        if (playerInventory != null)
        {
            // Pobierz liczbê baterii z ekwipunku
            int batteryCount = playerInventory.GetBatteryCount();

            // Wyœwietlenie informacji na ekranie
            GUI.Label(new Rect(10, 10, 200, 20), "Battery Life: " + Mathf.CeilToInt(currentBatteryLife) + "%");
            GUI.Label(new Rect(10, 30, 200, 20), "Batteries: " + batteryCount);
        }
    }
}
