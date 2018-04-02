using UnityEngine;

public class EnemyCampSpawner : MonoBehaviour
{
    public int numCamps;

    public GameObject camp;

    // Use this for initialization
    void Start()
    {
        MazeGenerator maze = GetComponent<MazeGenerator>();
        int height = maze.height;
        int width = maze.width;
        float corridorWidth = maze.block.transform.localScale.x * 4;

        for (int i = 0; i < numCamps; i++)
        {
            int xpos = Random.Range(0, width);
            int ypos = Random.Range(0, height) - 1;
            // TODO check if there already is one there
            Instantiate(camp, new Vector3(xpos, ypos) * corridorWidth + new Vector3(0, corridorWidth / 2),
                Quaternion.identity);
        }
    }
}