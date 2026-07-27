using System.Collections;
using UnityEngine;

public class CripSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject cripPrefab;
    [SerializeField] private Transform targetBase;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int cripsPerWave = 3;
    [SerializeField] private float delayBetweenCrips = 0.8f;

    private void Start()
    {
        StartCoroutine(SpawnWavesRoutine());
    }

    private IEnumerator SpawnWavesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < cripsPerWave; i++)
            {
                SpawnCrip();
                yield return new WaitForSeconds(delayBetweenCrips);
            }
        }
    }

    private void SpawnCrip()
    {
        if (cripPrefab == null || targetBase == null) return;

        GameObject crip = Instantiate(cripPrefab, transform.position, transform.rotation);

        if (crip.TryGetComponent<CripAI>(out var cripAI))
        {
            cripAI.Initialize(targetBase);
        }
    }
}