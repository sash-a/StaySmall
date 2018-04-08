using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Dictionary<string, float> gun;
    public ParticleSystem flare;
    public GameObject bullet;
    public int flareAmmo;
    public int gunAmmo;

    private bool canFire = true;
    private string controlPrefix;
    
    public AudioClip a_shot;
    private AudioSource audio;
    
    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponentInParent<Collider2D>(), bullet.GetComponent<Collider2D>());
        flareAmmo = 3;
        gunAmmo = 30;

        // Starting gun
        gun = Gun.pistol;

        controlPrefix = "p" + GetComponentInParent<PlayerMovement>().playerNum + "_";

        audio = GetComponentInParent<AudioSource>();
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.up * gun[Gun.RANGE], Color.green);
        List<Vector3> rotations =
            new List<Vector3> {new Vector3(0, 0, 0), new Vector3(0.5f, 0, 0), -new Vector3(0.5f, 0, 0)};
        if (Input.GetButton(controlPrefix + "Fire1") && canFire && gunAmmo > 0) // left mouse
        {
            audio.PlayOneShot(a_shot);
            // TODO: Muzzle flash
            for (int i = 0; i < Mathf.Min(gun[Gun.NUM_BULLETS], gunAmmo); i++)
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                // TODO spread does not work when at 90 degrees
                b.GetComponent<BulletController>()
                    .fire((transform.up + rotations[i]).normalized,
                        gun[Gun.BULLET_SPEED] * Gun.fireRateMod,
                        gun[Gun.DAMAGE] * Gun.damageMod);

                if (gunAmmo != -1) gunAmmo--; // infinte ammo
            }

            StartCoroutine(_timeBetweenFire());
        }

        if (Input.GetButtonDown(controlPrefix + "Fire2") && flareAmmo > 0)
        {
            Instantiate(flare, transform.position, Quaternion.identity);

            if (flareAmmo != -1) flareAmmo--; // infinite ammo
        }
    }


    private IEnumerator _timeBetweenFire()
    {
        canFire = false;
        yield return new WaitForSeconds(gun[Gun.FIRE_RATE]);
        canFire = true;
    }
}