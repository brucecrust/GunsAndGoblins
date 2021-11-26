using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    // -- Variables --
    public float health = 100;
    public float damage = 10;
    public float speed = 0.3f;

    // -- Components --
    protected Canvas canvas;
    protected Camera camera;
    protected Rigidbody2D rigidbody2D;

    // -- Life Cycle Methods --
    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // -- Utility Methods --
    protected virtual void Move() {}

    protected virtual bool IsAlive() {
        return health > 0;
    }

    protected virtual void Kill() {
        Destroy(gameObject);
    }
}