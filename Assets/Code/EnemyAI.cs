using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int attackDamage = 25;
    public float flashlightExposureTime = 4f;

    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip footstepSound;
    public AudioClip spawnSound;
    public AudioClip dissolveSound;

    public Renderer enemyRenderer; // Renderer do efektu zanikania
    public Material dissolveMaterial; // Shader do efektu rozpadu

    private Transform player;
    private NavMeshAgent agent;
    private PlayerStats playerStats;
    private Light[] lightsToTurnOff;
    private Door[] doorsToLock;

    private bool isFinalBoss = false;
    private bool isAttacking = false;
    private bool isDissolving = false;
    private float flashlightExposure = 0f;
    private Material originalMaterial;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerStats = playerObj.GetComponent<PlayerStats>();
            }
        }

        if (player == null)
        {
            Debug.LogError("EnemyAI: Nie znaleziono gracza!");
        }

        if (enemyRenderer != null)
        {
            originalMaterial = enemyRenderer.material;
        }

        StartCoroutine(PlayFootstepSounds());

        // DŸwiêk na start
        if (audioSource != null && spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
        if (playerStats != null)
        {
            Debug.Log("EnemyAI: PlayerStats zosta³ poprawnie znaleziony.");
        }
        else
        {
            Debug.LogError("EnemyAI: Nie uda³o siê znaleŸæ PlayerStats!");
        }
    }

    void Update()
    {
        if (player == null || isDissolving) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log($"EnemyAI: Odleg³oœæ do gracza: {distanceToPlayer}");

        if (distanceToPlayer <= detectionRange)
        {
            Debug.Log("EnemyAI: Gracz w zasiêgu wykrycia!");
            agent.SetDestination(player.position);

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                Debug.Log("EnemyAI: Gracz w zasiêgu ataku!");
                StartCoroutine(AttackPlayer());
            }
        }
        if (playerStats == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerStats = playerObj.GetComponent<PlayerStats>();
                Debug.Log("EnemyAI: Odnaleziono PlayerStats.");
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;


        Debug.Log("EnemyAI: Atakujê gracza!");

        if (playerStats != null)
        {
            Debug.Log("EnemyAI: Zadawanie obra¿eñ...");
            playerStats.TakeDamage(attackDamage);
            Debug.Log($"EnemyAI: Gracz otrzyma³ {attackDamage} obra¿eñ.");

            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
        else
        {
            Debug.LogError("EnemyAI: playerStats jest null! Gracz nie ma komponentu PlayerStats.");
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        agent.isStopped = false;

    }

    IEnumerator PlayFootstepSounds()
    {
        while (true)
        {
            if (agent.velocity.magnitude > 0.1f && audioSource != null && footstepSound != null)
            {
                audioSource.PlayOneShot(footstepSound);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void SetAsFinalBoss(bool isFinal)
    {
        isFinalBoss = isFinal;
    }

    public void SetLightsAndDoors(Light[] lights, Door[] doors)
    {
        lightsToTurnOff = lights;
        doorsToLock = doors;

        foreach (Light light in lightsToTurnOff)
        {
            if (light != null) light.enabled = false;
        }

        foreach (Door door in doorsToLock)
        {
            if (door != null) door.LockDoor();
        }
    }

    public void ApplyFlashlightDamage(float exposureTime)
    {
        if (isFinalBoss) return;

        flashlightExposure += exposureTime;
        if (flashlightExposure >= flashlightExposureTime)
        {
            StartCoroutine(DissolveAndDestroy());
        }
    }

    IEnumerator DissolveAndDestroy()
    {
        isDissolving = true;
        agent.isStopped = true;

        Debug.Log("Przeciwnik pokonany, przywracanie œwiate³ i odblokowywanie drzwi!");

        foreach (Light light in lightsToTurnOff)
        {
            if (light != null)
                light.enabled = true; // W³¹cz œwiat³o
        }

        foreach (Door door in doorsToLock)
        {
            if (door != null)
                door.UnlockDoor(); // Odblokuj drzwi
        }

        // DŸwiêk rozpadu
        if (audioSource != null && dissolveSound != null)
        {
            audioSource.PlayOneShot(dissolveSound);
        }

        // Efekt rozpuszczania
        if (enemyRenderer != null && dissolveMaterial != null)
        {
            enemyRenderer.material = dissolveMaterial;
            float dissolveAmount = 0f;
            while (dissolveAmount < 2f)
            {
                dissolveAmount += Time.deltaTime / 1.5f;
                enemyRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
