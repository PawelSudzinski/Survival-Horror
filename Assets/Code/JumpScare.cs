using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public AudioClip scareSound;               // D�wi�k jump scare
    public float flashlightFlickerDuration = 2f; // Czas trwania migania latarki
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return; // Zapobiegaj wielokrotnemu wywo�aniu
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            // Znajd� skrypt latarki
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
        // Odtw�rz d�wi�k
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

        // Wy��cz latark� na 2 sekundy
        flashlight.flashlight.enabled = false;
        yield return new WaitForSeconds(2f);

        // W��cz latark� z powrotem, je�li bateria na to pozwala
        if (flashlight.CurrentBatteryLife > 0)
        {
            flashlight.flashlight.enabled = true;
        }
    }
}
