using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shoot : MonoBehaviour
{
    // Starting gun
    public Dictionary<string, float> gun = Gun.shotgun;
    public ParticleSystem flare;
    public GameObject bullet;

    private bool canFire = true;

    private int flareAmmo;
    private int gunAmmo;

    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
        flareAmmo = 3;
        gunAmmo = 30;
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.up * gun[Gun.RANGE], Color.green);
        List<Vector3> rotations =
            new List<Vector3> {new Vector3(0, 0, 0), new Vector3(0.5f, 0, 0), -new Vector3(0.5f, 0, 0)};
        if (Input.GetMouseButtonDown(0) && canFire) // left mouse
        {
            // TODO: Muzzle flash
            for (int i = 0; i < gun[Gun.NUM_BULLETS]; i++)
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                // TODO spread does not work when at 90 degrees
                b.GetComponent<BulletController>()
                    .fire((transform.up + rotations[i]).normalized, gun[Gun.BULLET_SPEED], gun[Gun.DAMAGE]);
                gunAmmo--;
            }

            StartCoroutine(_timeBetweenFire());
        }

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(flare, transform.position, Quaternion.identity);
            flareAmmo--;
        }
    }


    private IEnumerator _timeBetweenFire()
    {
        canFire = false;
        yield return new WaitForSeconds(gun[Gun.FIRE_RATE]);
        canFire = true;
    }
}