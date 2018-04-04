using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    private float health;

    private Vector2 direction;
    private float accel;
    private float maxSpeed;

    private const int MAX_PATROL_SPEED = 20;
    private const int MAX_ATTACK_SPEED = 100;
    private bool chasing;

    private const int LAYER = 10;
    private int layermask = ~(1 << LAYER);

    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        accel = 20;
        maxSpeed = MAX_PATROL_SPEED;
        health = 100;

        Physics2D.IgnoreLayerCollision(LAYER, LAYER);
        rb = GetComponent<Rigidbody2D>();
        chasing = false;
        findLongestDirection();
        patrol();
    }

    private void FixedUpdate()
    {
        // Speed limit
        rb.AddForce(direction * accel);

        if (rb.velocity.magnitude >= maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * MAX_PATROL_SPEED;
        }

        if (!chasing)
            checkForPlayer();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // TODO if hit maze piece pick random dir
        if (other.transform.name.Contains("MazePiece"))
        {
            findLongestDirection(); // TODO: randomize between the two longest directions
        }
    }

    // find the longest direction using raycasts
    void findLongestDirection()
    {
        RaycastHit2D up = Physics2D.Raycast(transform.position, transform.up, 100, layermask);
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

    bool checkForPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, transform.localScale * 4,
            0,
            direction, 100, layermask);

        if (hit.collider != null && hit.collider.name.Contains("Player"))
        {
            chase(hit.transform.position);
            return true;
        }

        patrol();
        return true;
    }

    void patrol()
    {
        chasing = false;
        // TODO randomise speed
        maxSpeed = MAX_PATROL_SPEED + Random.Range(-5, 5);
    }

    void chase(Vector3 playerPos)
    {
        chasing = true;
        direction = playerPos - transform.position; // This needs to be A*!!!!
        direction.Normalize();
        maxSpeed = MAX_ATTACK_SPEED;
    }

    public void damage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}