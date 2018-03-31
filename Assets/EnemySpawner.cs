using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private bool nextWave;

    public float timeBetweenWaves;
    public int maxEnemies;
    public int minEnemies;
    public GameObject enemy;

    // Use this for initialization
    void Start()
    {
        nextWave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!nextWave) return;

        int numEnemies = Random.Range(minEnemies, maxEnemies + 1);
        for (int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
        }

        StartCoroutine(_waveTimer());
    }

    private IEnumerator _waveTimer()
    {
        nextWave = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        nextWave = true;
    }
}