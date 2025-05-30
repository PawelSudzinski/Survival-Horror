using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;                     // Obiekt �wiat�a
    public KeyCode toggleKey = KeyCode.E;        // Klawisz w��czania/wy��czania latarki
    public float maxBatteryLife = 100f;          // Maksymalny poziom baterii
    public float batteryDrainRate = 10f;         // Tempo zu�ywania baterii
    public Transform playerCamera;               // Kamera gracza
    public float flashlightRotationSpeed = 5f;   // Szybko�� pod��ania latarki za kamer�

    private float currentBatteryLife;            // Obecny poziom baterii
    private bool isFlashlightOn = false;
    private PlayerInventory playerInventory;     // Odniesienie do skryptu ekwipunku

    // Publiczna w�a�ciwo��, aby inne skrypty mog�y odczyta� poziom baterii
    public float CurrentBatteryLife
    {
        get { return currentBatteryLife; }
    }

    void Start()
    {
        currentBatteryLife = maxBatteryLife;     // Ustaw bateri� na maksymalny poziom na start
        flashlight.enabled = false;             // Latarka domy�lnie wy��czona

        // Znajd� komponent PlayerInventory na obiekcie gracza
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
        // Obs�uga w��czania/wy��czania latarki
        if (Input.GetKeyDown(toggleKey) && currentBatteryLife > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;
        }

        // Zu�ycie baterii, gdy latarka jest w��czona
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

        // Rotacja latarki pod��aj�ca za kamer� gracza
        RotateFlashlight();

        // Obs�uga prze�adowania baterii
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
        // Sprawdzenie, czy jest mo�liwo�� zu�ycia baterii z ekwipunku
        if (playerInventory != null && currentBatteryLife < maxBatteryLife)
        {
            if (playerInventory.ConsumeBattery()) // Zu�yj bateri�
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

    // Wy�wietlanie informacji na ekranie
    void OnGUI()
    {
        if (playerInventory != null)
        {
            // Pobierz liczb� baterii z ekwipunku
            int batteryCount = playerInventory.GetBatteryCount();

            // Wy�wietlenie informacji na ekranie
            GUI.Label(new Rect(10, 10, 200, 20), "Battery Life: " + Mathf.CeilToInt(currentBatteryLife) + "%");
            GUI.Label(new Rect(10, 30, 200, 20), "Batteries: " + batteryCount);
        }
    }
}
