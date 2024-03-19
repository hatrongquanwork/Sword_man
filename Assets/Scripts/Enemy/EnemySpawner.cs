using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab; 
    public float spawnInterval;
    public int maxEnemies; 
    public Transform[] spawnPoints; 

    private int enemy_count = 0; 
    private float timeSinceLastSpawn; 

    private void Update()
    {
        if (enemy_count < maxEnemies && timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
            enemy_count++;
        }

        timeSinceLastSpawn += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefabs = enemyPrefab[Random.Range(0, enemyPrefab.Length)];
        GameObject newEnemy = Instantiate(enemyPrefabs, spawnPoint.position, spawnPoint.rotation);

    }

    public void EnemyDied(GameObject enemy)
    {
        //enemy_count--;
    }
}