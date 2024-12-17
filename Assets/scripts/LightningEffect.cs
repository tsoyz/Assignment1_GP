using System.Collections;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public Light lightningLight; // Assign the light in the inspector
    public float minIntensity = 0f;
    public float maxIntensity = 5f;
    public float flashDuration = 0.2f;
    public float timeBetweenFlashes = 2f;

    private float flashTimer;
    private bool isFlashing;

    void Update()
    {
        if (!isFlashing)
        {
            flashTimer += Time.deltaTime;

            if (flashTimer >= timeBetweenFlashes)
            {
                StartCoroutine(FlashLightning());
                flashTimer = 0f;
            }
        }
    }

    private IEnumerator FlashLightning()
    {
        isFlashing = true;

        // Simulate multiple quick flashes
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            lightningLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            lightningLight.intensity = 0;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        lightningLight.intensity = 0; // Ensure light is off after flash
        isFlashing = false;
    }
}
