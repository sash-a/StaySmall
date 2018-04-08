using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public int playerNum;
    public KeyValuePair<string, float> currentPowerup;
    public float remainingPowerupTime;
    public float health;

    private string controlPrefix;
    private Rigidbody2D rb;

    public Text text;
    private TextDisplayer textDisplayer;

    void Start()
    {
        health = 100;
        controlPrefix = "p" + playerNum + "_";
        rb = GetComponent<Rigidbody2D>();

        textDisplayer = new TextDisplayer(text);

        spawnPlayers();
    }

    void FixedUpdate()
    {
        remainingPowerupTime -= Time.fixedDeltaTime;

        onInput();
    }

    void onInput()
    {
        /*
         * Rotation
         */
//        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        var rotation = Quaternion.LookRotation(transform.position - mousePos, Vector3.forward);
//        transform.rotation = rotation;
//        // Locking rotation on z axis
//        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
//        rigidbody.angularVelocity = 0;

        var input = Input.GetAxis(controlPrefix + "Horizontal");
        rb.MoveRotation(rb.rotation - 90 * input * Time.fixedDeltaTime * 4);

        /*
         * Movement
         */
        input = Input.GetAxis(controlPrefix + "Vertical");
        rb.AddForce(gameObject.transform.up * speed * input);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("PowerUp") &&
            currentPowerup.Equals(new KeyValuePair<string, float>())) // basically if null
        {
            currentPowerup = other.gameObject.GetComponent<PowerupController>().chosenPowerup;

            StartCoroutine(textDisplayer.ShowMessage("Powerup: " + currentPowerup.Key, currentPowerup.Value));

            remainingPowerupTime = currentPowerup.Value;
            other.gameObject.GetComponent<PowerupController>().powerUp(gameObject);

            // Can't be destroyed until powerup is over
            // Make powerup invisible and uncollidable and set the destroy timer to 11 (Longest powerup time is 10)
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(other.gameObject, 11);
        }
        else if (other.gameObject.name.Contains("GunBox"))
        {
            StartCoroutine(textDisplayer.ShowMessage("You found a new gun!", 2));

            int gunChoice = Random.Range(0, 2);
            if (gunChoice == 0)
            {
                GetComponentInChildren<Shoot>().gun = Gun.machinegun;
            }
            else
            {
                GetComponentInChildren<Shoot>().gun = Gun.shotgun;
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("GunAmmo"))
        {
            if (GetComponentInChildren<Shoot>().gunAmmo == -1) return;

            StartCoroutine(textDisplayer.ShowMessage("You found ammo!", 2));
            
            GetComponentInChildren<Shoot>().gunAmmo += 10;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("FlareAmmo"))
        {
            if (GetComponentInChildren<Shoot>().flareAmmo == -1) return;

            StartCoroutine(textDisplayer.ShowMessage("You found flare Ammo!", 2));

            GetComponentInChildren<Shoot>().flareAmmo += 1;
            Destroy(other.gameObject);
        }

        if (other.gameObject.name.Contains("Player"))
        {
            StartCoroutine(textDisplayer.ShowMessage("You win!", 5));
            StartCoroutine(endGame());
        }
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.UnloadSceneAsync("GameScreen");
        SceneManager.LoadScene("Menu");
    }

    public void damage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            StartCoroutine(textDisplayer.ShowMessage("You died!", 5));
            StartCoroutine(endGame());
        }
    }

    void spawnPlayers()
    {
        var maze = FindObjectOfType<MazeGenerator>();

        if (playerNum == 1)
        {
            transform.position =
                new Vector3(
                    Random.Range(0, maze.width / 2),
                    Random.Range(0, (maze.height) / 2),
                    1
                ) * maze.corridorWidth + new Vector3(0, -maze.corridorWidth / 2);
        }
        else
        {
            transform.position =
                new Vector3(
                    Random.Range(maze.width / 2, maze.width),
                    Random.Range((maze.height - 1) / 2, maze.height),
                    1
                ) * maze.corridorWidth + new Vector3(0, -maze.corridorWidth / 2);
        }
    }
}