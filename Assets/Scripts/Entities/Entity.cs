using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity : BlurbObject {

    // -- Constants --
    private const int BULLET_LAYER = 12;

    // -- Variables --
    public float bloodVariance = 0.1f;
    public float damage = 10;
    public float deletePrefabTime = 3;
    public float health = 100;
    public float interpolationPeriod = 1;
    public float speed = 0.3f;

    public float initialHealth;
    
    public Vector3 position;

    protected bool canJump = false;
    protected bool printedBlood = false;
    protected bool printedCloud = false;
    protected bool standStill = false;
    protected bool wasShot = false;

    private Vector3 shotPosition = Vector3.zero;
    protected List<GameObject> spawnedBlood = new List<GameObject>();

    private float damageTimer = 0.0f;

    // -- Components --
    protected Rigidbody2D rigidbody2D;
    protected BoxCollider2D boxCollider2D;
    
    public GameObject sprite;
    public GameObject bloodPrefab;
    public GameObject cloudPrefab;

    // -- Life Cycle Methods --
    protected override void Awake() {
        base.Awake();

        initialHealth = health;
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        foreach (Transform child in transform) {
            if (child.GetComponent<SpriteRenderer>() != null) {
                sprite = child.gameObject;
            }
        }
    }

    protected virtual void FixedUpdate() {
        UpdatePosition();

        if (wasShot) WasShot();
        
        Kill();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) {
        // If layer is a bullet
        if (other.gameObject.layer == BULLET_LAYER) {
            wasShot = true;
            shotPosition = other.contacts[0].point;
            health -= other.gameObject.GetComponent<Bullet>().damage;
        }
    }

    // -- Utility Methods --
    protected virtual void Heal() {
        health = initialHealth;
    }
    
    protected virtual void Heal(float amount) {
        health += amount;
    }
    
    protected virtual bool IsAlive() {
        return health > 0;
    }

    protected virtual void CalculateTime() {
        bool interpolatedPeriodReached = damageTimer >= interpolationPeriod;
        
        damageTimer += Time.fixedDeltaTime;

        if (!interpolatedPeriodReached) return;

        damageTimer -= interpolationPeriod;
        
        wasShot = false;
        printedBlood = false;
        standStill = false;
    }

    protected virtual void Jump() { }
    
    protected virtual void Move() { }

    protected virtual void Kill() {
        if (health > 0) return;

        foreach (var blood in spawnedBlood) {
            if (blood != null) Destroy(blood);
        }

        Destroy(boxCollider2D);
        if (!printedCloud) {
            Destroy(PrintCloud(), deletePrefabTime);
            printedCloud = true;
        }
        sprite.SetActive(false);
        Destroy(gameObject, deletePrefabTime);
    }
    
    protected virtual void PrintBlood() {
        var positionVariant = new Vector3(Random.Range(0, bloodVariance), Random.Range(0, bloodVariance), 0);
        var blood = Instantiate(bloodPrefab, shotPosition + positionVariant, Quaternion.identity);

        spawnedBlood.Add(blood);

        blood.transform.parent = transform;
        printedBlood = true;

        Destroy(blood, deletePrefabTime);
    }

    protected virtual GameObject PrintCloud() {
        var prefab = Instantiate(cloudPrefab, position, Quaternion.identity);
        prefab.transform.parent = transform;
        return prefab;
    }

    protected virtual void UpdatePosition() {
        position = transform.position;
    }
    
    protected virtual void WasShot() {
        Kill();

        if (!printedBlood) PrintBlood();

        standStill = true;

        CalculateTime();
    }
}