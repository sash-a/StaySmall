using System.Collections;
using UnityEngine;

public class ConsumableSpawner : MonoBehaviour
{
    //public GameObject maze;
    public GameObject gunBox;
    public GameObject gunAmmo;
    public GameObject flareAmmo;

    public GameObject powerup;

    private int retries;
    private float timer;
    private MazeGenerator maze;


    // Use this for initialization
    void Start()
    {
        maze = FindObjectOfType<MazeGenerator>();
        retries = 0;
        
        StartCoroutine(_spawner());
    }

    public void spawnPickup(GameObject obj)
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
            spawnPickup(obj);
        }

        Instantiate(obj, pos, Quaternion.identity);
    }

    public void spawnPowerup()
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
            spawnPowerup();
        }

        Instantiate(powerup, pos, Quaternion.identity);
    }

    public GameObject chooseObject()
    {
        float chance = Random.Range(0, 1);
        if (chance > 0.8)
        {
            return gunBox;
        }

        if (chance > 0.5)
        {
            return flareAmmo;
        }

        return gunAmmo;
    }

    private IEnumerator _spawner()
    {
        float choice = Random.Range(0f, 1f);
        if (choice > 0.8)
        {
            spawnPowerup();
        }
        else
        {
            spawnPickup(chooseObject());
        }

        yield return new WaitForSeconds(Random.Range(1, 2));

        StartCoroutine(_spawner());
    }
}