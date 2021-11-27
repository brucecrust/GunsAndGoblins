using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // -- Variables --
    public float destroyTime = 0.3f;
    public float bulletSpeed = 20;
    public float damage;
    public Vector3 moveDirection = Vector3.zero;

    private void Start() {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void FixedUpdate() {
        var newPosition = new Vector3(moveDirection.x, moveDirection.y, 0) - transform.position;
        transform.Translate(newPosition * (bulletSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionExit2D(Collision2D other) {
        Destroy(gameObject);
    }
}
