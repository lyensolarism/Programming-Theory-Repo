using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public GameObject powerupPrefab;
    public int maxEnemyWave;

    public void SpawnEnemies(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var index = Random.Range(0, enemyPrefabs.Length);

            var position = GeneratePosition();

            Instantiate(enemyPrefabs[index], position, enemyPrefabs[index].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var aliveEnemies = FindObjectsOfType<Enemy>().Length;

        if (aliveEnemies <= 0)
        {
            if (routineEnumerator != null)
            {
                StopCoroutine(routineEnumerator);
                CleanUpGround();
            }

            if (enemiesToSpawn <= maxEnemyWave)
            {
                SpawnEnemyWave();
                enemiesToSpawn++;
            }
            else
            {
                SpawnBigBoss();
                enemiesToSpawn %= maxEnemyWave;
            }
        }
    }

    private void SpawnEnemyWave()
    {
        GeneratePowerup();

        SpawnEnemies(enemiesToSpawn);
    }

    private void GeneratePowerup()
    {
        var position = GeneratePosition();

        Instantiate(powerupPrefab, position, powerupPrefab.transform.rotation);
    }

    private void SpawnBigBoss()
    {
        CleanUpGround();

        var position = GeneratePosition();
        Instantiate(bossPrefab, position, bossPrefab.transform.rotation);

        routineEnumerator = GeneratePowerupAgainstBoss();
        StartCoroutine(routineEnumerator);
    }

    private void CleanUpGround()
    {
        var powerups = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (var p in powerups)
        {
            Destroy(p);
        }
    }

    private IEnumerator GeneratePowerupAgainstBoss()
    {
        var powerupInterval = 10;

        while (true)
        {
            yield return new WaitForSeconds(powerupInterval);

            GeneratePowerup();
        }
    }

    private Vector3 GeneratePosition()
    {
        var x = Random.Range(-range, range);
        var z = Random.Range(-range, range);

        return new Vector3(x, 0, z);
    }

    private readonly float range = 9;
    private int enemiesToSpawn = 1;
    private IEnumerator routineEnumerator;
}
