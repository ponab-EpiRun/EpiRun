using UnityEngine;

public class PowerSpawner : MonoBehaviour
{
    // Prefab único de runa
    public GameObject powerRunePrefab;

    // Tiempo entre apariciones
    public float minSpawnTime = 12f;
    public float maxSpawnTime = 20f;

    // Alturas posibles
    public float minY = -1f;
    public float maxY = 3f;

    // Posición X de spawn
    public float spawnX = 12f;

    private float spawnTimer;
    private DifficultyManager difficultyManager;

    void Start()
    {
        difficultyManager = FindFirstObjectByType<DifficultyManager>();
        ResetTimer();
    }

    void Update()
    {
        float multiplier = 1f;

        if (difficultyManager != null)
            multiplier = difficultyManager.spawnSpeedMultiplier;

        spawnTimer -= Time.deltaTime * multiplier;

        if (spawnTimer <= 0f)
        {
            SpawnPowerRune();
            ResetTimer();
        }
    }

    void SpawnPowerRune()
    {
        if (powerRunePrefab == null)
            return;

        Vector3 pos = new Vector3(spawnX, Random.Range(minY, maxY), 0f);

        Instantiate(powerRunePrefab, pos, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}