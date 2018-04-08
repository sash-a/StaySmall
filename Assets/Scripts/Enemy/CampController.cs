using UnityEngine;

public class CampController : MonoBehaviour
{
    public float health;

    public void damage(float damage)
    {
        health -= damage;
        if (health < 0) Destroy(gameObject);
    }
}