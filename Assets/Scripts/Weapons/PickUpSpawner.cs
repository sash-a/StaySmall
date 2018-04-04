using System.Collections;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    //public GameObject maze;
    public GameObject gunBox;
    public GameObject gunAmmo;
    public GameObject flareAmmo;

    private int retries;
    private float timer;
    private MazeGenerator maze;

    // Use this for initialization
    void Start()
    {
        maze = FindObjectOfType<MazeGenerator>();
        retries = 0;
        StartCoroutine(_timeBetweenSpawn());
    }

    void spawn(GameObject obj)
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
            spawn(obj);
        }

        Instantiate(obj, pos, Quaternion.identity);
    }

    GameObject chooseObject()
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

    private IEnumerator _timeBetweenSpawn()
    {
        timer = Random.Range(1, 2);
        yield return new WaitForSeconds(timer);
        spawn(chooseObject());
        StartCoroutine(_timeBetweenSpawn());
    }
}