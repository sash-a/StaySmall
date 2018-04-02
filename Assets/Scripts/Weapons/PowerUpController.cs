using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private const float ROTATION_SPEED = 500;
    public Dictionary<string, float> gun;

    void Start()
    {
        int gunChoice = Random.Range(0, 2);
        if (gunChoice == 0)
        {
            gun = Gun.machinegun;
        }
        else
        {
            gun = Gun.shotgun;
        }
    }

    void Update()
    {
        /**
         * Animate
         */
        transform.Rotate(Vector3.up * Time.deltaTime * ROTATION_SPEED);
    }

    //NOT WORKING?
    /*private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }*/
}