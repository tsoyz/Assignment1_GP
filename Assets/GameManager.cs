using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int currentEnemyCount = 0;
    private int totalEnemyCount = 0;

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

    public void EnemyKilled(GameObject enemy)
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
            Debug.Log("Enemy killed! Enemies left: " + currentEnemyCount);
        }
        else
        {
            Debug.LogWarning("Attempted to kill an enemy, but count is already zero.");
        }

        if (currentEnemyCount <= 0)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        Debug.Log("Player has won!");
        SceneManager.LoadScene("WinScene");
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy.isRegistered) return;

        enemy.isRegistered = true;
        enemy.OnEnemyDestroyed -= EnemyKilled; // Ensure no double subscription
        enemy.OnEnemyDestroyed += EnemyKilled;
        totalEnemyCount++;
        currentEnemyCount++;
        Debug.Log("Registering enemy: " + enemy.gameObject.name);
    }
}