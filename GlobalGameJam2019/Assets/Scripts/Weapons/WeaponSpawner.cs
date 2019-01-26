using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] WeaponPrefabs;
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] int MaxWeapons = 5;
    
    int numberOfWeapons;
    float spawnDelay = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnDelay -= Time.deltaTime;
        if (spawnDelay <= 0)
        {
            if (numberOfWeapons < MaxWeapons)
            {
                SpawnAtRandomSpawnPoint(WeaponPrefabs[Random.Range(0, WeaponPrefabs.Length - 1)]);
            }
            spawnDelay = 1.0f;
        }
    }

    void SpawnAtRandomSpawnPoint(GameObject WeaponPrefab)
    {
        if (SpawnPoints.Length > 0)
        {
            Transform SpawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)];
            Instantiate(WeaponPrefab, SpawnPoint);
            numberOfWeapons++;
        }

    }
}
