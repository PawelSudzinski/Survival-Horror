using UnityEngine;

public class OrangeLight : MonoBehaviour
{
    private Light pointLight;
    public float minIntensity = 2f;
    public float maxIntensity = 5f;
    public float flickerSpeed = 0.1f;
    public float pulseSpeed = 2f; // Speed of the pulsing effect

    void Start()
    {
        // Add a Point Light component if it doesn't exist
        pointLight = gameObject.GetComponent<Light>();
        if (pointLight == null)
        {
            pointLight = gameObject.AddComponent<Light>();
        }

        // Set up the light properties
        pointLight.type = LightType.Point;
        pointLight.color = new Color(1f, 0.5f, 0f); // Orange
        pointLight.range = 10f;
        pointLight.intensity = maxIntensity;
    }

    void Update()
    {
        // Flickering effect
        float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);

        // Pulse effect using PingPong
        float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);

        // Combine flickering and pulsing
        pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, flicker * pulse);
    }
}
