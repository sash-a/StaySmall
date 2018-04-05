using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    private float timer;
    public GameObject powerup;

    private int retries;
    private MazeGenerator maze;

    private void Start()
    {
        retries = 0;
        maze = GetComponent<MazeGenerator>();
        // timer = Random.Range(8, 20);
        StartCoroutine(_timeBetweenSpawn());
    }

    public void spawn()
    {
        int xpos = Random.Range(0, maze.width);
        int ypos = Random.Range(0, maze.height) - 1;

        Vector3 pos = new Vector3(xpos, ypos) * maze.corridorWidth +
                      new Vector3(0, maze.corridorWidth / 2);
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
        timer = Random.Range(1, 2);
        yield return new WaitForSeconds(timer);
        spawn();
        StartCoroutine(_timeBetweenSpawn());
    }
}