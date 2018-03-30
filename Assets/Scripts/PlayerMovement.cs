using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public Dictionary<string, float> gun = Gun.shotgun;
    private bool canFire = true;

    void Update()
    {
        /*
         * Insantiate bullets
         */
        if (Input.GetMouseButtonDown(0) && canFire) // left mouse
        {
            GameObject proj;
            if (gun[Gun.NUM_BULLETS] == 3)
            {
                proj = Instantiate(
                    bullet,
                    transform.position +
                    transform.up.normalized * transform.localScale.magnitude * 2, // always spawns infront of player
                    transform.rotation);
                proj.GetComponent<Bullet>().attributes = gun;
                proj.GetComponent<Bullet>().angle = 30;

                proj = Instantiate(
                    bullet,
                    transform.position +
                    transform.up.normalized * transform.localScale.magnitude * 2, // always spawns infront of player
                    transform.rotation);
                proj.GetComponent<Bullet>().attributes = gun;
                proj.GetComponent<Bullet>().angle = -30;
            }

            proj = Instantiate(
                bullet,
                transform.position +
                transform.up.normalized * transform.localScale.magnitude * 2, // always spawns infront of player
                transform.rotation);
            proj.GetComponent<Bullet>().attributes = gun;
            StartCoroutine(_timeBetweenFire());
        }
    }

    void FixedUpdate()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        /*
         * Rotation
         */
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var rotation = Quaternion.LookRotation(transform.position - mousePos, Vector3.forward);
        transform.rotation = rotation;
        // Locking rotation on z axis
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        rigidbody.angularVelocity = 0;
        /*
         * Movement
         */
        var input = Input.GetAxis("Vertical");
        rigidbody.AddForce(gameObject.transform.up * speed * input);
        input = Input.GetAxis("Horizontal");
        rigidbody.AddForce(gameObject.transform.right * speed * input);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.collider.gameObject.name);

        // Bullet collision
        if (other.collider.gameObject.name.Equals("Bullet(Clone)"))
        {
            transform.localScale *= 1.1f; // TODO should this be public?
            Destroy(other.gameObject); //TODO collision pushing player too far back
        }

        // TODO incorrect name
        if (other.collider.gameObject.name.Contains("Powerup"))
        {
            gun = other.collider.GetComponent<PowerUpController>().gun;
            Destroy(other.collider.gameObject);
        }

        if (other.collider.gameObject.name.Equals("Spikes"))
        {
            // TODO: end game
            Destroy(gameObject);
        }
    }

    private IEnumerator _timeBetweenFire()
    {
        canFire = false;
        yield return new WaitForSeconds(gun[Gun.FIRE_RATE]);
        canFire = true;
    }
}