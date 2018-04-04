using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupController : MonoBehaviour
{
    // Use this for initialization
    public KeyValuePair<string, float> chosenPowerup;

    void Start()
    {
        chosenPowerup = Powerup.powerUps.ElementAt(Random.Range(0, Powerup.powerUps.Count));
    }

    public void powerUp(GameObject player)
    {
        if (chosenPowerup.Key.Equals(Powerup.DOUBLE_DAMAGE))
        {
            StartCoroutine(_gunDamage(player));
        }
        else if (chosenPowerup.Key.Equals(Powerup.DOUBLE_FIRE_RATE))
        {
            StartCoroutine(_fireRate(player));
        }
        else if (chosenPowerup.Key.Equals(Powerup.INFINITE_AMMO))
        {
            StartCoroutine(_infiniteAmmo(player));
        }
        else if (chosenPowerup.Key.Equals(Powerup.WALLFAZE))
        {
            StartCoroutine(_wallFaze(player));
        }
        else if (chosenPowerup.Key.Equals(Powerup.ZOOM_OUT))
        {
            StartCoroutine(_zoomOut(player));
        }
    }

    private IEnumerator _gunDamage(GameObject player)
    {
        Gun.damageMod *= 2;
        yield return new WaitForSeconds(chosenPowerup.Value);
        Gun.damageMod /= 2;

        player.GetComponent<PlayerMovement>().currentPowerup = new KeyValuePair<string, float>();
    }

    private IEnumerator _fireRate(GameObject player)
    {
        Gun.fireRateMod *= 2;
        yield return new WaitForSeconds(chosenPowerup.Value);
        Gun.fireRateMod /= 2;

        player.GetComponent<PlayerMovement>().currentPowerup = new KeyValuePair<string, float>();
    }

    private IEnumerator _infiniteAmmo(GameObject player)
    {
        Shoot ammoHolder = player.GetComponentInChildren<Shoot>();
        int oldGunAmmo = ammoHolder.gunAmmo;
        int oldFlareAmmo = ammoHolder.flareAmmo;

        ammoHolder.gunAmmo = -1;
        ammoHolder.flareAmmo = -1;

        yield return new WaitForSeconds(chosenPowerup.Value);

        ammoHolder.gunAmmo = oldGunAmmo;
        ammoHolder.flareAmmo = oldFlareAmmo;

        player.GetComponent<PlayerMovement>().currentPowerup = new KeyValuePair<string, float>();
    }

    private IEnumerator _wallFaze(GameObject player)
    {
        Physics2D.IgnoreLayerCollision(player.layer, MazeGenerator.layer);
        print("starting");
        yield return new WaitForSeconds(2);
        print("done!");
        Physics2D.IgnoreLayerCollision(player.layer, MazeGenerator.layer, false);

        player.GetComponent<PlayerMovement>().currentPowerup = new KeyValuePair<string, float>();
    }

    private IEnumerator _zoomOut(GameObject player)
    {
        Camera.main.fieldOfView += 10;
        yield return new WaitForSeconds(chosenPowerup.Value);
        Camera.main.fieldOfView -= 10;

        player.GetComponent<PlayerMovement>().currentPowerup = new KeyValuePair<string, float>();
    }
}