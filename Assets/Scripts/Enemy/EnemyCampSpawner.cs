using System.Collections.Generic;
using UnityEngine;

public class EnemyCampSpawner : MonoBehaviour
{
    public int numCamps;

    public GameObject camp;
    public static HashSet<Vector2> spawnedPositions;

    private int retries;

    // Use this for initialization
    void Start()
    {
        numCamps = (int) LevelManager.difficulty;
        retries = 0;

        MazeGenerator maze = GetComponent<MazeGenerator>();
        spawnedPositions = new HashSet<Vector2>();

        for (int i = 0; i < numCamps; i++)
        {
            spawn(maze);
        }
    }

    void spawn(MazeGenerator maze)
    {
        int xpos = Random.Range(0, maze.width);
        int ypos = Random.Range(0, maze.height) - 1;
        // TODO check if there already is one there
        Vector3 pos = new Vector3(xpos, ypos) * maze.corridorWidth +
                      new Vector3(0, maze.corridorWidth / 2);
        if (!spawnedPositions.Add(pos)) retries++;
        if (retries < 5)
        {
            Instantiate(camp, pos, Quaternion.identity);
            retries = 0;
        }
    }
}