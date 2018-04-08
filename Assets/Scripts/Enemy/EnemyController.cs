using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    private float health;

    private bool chasing;
    private bool canChangeDirection;
    
    private Vector2 direction;
    private float accel;
    private float maxSpeed;

    private const int MAX_PATROL_SPEED = 20;
    private const int MAX_ATTACK_SPEED = 100;

    private const int LAYER = 10;
    private int layermask;

    private GameObject player;
    private List<Node> pathToPlayer;
    private List<Node> allNodes = new List<Node>();

    private Rigidbody2D rb;
    private MazeGenerator maze;

    void Start()
    {
        maze = FindObjectOfType<MazeGenerator>();
        Physics2D.IgnoreLayerCollision(LAYER, LAYER);
        rb = GetComponent<Rigidbody2D>();

        chasing = false;
        canChangeDirection = true;

        accel = 20;
        health = 50;
        patrol();

        layermask = ~(1 << LAYER);
        findLongestDirection();
    }

    private void FixedUpdate()
    {
        // Speed limit
        if (rb.velocity.magnitude >= maxSpeed) rb.velocity = rb.velocity.normalized * MAX_PATROL_SPEED;
        if (!chasing)
        {
            rb.velocity = direction * accel;
            // Speed limit
            if (rb.velocity.magnitude >= maxSpeed) rb.velocity = rb.velocity.normalized * MAX_PATROL_SPEED;

            player = checkForPlayer();
            if (player != null)
            {
                pathToPlayer = astar(new Node(worldToGridPoint(player.transform.position)));
                chasing = true;
            }
        }
        else
        {
            moveToPlayer(player);
        }
    }

    // Just here to fix the case when the enemy double collides with the wall
    private IEnumerator changeDirTimer()
    {
        canChangeDirection = false;
        yield return new WaitForSeconds(0.5f);
        canChangeDirection = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.name.Contains("MazePiece"))
        {
            if (canChangeDirection)
            {
                direction = -direction;
                StartCoroutine(changeDirTimer());
            }
        }

        if (other.transform.name.Contains("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().damage(10);
            Destroy(gameObject);
        }
    }

    // find the longest direction using raycasts
    void findLongestDirection()
    {
        RaycastHit2D up = Physics2D.Raycast(transform.position, transform.up, 1001, layermask);
        RaycastHit2D right = Physics2D.Raycast(transform.position, transform.right, 100, layermask);
        RaycastHit2D down = Physics2D.Raycast(transform.position, -transform.up, 100, layermask);
        RaycastHit2D left = Physics2D.Raycast(transform.position, -transform.right, 100, layermask);

        float maxDist = -1;
        List<RaycastHit2D> hits = new List<RaycastHit2D> {up, right, down, left};
        // finding the max distance
        foreach (var hit in hits)
        {
            if (Math.Abs(hit.distance) > maxDist)
            {
                maxDist = Math.Abs(hit.distance);
                direction = -hit.normal;
            }
        }
    }

    void moveToPlayer(GameObject player)
    {
        if (pathToPlayer == null)
        {
            return;
        }

        if (pathToPlayer.Count == 0)
        {
            // In the block player WAS in should call A* till definitely in the block player currently is in...but this
            // will work 90% of the time
            chase(player.transform.position);
            return;
        }

        // Move throught the path and delete from the list when at the correct pos
        transform.position = Vector3.MoveTowards(transform.position, gridToWorldPoint(pathToPlayer.First().position),
            Time.deltaTime * 30);

        if (transform.position.Equals(gridToWorldPoint(pathToPlayer.First().position)))
        {
            pathToPlayer.Remove(pathToPlayer.First());
        }
    }

    GameObject checkForPlayer()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(
                GetComponent<BoxCollider2D>().bounds.center,
                transform.localScale * 4,
                0,
                direction,
                100,
                layermask);

        if (hit.collider != null && hit.collider.name.Contains("Player"))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    void patrol()
    {
        maxSpeed = MAX_PATROL_SPEED + Random.Range(-5, 5);
    }

    void chase(Vector3 playerPos)
    {
        chasing = true;
        direction = playerPos - transform.position;
        direction.Normalize();
        maxSpeed = MAX_ATTACK_SPEED;

        rb.velocity = direction * accel;
        // Speed limit
        if (rb.velocity.magnitude >= maxSpeed) rb.velocity = rb.velocity.normalized * MAX_PATROL_SPEED;
    }

    public void damage(float amount)
    {
        health -= amount;
        if (health <= 0) Destroy(gameObject);
    }

    Vector3 gridToWorldPoint(Vector3 point)
    {
        float x = point.x * maze.corridorWidth;
        float y = point.y * maze.corridorWidth - maze.corridorWidth / 2;
        return new Vector3(x, y);
    }

    Vector2 worldToGridPoint(Vector3 point)
    {
        int x = (int) Math.Floor((point.x + maze.corridorWidth / 2) / maze.corridorWidth);
        int y = (int) Math.Floor((point.y + maze.corridorWidth) / maze.corridorWidth);
        return new Vector2(x, y);
    }

    void addNeighbour(Node n, Node target, Vector3 shift)
    {
        var node = new Node(new Vector3(n.position.x, n.position.y) + shift,
            n.costFromOrigin + 1,
            target);

        foreach (var otherNode in allNodes)
        {
            if (node.Equals(otherNode))
            {
                node = otherNode;
            }
        }

        allNodes.Add(node);
        n.neighbours.Add(node);
    }


    void findNeighbours(Node n, Node target)
    {
        Cell position = maze.maze[(int) n.position.x, (int) n.position.y];
        if (position.n)
        {
            addNeighbour(n, target, new Vector3(0, 1));
        }

        if (position.s)
        {
            addNeighbour(n, target, new Vector3(0, -1));
        }

        if (position.w)
        {
            addNeighbour(n, target, new Vector3(-1, 0));
        }

        if (position.e)
        {
            addNeighbour(n, target, new Vector3(1, 0));
        }
    }

    public List<Node> astar(Node target)
    {
        Node start = new Node(worldToGridPoint(transform.position), 0, target);

        findNeighbours(start, target);

        start.path.Add(start);


        List<Node> nextNodes = new List<Node>();
        List<Node> visited = new List<Node>();
        nextNodes.Add(start);

        while (nextNodes.Count != 0)
        {
            nextNodes.Sort();
            var current = nextNodes.Last();
            visited.Add(current);
            nextNodes.Remove(current);

            if (current.Equals(target))
            {
                return current.path;
            }

            findNeighbours(current, target);

            foreach (var neighbour in current.neighbours)
            {
                if (!visited.Contains(neighbour) && !nextNodes.Contains(neighbour))
                {
                    nextNodes.Add(neighbour);
                    neighbour.path.AddRange(current.path);
                    neighbour.path.Add(neighbour);
                }
            }
        }

        return null;
    }
}