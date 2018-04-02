using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    

    void Update()
    {
        /*
         * Insantiate bullets
         */
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
        // TODO getting a powerup
//        if (other.collider.gameObject.name.Contains("Powerup"))
//        {
//            gun = other.collider.GetComponent<PowerUpController>().gun;
//            Destroy(other.collider.gameObject);
//        }
    }

}