using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private const int PATROL_SPEED = 100;
    private const int ATTACK_SPEED = 200;

    private const int LAYER = 10;
    private int layermask = ~(1 << LAYER);

    private Rigidbody2D rb;

    private bool chasing;

    // Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LAYER, LAYER);
        rb = GetComponent<Rigidbody2D>();
        chasing = false;
        findLongestDirection();
        patrol();
    }

    private void FixedUpdate()
    {
        rb.AddForce(direction * speed);
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
        speed = PATROL_SPEED;
    }

    void chase(Vector3 playerPos)
    {
        chasing = true;
        direction = playerPos - transform.position; // This needs to be A*!!!!
        direction.Normalize();
        speed = ATTACK_SPEED;
    }

//    private void OnDrawGizmos()
//    {
//        //Draw a Ray forward from GameObject toward the maximum distance
//        Gizmos.DrawRay(transform.position, transform.up * 100);
//        //Draw a cube at the maximum distance
//        Gizmos.DrawWireCube(transform.position + transform.up * 100, transform.localScale * 4);
//    }
}