using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shoot : MonoBehaviour
{
    // Starting gun
    public Dictionary<string, float> gun = Gun.pistol;
    public ParticleSystem flare;

    private bool canFire = true;


    void Update()
    {
        Debug.DrawRay(transform.position, transform.up * gun[Gun.RANGE], Color.green);

        if (Input.GetMouseButtonDown(0) && canFire) // left mouse
        {
            // TODO: Muzzle flash
            for (int i = 0; i < gun[Gun.NUM_BULLETS]; i++)
            {
                Debug.Log("shot");

                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, gun[Gun.RANGE]);

                if (hit.collider != null)
                {
                    // TODO damage enemy
                    Debug.Log(hit.transform.name);
                }
            }

            StartCoroutine(_timeBetweenFire());
        }

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(flare, transform.position, Quaternion.identity);
        }
    }


    private IEnumerator _timeBetweenFire()
    {
        canFire = false;
        yield return new WaitForSeconds(gun[Gun.FIRE_RATE]);
        canFire = true;
    }
}