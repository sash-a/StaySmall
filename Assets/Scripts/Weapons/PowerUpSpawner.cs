using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    private float timer;
    public GameObject powerup;

    private MazeGenerator maze;
    private float corridorWidth;
    private int retries;
    private void Start()
    {
        retries = 0;
        maze = GetComponent<MazeGenerator>();
        corridorWidth = maze.block.transform.localScale.x * 4;

        timer = Random.Range(8, 20);
        StartCoroutine(_timeBetweenSpawn());
    }

    void spawn()
    {
        print("hi");
        int xpos = Random.Range(0, maze.width);
        int ypos = Random.Range(0, maze.height);

        Vector3 pos = new Vector3(xpos, ypos) * corridorWidth + new Vector3(0, corridorWidth / 2);
        if (!EnemyCampSpawner.spawnedPositions.Add(pos))
        {
            // So that it doesn't keep retrying
            retries++;
            if (retries >= 5) return;

            retries = 0;
            spawn();
        }
        Instantiate(powerup, pos, Quaternion.identity);
    }

    private IEnumerator _timeBetweenSpawn()
    {
        timer = Random.Range(8, 20);
        yield return new WaitForSeconds(timer);
        spawn();
        StartCoroutine(_timeBetweenSpawn());
    }
}