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

    private bool canChangeDirection;
    private Vector2 direction;
    private float accel;
    private float maxSpeed;

    private const int MAX_PATROL_SPEED = 20;
    private const int MAX_ATTACK_SPEED = 100;
    private bool chasing;

    private const int LAYER = 10;
    private int layermask;


    private GameObject player;
    private List<Node> pathToPlayer;

    private Rigidbody2D rb;
    private MazeGenerator maze;


    // Use this for initialization
    void Start()
    {
        maze = FindObjectOfType<MazeGenerator>();
        Physics2D.IgnoreLayerCollision(LAYER, LAYER);
        rb = GetComponent<Rigidbody2D>();

        chasing = false;
        canChangeDirection = true;

        accel = 20;
        health = 100;
        patrol();

        layermask = ~(1 << LAYER);
        //findLongestDirection();
        StartCoroutine(a());
    }

    IEnumerator a()
    {
        yield return new WaitForSeconds(1);
//        pathToPlayer = astar(new Node(worldToGridPoint(player.transform.position)));
//        print(pathToPlayer.First());
        findLongestDirection();
    }

    private void FixedUpdate()
    {
        //if (player != null) return;
        rb.velocity = direction * accel; // enemy kept double colliding with wall
        // Speed limit
        if (rb.velocity.magnitude >= maxSpeed) rb.velocity = rb.velocity.normalized * MAX_PATROL_SPEED;
//        if (!chasing)
//        {
//            rb.velocity = direction * accel; // enemy kept double colliding with wall
//            // Speed limit
//            if (rb.velocity.magnitude >= maxSpeed) rb.velocity = rb.velocity.normalized * MAX_PATROL_SPEED;
//
//            player = checkForPlayer();
//            if (player != null)
//            {
//                pathToPlayer = astar(new Node(worldToGridPoint(player.transform.position)));
//                chasing = true;
//            }
//        }
//        else
//        {
//            // print("going");
//            moveToPlayer(player);
//        }
    }

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

        foreach (var x in pathToPlayer)
        {
            print(x.position);
        }

        if (pathToPlayer.Count == 0)
        {
            player.GetComponent<PlayerMovement>().damage(10);
            Destroy(gameObject);
        }

        transform.position = Vector3.MoveTowards(transform.position, gridToWorldPoint(pathToPlayer.First().position),
            Time.deltaTime * 20);

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
            print("found player");
            return hit.collider.gameObject;
        }

        return null;
    }

    void patrol()
    {
        maxSpeed = MAX_PATROL_SPEED + Random.Range(-5, 5);
    }

//    void chase(Vector3 playerPos)
//    {
//        chasing = true;
//        direction = playerPos - transform.position; // This needs to be A*!!!!
//        direction.Normalize();
//        maxSpeed = MAX_ATTACK_SPEED;
//    }

    public void damage(float amount)
    {
        health -= amount;
        if (health <= 0) Destroy(gameObject);
    }

    Vector3 gridToWorldPoint(Vector3 point)
    {
        return point * maze.corridorWidth - new Vector3(0, maze.corridorWidth / 2);
    }

    Vector2 worldToGridPoint(Vector3 point)
    {
        int x = (int) (point.x / maze.corridorWidth) + 1;
        int y = (int) (point.y / maze.corridorWidth) + 1;
        if (point.x % maze.corridorWidth == 0)
        {
            x--;
        }

        if (point.y % maze.corridorWidth == 0)
        {
            y--;
        }

        return new Vector2(x, y);
    }

    private List<Node> allNodes = new List<Node>();

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
            var node = new Node(new Vector2(n.position.x, n.position.y + 1),
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

        if (position.s)
        {
            var node = new Node(new Vector2(n.position.x, n.position.y - 1),
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

        if (position.w)
        {
            var node = new Node(new Vector2(n.position.x - 1, n.position.y),
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

        if (position.e)
        {
            var node = new Node(new Vector2(n.position.x + 1, n.position.y),
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