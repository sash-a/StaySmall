using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Use this for initialization
    private float damage;
    
    public AudioClip a_hit;
    private AudioSource audio;
    
    void Start()
    {
        Destroy(gameObject, 5);

        audio = GetComponent<AudioSource>();
    }

    public void fire(Vector3 direction, float speed, float damage)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * speed);
        this.damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.name.Equals("Enemy(Clone)"))
        {
            other.gameObject.GetComponent<EnemyController>().damage(damage);
            audio.PlayOneShot(a_hit);
        }
        else if (other.transform.name.Contains("EnemyCamp"))
        {
            other.gameObject.GetComponent<CampController>().damage(damage);
            audio.PlayOneShot(a_hit);
        }

        Destroy(gameObject);
    }
}