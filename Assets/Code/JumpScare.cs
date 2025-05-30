using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public AudioClip scareSound;               // DŸwiêk jump scare
    public float flashlightFlickerDuration = 2f; // Czas trwania migania latarki
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return; // Zapobiegaj wielokrotnemu wywo³aniu
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            // ZnajdŸ skrypt latarki
            Flashlight flashlight = other.GetComponentInChildren<Flashlight>();

            if (flashlight != null)
            {
                // Uruchom jump scare
                StartCoroutine(TriggerJumpScare(flashlight));
            }
        }
    }

    private System.Collections.IEnumerator TriggerJumpScare(Flashlight flashlight)
    {
        // Odtwórz dŸwiêk
        AudioSource.PlayClipAtPoint(scareSound, transform.position);

        // Migotanie latarki
        float elapsedTime = 0f;
        bool isOn = true;
        while (elapsedTime < flashlightFlickerDuration)
        {
            elapsedTime += 0.1f;
            isOn = !isOn;
            flashlight.flashlight.enabled = isOn;
            yield return new WaitForSeconds(0.1f);
        }

        // Wy³¹cz latarkê na 2 sekundy
        flashlight.flashlight.enabled = false;
        yield return new WaitForSeconds(2f);

        // W³¹cz latarkê z powrotem, jeœli bateria na to pozwala
        if (flashlight.CurrentBatteryLife > 0)
        {
            flashlight.flashlight.enabled = true;
        }
    }
}
