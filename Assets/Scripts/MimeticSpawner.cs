using UnityEngine;

public class MimeticSpawner : MonoBehaviour
{
    public GameObject MimeticPrefab;

    // Tiempo antes de empezar a generar Mimetic
    public float startSpawningAfter = 30f;

    // Tiempo entre Mimetic
    public float minSpawnTime = 4f;
    public float maxSpawnTime = 8f;

    [Header("Spawn Position")]
    public float spawnOffsetX = 8f;
    public float MimeticY = -1.93f;

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
            SpawnMimetic();
            ResetTimer();
        }
    }

    void SpawnMimetic()
    {
        if (MimeticPrefab == null)
        {
            Debug.LogError("Falta asignar MimeticPrefab en el objeto: " + gameObject.name);
            enabled = false;
            return;
        }

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        float spawnX = cameraRightEdge + spawnOffsetX;

        Vector3 spawnPosition = new Vector3(spawnX, MimeticY, 0f);

        Instantiate(MimeticPrefab, spawnPosition, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}