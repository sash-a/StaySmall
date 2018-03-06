using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{    
    public float Speed;

    void Start()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var rotation = Quaternion.LookRotation(transform.position - mousePos, Vector3.forward);
        transform.rotation = rotation;
        // Locking rotation on z axis
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        
        GetComponent<Rigidbody2D>().AddForce(Speed * transform.up);
        StartCoroutine(_bulletLivingTimer());
    }

    private IEnumerator _bulletLivingTimer()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}