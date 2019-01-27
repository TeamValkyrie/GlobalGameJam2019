using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] WeaponPrefabs;
    GameObject[] SpawnPoints;
    [SerializeField] int MaxWeapons = 5;

    [SerializeField] private float maxSpawnDelay = 0.1f;
    int numberOfWeapons;
    float spawnDelay = 1.0f;

    private void Start()
    {
        spawnDelay = maxSpawnDelay;
        SpawnPoints = GameObject.FindGameObjectsWithTag("WeaponSpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        spawnDelay -= Time.deltaTime;
        if (spawnDelay <= 0)
        {
            if (numberOfWeapons < MaxWeapons)
            {
                SpawnAtRandomSpawnPoint(WeaponPrefabs[Random.Range(0, WeaponPrefabs.Length)]);
            }
            spawnDelay = maxSpawnDelay;
        }
    }

    void SpawnAtRandomSpawnPoint(GameObject WeaponPrefab)
    {
        if (SpawnPoints.Length > 0)
        {
            Transform SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform;
            Instantiate(WeaponPrefab, SpawnPoint);
            numberOfWeapons++;
        }

    }
}
