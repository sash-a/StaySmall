using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public GameObject Bullet;

    void Update()
    {
        /*
         * Insantiate bullets
         */
        if (Input.GetMouseButtonDown(0)) // left mouse
            Instantiate(
                Bullet,
                transform.position + transform.up.normalized * transform.localScale.magnitude * 2, // always spawns infront of player
                transform.rotation);
    }

    void FixedUpdate()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        /*
         * Rotation
         */
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rotation = Quaternion.LookRotation(transform.position - mousePos, Vector3.forward);
        transform.rotation = rotation;
        // Locking rotation on z axis
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        rigidbody.angularVelocity = 0;
        /*
         * Movement
         */
        float input = Input.GetAxis("Vertical");
        rigidbody.AddForce(gameObject.transform.up * Speed * input);
        input = Input.GetAxis("Horizontal");
        rigidbody.AddForce(gameObject.transform.right * Speed * input);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Bullet collision
        if (other.collider.gameObject.name.Equals("Bullet(Clone)"))
        {
            transform.localScale *= 1.1f; // TODO should this be public?
            Destroy(other.gameObject); //TODO collision pushing player too far back
        }
    }
}