using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    //Adaptamos los tiempos
    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 3f;

    private float spawnTimer;

    void Start()
    {
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
        Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
    }

    void ResetTimer()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}