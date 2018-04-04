using System.Collections.Generic;
using UnityEngine;

public class EnemyCampSpawner : MonoBehaviour
{
    public int numCamps;

    public GameObject camp;
    public static HashSet<Vector2> spawnedPositions;

    // Use this for initialization
    void Start()
    {
        spawnedPositions = new HashSet<Vector2>();

        MazeGenerator maze = GetComponent<MazeGenerator>();
        int height = maze.height;
        int width = maze.width;
        float corridorWidth = maze.block.transform.localScale.x * 4;

        for (int i = 0; i < numCamps; i++)
        {
            int xpos = Random.Range(0, width);
            int ypos = Random.Range(0, height) - 1;
            // TODO check if there already is one there
            Vector3 pos = new Vector3(xpos, ypos) * corridorWidth + new Vector3(0, corridorWidth / 2);
            if (!spawnedPositions.Add(pos))
            {
                i--;
                continue;
            }

            Instantiate(camp, pos, Quaternion.identity);
        }
    }
}