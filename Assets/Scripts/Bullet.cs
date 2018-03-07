using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Dictionary<string, float> attributes;
    public float angle = 1;
    void Start()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var rotation = Quaternion.LookRotation(transform.position - mousePos, Vector3.forward);
        transform.rotation = rotation;
        // Locking rotation on z axis
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        GetComponent<Rigidbody2D>().angularVelocity = 0;

        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * transform.up;
        GetComponent<Rigidbody2D>().AddForce(dir * attributes[Gun.SPEED]);

        StartCoroutine(_bulletLivingTimer());
    }

    private IEnumerator _bulletLivingTimer()
    {
        yield return new WaitForSeconds(attributes[Gun.BULLET_LIVE_TIME]);
        Destroy(gameObject);
    }
}