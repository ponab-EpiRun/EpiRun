using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    // Adaptamos los tiempos
    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 3f;

    [Header("Spawn Position")]
    public float spawnOffsetX = 15f;
    public float obstacleY = -1.5f;

    [Header("Ground Check")]
    public float rayStartY = 5f;
    public float rayDistance = 20f;

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
        float multiplier = 1f;

        if (difficultyManager != null)
            multiplier = difficultyManager.spawnSpeedMultiplier;

        spawnTimer -= Time.deltaTime * multiplier;

        if (spawnTimer <= 0f)
        {
            SpawnObstacle();
            ResetTimer();
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Falta asignar obstaclePrefab en el objeto: " + gameObject.name);
            enabled = false;
            return;
        }

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        float spawnX = cameraRightEdge + spawnOffsetX;

        Vector2 rayOrigin = new Vector2(spawnX, rayStartY);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance);

        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red, 1f);

        if (hit.collider == null)
            return;

        if (!hit.collider.CompareTag("Ground"))
            return;

        Vector3 spawnPosition = new Vector3(spawnX, obstacleY, 0f);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}