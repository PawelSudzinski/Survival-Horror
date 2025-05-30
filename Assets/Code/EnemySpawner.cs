using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefaby")]
    public GameObject enemyPrefab; // Prefab zwyk≥ego przeciwnika
    public GameObject finalBossPrefab; // Prefab finalnego bossa
    public Transform player; // Referencja do gracza

    [Header("Spawnowanie")]
    public float spawnDistance = 5f; // Odleg≥oúÊ za graczem
    public float navMeshCheckRadius = 3f; // PromieÒ szukania poprawnego miejsca na NavMesh
    public bool isFinalBoss = false; // Czy to fina≥owy boss?
    public bool useCustomSpawnPoint = false; // Czy uøywaÊ w≥asnego punktu?
    public Transform customSpawnPoint; // W≥asny punkt spawn

    [Header("Efekty düwiÍkowe")]
    public AudioSource audioSource; // èrÛd≥o düwiÍku
    public AudioClip spawnSound; // DüwiÍk spawnu przeciwnika
    public bool useSpawnerAudioSource = true; // Czy uøywaÊ wbudowanego AudioSource?

    [Header("åwiat≥a i drzwi")]
    public Light[] lightsToTurnOff; // åwiat≥a do wy≥πczenia
    public Door[] doorsToLock; // Drzwi do zablokowania
    public DoorInteraction doorToToggle; // Drzwi do otwarcia/zamkniÍcia
    public bool toggleDoorOnSpawn = false; // Czy otworzyÊ/zamknπÊ drzwi?

    private bool hasSpawned;

    private void Start()
    {
        // Jeúli mamy uøywaÊ w≥asnego AudioSource, a nie ma go na obiekcie, dodaj go
        if (useSpawnerAudioSource && audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpawned || !other.CompareTag("Player")) return; // Zapobiega wielokrotnemu spawn

        Debug.Log("Gracz wszed≥ w trigger - spawn przeciwnika!");
        SpawnEnemy();
        hasSpawned = true;

        // Odtworzenie düwiÍku spawnu
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        // Jeúli wybrano, prze≥πcz drzwi
        if (toggleDoorOnSpawn && doorToToggle != null)
        {
            doorToToggle.ToggleDoor();
        }
    }

    void SpawnEnemy()
    {
        if (player == null)
        {
            Debug.LogError("EnemySpawner: Brak referencji do gracza!");
            return;
        }

        // WybÛr prefabu przeciwnika
        GameObject prefabToSpawn = isFinalBoss ? finalBossPrefab : enemyPrefab;
        if (prefabToSpawn == null)
        {
            Debug.LogError("EnemySpawner: Nie przypisano prefabu przeciwnika!");
            return;
        }

        // Ustal pozycjÍ spawnu
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (useCustomSpawnPoint && customSpawnPoint != null)
        {
            spawnPosition = customSpawnPoint.position;
            spawnRotation = customSpawnPoint.rotation;
        }
        else
        {
            spawnPosition = player.position - player.forward * spawnDistance;
            spawnRotation = Quaternion.LookRotation(player.forward);

            // Znajdü najbliøszπ pozycjÍ na NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, navMeshCheckRadius, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
            }
            else
            {
                Debug.LogWarning("EnemySpawner: Nie znaleziono miejsca na NavMesh! Spawnowanie w domyúlnej pozycji.");
            }
        }

        // Tworzenie przeciwnika
        GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        // Konfiguracja przeciwnika
        if (enemyAI != null)
        {
            enemyAI.SetPlayer(player);
            enemyAI.SetAsFinalBoss(isFinalBoss);
            enemyAI.SetLightsAndDoors(lightsToTurnOff, doorsToLock);
        }
    }
}