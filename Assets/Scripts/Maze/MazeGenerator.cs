using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MazeGenerator : MonoBehaviour
{
    public static int layer = 11;

    public GameObject block;

    public int width;
    public int height;
    public float corridorWidth;

    public Cell[,] maze;
    private Random rnd = new Random(Guid.NewGuid().GetHashCode());

    // Use this for initialization
    void Start()
    {
        corridorWidth = block.transform.localScale.x * 4;
        maze = new Cell[width, height];
        genMaze(0, 0);
        displayMaze();
    }

    private GameObject placeBlock(Vector3 pos, bool vert)
    {
        GameObject piece = Instantiate(block, pos, Quaternion.identity);

        if (!vert) return piece;
        var trans = piece.GetComponent<Transform>();
        trans.Rotate(0, 0, 90);
        trans.position -= new Vector3(trans.localScale.x, trans.localScale.x) * 2;

        return piece;
    }


    void genMaze(int cx, int cy)
    {
        var randDirections = Cell.directions.OrderBy(x => rnd.Next()).ToList();

        foreach (var dir in randDirections)
        {
            int nx = cx + Cell.dx[dir], ny = cy + Cell.dy[dir];

            if (0 <= ny && ny < height && 0 <= nx && nx < width && maze[nx, ny] == null)
            {
                if (maze[cx, cy] == null) maze[cx, cy] = new Cell();
                if (maze[nx, ny] == null) maze[nx, ny] = new Cell();

                maze[cx, cy].addDir(dir);
                maze[nx, ny].addDir(Cell.opposite[dir]);
                genMaze(nx, ny);
            }
        }
    }

    void displayMaze()
    {
        //Vector3 initialPos = new Vector3(1, 1, 1);
        //float corridorWidth = 2;
        
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                Cell cell = maze[w, h];
                var pos = new Vector3(w, h, 0);
                if (!cell.n)
                {
                    var g = placeBlock(pos * corridorWidth, false);
                    //g.GetComponent<SpriteRenderer>().color = Color.blue;
                }

                if (!cell.s)
                {
                    var g = placeBlock(pos * corridorWidth + new Vector3(0, -corridorWidth, 0), false);
                    //g.GetComponent<SpriteRenderer>().color = Color.red;
                }

                if (!cell.w)
                {
                    var g = placeBlock(pos * corridorWidth, true);
                    //g.GetComponent<SpriteRenderer>().color = Color.green;
                }

                if (!cell.e)
                {
                    var g = placeBlock(pos * corridorWidth + new Vector3(corridorWidth, 0, 0), true);
                    //g.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }
}