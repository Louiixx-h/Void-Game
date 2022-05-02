using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limbo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;
        damageable.TakeDamage(1000000000);
    }
}
