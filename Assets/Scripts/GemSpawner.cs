using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    public GameObject gemPrefab;

    // Adaptamos los tiempos de aparición
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    [Header("Spawn Position")]
    public float spawnOffsetX = 15f;
    public float minY = -0.5f;
    public float maxY = 2.5f;

    private float spawnTimer;
    private DifficultyManager difficultyManager;
    private Camera mainCamera;

    void Start()
    {
        difficultyManager = FindFirstObjectByType<DifficultyManager>();
        mainCamera = Camera.main;
        ResetTimer();
    }

    void Update()
    {
        float multiplier = 1f;

        if (difficultyManager != null)
        {
            multiplier = difficultyManager.spawnSpeedMultiplier;
        }

        spawnTimer -= Time.deltaTime * multiplier;

        if (spawnTimer <= 0f)
        {
            SpawnGem();
            ResetTimer();
        }
    }

    void SpawnGem()
    {
        if (gemPrefab == null)
            return;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        float spawnX = cameraRightEdge + spawnOffsetX;
        float spawnY = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        Instantiate(gemPrefab, spawnPosition, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}