using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    public int level;
    private MazeGenerator maze;

    public LevelManager(MazeGenerator maze)
    {
        level = 1;
        this.maze = maze;
        setMazeSize();
    }

    private void setMazeSize()
    {
        maze.width = level * 4;
        maze.height = level * 4;
    }

    public void nextLevel()
    {
        if (level == 5)
            endGame();
        // you win
        level++;
        setMazeSize();
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.UnloadSceneAsync("GameScreen");
        SceneManager.LoadScene("Menu");
    }

    IEnumerator transitionToNextLevel()
    {
        yield return new WaitForSeconds(5);
        SceneManager.UnloadSceneAsync("GameScreen");
        SceneManager.LoadScene("Menu");
    }

    public void win()
    {
    }

    public void lose()
    {
    }
}