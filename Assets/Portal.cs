using UnityEngine;
using TMPro;

public class Portal : MonoBehaviour
{
    private bool isUnlocked = false; // Tracks if the portal is unlocked
    private Renderer portalRenderer; // Reference to the portal's renderer
    private Collider portalCollider; // Reference to the portal's collider

    // Colors for locked and unlocked states
    public Color lockedColor = Color.red;     // Red color when locked
    public Color unlockedColor = Color.green; // Green color when unlocked
    public TextMeshProUGUI PortalText;


    void Start()
    {
        // Get the portal's renderer and collider components
        portalRenderer = GetComponent<Renderer>();
        portalCollider = GetComponent<Collider>();

        // Ensure collider starts inactive and material starts red
        portalCollider.enabled = false; // Portal is non-interactive at start
        if (portalRenderer != null && portalRenderer.material.HasProperty("_MainColor"))
        {
            portalRenderer.material.SetColor("_MainColor", lockedColor); // Set initial color to red
        }
    }

    // Called by the GameManager when all enemies are killed
    public void UnlockPortal()
    {
        isUnlocked = true;

        // Change color to green and enable collider
        if (portalRenderer != null && portalRenderer.material.HasProperty("_MainColor"))
        {
            portalRenderer.material.SetColor("_MainColor", unlockedColor); // Set color to green
        }
        portalCollider.enabled = true; // Allow player interaction
        PortalText.text = "Portal is unlocked!";
        //Debug.Log("Portal is now unlocked!");
    }

    // Trigger event for player interaction
    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked && other.CompareTag("Player")) // Only works if unlocked and player enters
        {
            //Debug.Log("Player entered the portal!");
            GameManager.Instance.WinGame(); // Call GameManager to end the game
        }
        else if (!isUnlocked && other.CompareTag("Player"))
        {
            PortalText.text = "The portal is still locked! \n Kill all enemies first.";

            Debug.Log("The portal is still locked!  Kill all enemies first.");
        }
    }
}
