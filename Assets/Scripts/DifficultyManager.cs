using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Time")]
    public float difficultyTimer;

    [Header("World Speed")]
    public float startMoveSpeed = 3f;
    public float maxMoveSpeed = 7f;
    public float speedIncreasePerSecond = 0.025f;

    [Header("Current Difficulty Values")]
    public float currentMoveSpeed;
    public float spawnSpeedMultiplier = 1f;

    [Header("Power Multipliers")]
    public float moveSpeedMultiplier = 1f;
    public float scoreMultiplier = 1f;

    [Header("Obstacle Difficulty")]
    public float minObstacleSpawnTime = 0.8f;
    public float maxObstacleSpawnTime = 2f;

    [Header("Gem Difficulty")]
    public float minGemSpawnTime = 2f;
    public float maxGemSpawnTime = 5f;

    [Header("Energy Difficulty")]
    public float startEnergyDrain = 5f;
    public float maxEnergyDrain = 12f;
    public float energyDrainIncreasePerSecond = 0.02f;

    private ObstacleSpawner obstacleSpawner;
    private GemSpawner gemSpawner;
    private GameHUDManager hudManager;

    void Start()
    {
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        gemSpawner = FindFirstObjectByType<GemSpawner>();
        hudManager = FindFirstObjectByType<GameHUDManager>();
    }

    void Update()
    {
        difficultyTimer += Time.deltaTime;

        UpdateMoveSpeed();
        UpdateSpawnSpeedMultiplier();
        UpdateObstacleSpawner();
        UpdateGemSpawner();
        UpdateEnergyDrain();
    }

    void UpdateMoveSpeed()
    {
        currentMoveSpeed = Mathf.Clamp(
            startMoveSpeed + difficultyTimer * speedIncreasePerSecond,
            startMoveSpeed,
            maxMoveSpeed
        );

        currentMoveSpeed *= moveSpeedMultiplier;

        MoveLeft[] movers = FindObjectsByType<MoveLeft>(FindObjectsSortMode.None);

        for (int i = 0; i < movers.Length; i++)
        {
            movers[i].speed = currentMoveSpeed;
        }
    }

    void UpdateSpawnSpeedMultiplier()
    {
        spawnSpeedMultiplier = currentMoveSpeed / startMoveSpeed;
        spawnSpeedMultiplier = Mathf.Clamp(spawnSpeedMultiplier, 1f, 2.5f);
    }

    void UpdateObstacleSpawner()
    {
        if (obstacleSpawner == null)
            return;

        float progress = Mathf.Clamp01(difficultyTimer / 120f);

        obstacleSpawner.minSpawnTime = Mathf.Lerp(1.5f, minObstacleSpawnTime, progress);
        obstacleSpawner.maxSpawnTime = Mathf.Lerp(3f, maxObstacleSpawnTime, progress);
    }

    void UpdateGemSpawner()
    {
        if (gemSpawner == null)
            return;

        gemSpawner.minSpawnTime = minGemSpawnTime;
        gemSpawner.maxSpawnTime = maxGemSpawnTime;
    }

    void UpdateEnergyDrain()
    {
        if (hudManager == null)
            return;

        hudManager.energyDrainPerSecond = Mathf.Clamp(
            startEnergyDrain + difficultyTimer * energyDrainIncreasePerSecond,
            startEnergyDrain,
            maxEnergyDrain
        );
    }
}