using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Use this for initialization
    private float damage;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    public void fire(Vector3 direction, float speed, float damage)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * speed);
        this.damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.name.Equals("Enemy(Clone)"))
        {
            print(other.gameObject.GetComponent<EnemyController>());
            other.gameObject.GetComponent<EnemyController>().damage(damage);
        }

        Destroy(gameObject);
    }
}