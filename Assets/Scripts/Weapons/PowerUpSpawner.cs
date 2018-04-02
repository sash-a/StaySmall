using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    private float timer;
    public GameObject powerup;
    private void Start()
    {
        timer = Random.Range(8, 20);
        StartCoroutine(_timeBetweenSpawn());
    }

    void spawn()
    {
        Instantiate(powerup, new Vector3(
                Random.Range(-100, 100),
                Random.Range(-60, 60), 0),
            Quaternion.identity);
    }

    private IEnumerator _timeBetweenSpawn()
    {
        timer = Random.Range(8, 20);
        yield return new WaitForSeconds(timer);
        spawn();
        StartCoroutine(_timeBetweenSpawn());
    }
}