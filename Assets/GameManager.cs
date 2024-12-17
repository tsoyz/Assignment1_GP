//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    private int currentEnemyCount = 0;
//    private int totalEnemyCount = 0;

//    public GameObject portal;
//    public Slider enemyProgressSlider;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void EnemyKilled(GameObject enemy)
//    {
//        if (currentEnemyCount > 0)
//        {
//            currentEnemyCount--;
//            Debug.Log("Enemy killed! Enemies left: " + currentEnemyCount);

//            // Update slider value dynamically
//            if (enemyProgressSlider != null)
//            {
//                enemyProgressSlider.value = totalEnemyCount - currentEnemyCount;
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Attempted to kill an enemy, but count is already zero.");
//        }

//        if (currentEnemyCount <= 0)
//        {
//            UnlockPortal();
//        }
//    }

//    public void RegisterEnemy(Enemy enemy)
//    {
//        if (enemy.isRegistered) return;

//        enemy.isRegistered = true;
//        enemy.OnEnemyDestroyed -= EnemyKilled; // Ensure no double subscription
//        enemy.OnEnemyDestroyed += EnemyKilled;

//        totalEnemyCount++;
//        currentEnemyCount++;

//        // Dynamically update slider max value and current value
//        if (enemyProgressSlider != null)
//        {
//            enemyProgressSlider.maxValue = totalEnemyCount;
//            enemyProgressSlider.value = totalEnemyCount - currentEnemyCount; // Keep progress updated
//        }
//    }

//    void UnlockPortal()
//    {
//        Debug.Log("All enemies killed! Portal is now unlocked.");
//        if (portal != null)
//        {
//            Portal portalScript = portal.GetComponent<Portal>();
//            if (portalScript != null)
//            {
//                portalScript.UnlockPortal();
//            }
//        }
//    }

//    public void WinGame()
//    {
//        Debug.Log("Player has won!");
//        SceneManager.LoadScene("WinScene");
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentEnemyCount = 0;
    private int totalEnemyCount = 0;

    [Header("Portal and Enemy UI")]
    public GameObject portal;
    public Slider enemyProgressSlider;

    [Header("Start Screen and Gameplay UI")]
    public GameObject startScreenPanel;  // Start Screen UI Panel
    public Button startButton;           // Start Button
    public GameObject gameplayUIPanel;   // Gameplay UI (health bars, etc.)

    [Header("Cutscene")]
    public PlayableDirector cutsceneDirector; // Director to play the Timeline Cutscene

    private bool gameStarted = false;    // Prevents multiple start triggers

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
        // Show Start Screen and hide gameplay UI initially
        startScreenPanel.SetActive(true);
        gameplayUIPanel.SetActive(false);

        // Assign Start Button functionality
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonPressed);
        }
        else
        {
            Debug.LogError("Start Button not assigned!");
        }

        // Initialize enemy progress slider
        if (enemyProgressSlider != null)
        {
            enemyProgressSlider.value = 0;
        }
    }

    void OnStartButtonPressed()
    {
        if (gameStarted) return; // Prevent double clicks
        gameStarted = true;


        gameplayUIPanel.SetActive(true);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Game started. Playing cutscene...");

        // Hide Start Screen Panel
        startScreenPanel.SetActive(false);

        // Play cutscene
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            BlockPlayerInput();
            StartCoroutine(ShowGameplayUIAfterCutscene());
        }
        else
        {
            Debug.LogWarning("Cutscene Director not assigned! Skipping cutscene...");
            gameplayUIPanel.SetActive(true); // Directly show UI if no cutscene
        }
    }

    System.Collections.IEnumerator ShowGameplayUIAfterCutscene()
    {
        // Wait for the cutscene to finish
        yield return new WaitForSeconds((float)cutsceneDirector.duration);

        // Show the gameplay UI
        gameplayUIPanel.SetActive(true);
        RestorePlayerInput();
        Debug.Log("Gameplay UI is now visible.");
    }
    void BlockPlayerInput()
    {
        // Example: Disable EventSystem for UI input
        EventSystem.current.enabled = false;

        // Block cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Optionally, disable your player input components or script
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
    }

    void RestorePlayerInput()
    {
        EventSystem.current.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
    }
    // Enemy-related methods
    public void EnemyKilled(GameObject enemy)
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
            Debug.Log("Enemy killed! Enemies left: " + currentEnemyCount);

            // Update slider dynamically
            if (enemyProgressSlider != null)
            {
                enemyProgressSlider.value = totalEnemyCount - currentEnemyCount;
            }
        }
        else
        {
            Debug.LogWarning("Attempted to kill an enemy, but count is already zero.");
        }

        if (currentEnemyCount <= 0)
        {
            UnlockPortal();
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy.isRegistered) return;

        enemy.isRegistered = true;
        enemy.OnEnemyDestroyed -= EnemyKilled; // Prevent double subscription
        enemy.OnEnemyDestroyed += EnemyKilled;

        totalEnemyCount++;
        currentEnemyCount++;

        // Dynamically set slider values
        if (enemyProgressSlider != null)
        {
            enemyProgressSlider.maxValue = totalEnemyCount;
            enemyProgressSlider.value = totalEnemyCount - currentEnemyCount;
        }
    }

    void UnlockPortal()
    {
        Debug.Log("All enemies killed! Portal is now unlocked.");
        if (portal != null)
        {
            Portal portalScript = portal.GetComponent<Portal>();
            if (portalScript != null)
            {
                portalScript.UnlockPortal();
            }
        }
    }

    public void WinGame()
    {
        Debug.Log("Player has won!");
        SceneManager.LoadScene("WinScene");
    }
}
