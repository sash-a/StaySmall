﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private bool nextWave;

    public float timeBetweenWaves;
    public int maxEnemies;

    //private int numEnemies;

    public GameObject enemy;

    private List<GameObject> enemies;

    // Use this for initialization
    void Start()
    {
        maxEnemies = (int) Math.Ceiling(LevelManager.difficulty / 2);
        timeBetweenWaves = 10 / LevelManager.difficulty;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>());
        enemies = new List<GameObject>();
        StartCoroutine(_waveTimer());
    }

    // Update is called once per frame
    void Update()
    {
        enemies.RemoveAll(e => e == null); // enemy has been destroyed

        if (!nextWave || enemies.Count >= maxEnemies) return;

        float offsetx = Random.Range(-2, 2);
        float offsety = Random.Range(-2, 2);
        enemies.Add(Instantiate(enemy, transform.position + new Vector3(offsetx, offsety),
            Quaternion.identity));


        StartCoroutine(_waveTimer());
    }

    private IEnumerator _waveTimer()
    {
        nextWave = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        nextWave = true;
    }
}