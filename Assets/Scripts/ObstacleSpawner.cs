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

    private float spawnTimer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        ResetTimer();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnObstacle();
            ResetTimer();
        }
    }

    void SpawnObstacle()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float cameraRightEdge = mainCamera.transform.position.x + camWidth;

        float spawnX = cameraRightEdge + spawnOffsetX;

        Vector3 spawnPosition = new Vector3(spawnX, obstacleY, 0f);

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}