using System.Collections;
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

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnPowerRune();
        }
    }

    void SpawnPowerRune()
    {
        if (powerRunePrefab == null)
            return;

        Vector3 pos = new Vector3(spawnX, Random.Range(minY, maxY), 0f);

        Instantiate(powerRunePrefab, pos, Quaternion.identity);
    }
}