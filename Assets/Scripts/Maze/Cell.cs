/*
 * Just a container for north south east and west directions
 */

using System.Collections.Generic;

public class Cell
{
    public bool n;
    public bool s;
    public bool w;
    public bool e;

    private static string north = "n", south = "s", west = "w", east = "e";
    public static List<string> directions = new List<string> {north, south, west, east};
    public List<bool> paths;


    public static Dictionary<string, int> dx = new Dictionary<string, int>
    {
        {north, 0},
        {south, 0},
        {west, -1},
        {east, 1}
    };

    public static Dictionary<string, int> dy = new Dictionary<string, int>
    {
        {north, 1},
        {south, -1},
        {west, 0},
        {east, 0}
    };

    public static Dictionary<string, string> opposite = new Dictionary<string, string>
    {
        {north, south},
        {south, north},
        {west, east},
        {east, west}
    };

    public Cell()
    {
        n = false;
        s = false;
        w = false;
        e = false;

        paths = new List<bool> {n, s, w, e};
    }

    public void addDir(string dir)
    {
        if (dir.ToLower().Equals("n"))
        {
            n = true;
        }
        else if (dir.ToLower().Equals("s"))
        {
            s = true;
        }
        else if (dir.ToLower().Equals("w"))
        {
            w = true;
        }
        else if (dir.ToLower().Equals("e"))
        {
            e = true;
        }
    }
}