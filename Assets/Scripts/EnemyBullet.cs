using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    // -- Variables --
    public float damage;

    // -- Utility Methods --
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.CompareTag("Enemy")) return;
        if (other.transform.CompareTag("Projectile")) return;
        Destroy(gameObject);
    }
}
