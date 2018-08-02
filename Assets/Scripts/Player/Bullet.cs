using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        HealthSystem health = hit.GetComponent<HealthSystem>();
        if (health != null)
        {
            health.TakeDamage(10);
        }

        Destroy(gameObject);
    }
}
