using UnityEngine;

public class FlyingEnemySpawner : MonoBehaviour
{
    public GameObject flyingEnemyPrefab;

    // Tiempo antes de empezar a generar enemigos
    public float startSpawningAfter = 20f;

    // Tiempo entre enemigos
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 6f;

    [Header("Spawn Position")]
    public float spawnOffsetX = 15f;
    public float minY = 0f;
    public float maxY = 3f;

    private float gameTimer;
    private float spawnTimer;
    private Camera mainCamera;
    private DifficultyManager difficultyManager;

    void Start()
    {
        mainCamera = Camera.main;
        difficultyManager = FindFirstObjectByType<DifficultyManager>();
        ResetTimer();
    }

    void Update()
    {
        gameTimer += Time.deltaTime;

        if (gameTimer < startSpawningAfter)
            return;

        float multiplier = 1f;

        if (difficultyManager != null)
            multiplier = difficultyManager.spawnSpeedMultiplier;

        spawnTimer -= Time.deltaTime * multiplier;

        if (spawnTimer <= 0f)
        {
            SpawnFlyingEnemy();
            ResetTimer();
        }
    }

    void SpawnFlyingEnemy()
    {
        if (flyingEnemyPrefab == null)
        {
            Debug.LogError("Falta asignar flyingEnemyPrefab en el objeto: " + gameObject.name);
            enabled = false;
            return;
        }

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        float spawnX = cameraRightEdge + spawnOffsetX;
        float spawnY = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        Instantiate(flyingEnemyPrefab, spawnPosition, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}