using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemies;
    public GameObject[] hiddenEnemies;
    public GameObject finalBoss;
    public GameObject[] keys;
    public GameObject finalDoor;
    public AudioSource ambientSound;
    public Light flickeringLight;

    private bool keyCollected = false;
    private bool finalBattleStarted = false;

    void Start()
    {
        ambientSound.Play();
        StartCoroutine(FlickerLight());
    }

    void Update()
    {
        if (keyCollected && Vector3.Distance(player.transform.position, finalDoor.transform.position) < 2f)
        {
            finalDoor.SetActive(false);
            StartFinalBattle();
        }
    }

    public void CollectKey()
    {
        keyCollected = true;
        Debug.Log("Key Collected!");
    }

    void StartFinalBattle()
    {
        if (!finalBattleStarted)
        {
            finalBattleStarted = true;
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
            foreach (GameObject hiddenEnemy in hiddenEnemies)
            {
                hiddenEnemy.SetActive(true);
            }
            finalBoss.SetActive(true);
        }
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            flickeringLight.enabled = !flickeringLight.enabled;
        }
    }
}