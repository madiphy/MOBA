using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Bases")]
    [SerializeField] private Health playerBaseHealth;
    [SerializeField] private Health enemyBaseHealth;

    private bool _isGameOver = false;

    private void OnEnable()
    {
        if (playerBaseHealth != null)
            playerBaseHealth.OnDied += HandlePlayerBaseDestroyed;

        if (enemyBaseHealth != null)
            enemyBaseHealth.OnDied += HandleEnemyBaseDestroyed;
    }

    private void OnDisable()
    {
        if (playerBaseHealth != null)
            playerBaseHealth.OnDied -= HandlePlayerBaseDestroyed;

        if (enemyBaseHealth != null)
            enemyBaseHealth.OnDied -= HandleEnemyBaseDestroyed;
    }

    private void HandlePlayerBaseDestroyed()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        Debug.Log("<color=red>DEFEAT! Ваша база была уничтожена!</color>");
        Time.timeScale = 0f;
    }

    private void HandleEnemyBaseDestroyed()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        Debug.Log("<color=green>VICTORY! Вражеская база повержена!</color>");
        Time.timeScale = 0f;
    }
}