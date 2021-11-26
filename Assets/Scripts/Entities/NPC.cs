using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// TODO: Boss teleports?

public class NPC : Entity {
    
    // -- Constants --
    private const int BULLET_LAYER = 12;
    private const int BLOCKING_LAYER = 8;
    private const int ACTOR_LAYER = 9;
    
    // -- Variables --
    public float repeatMovementRate = 5;
    public float interpolationPeriod = 1;
    public float deletePrefabTime = 3;
    public float bloodVariance = 0.1f;

    private float time = 0.0f;

    protected bool moveRight;
    protected bool moveUp;
    protected bool standStill = false;
    protected bool wasShot = false;
    protected bool printedBlood = false;
    
    private Vector3 shotPosition = Vector3.zero;

    // -- Components --
    public GameObject bloodPrefab;
    public GameObject cloudPrefab;

    private GameObject sprite;

    protected virtual void Start() {
        health = 100;
        damage = 10;

        InvokeRepeating("CalculateMovement", 0, repeatMovementRate);

        foreach (Transform child in transform) {
            if (child.GetComponent<SpriteRenderer>() != null) {
                sprite = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate() {
        if (wasShot) {
            WasShot();
        }
        
        if (standStill) return;
        
        Move();
        Kill();
    }
    
    // -- Public Methods --
    void OnCollisionEnter2D(Collision2D other) {
        // If layer is a bullet
        if (other.gameObject.layer == BULLET_LAYER) {
            wasShot = true;
            shotPosition = other.contacts[0].point;
            health -= other.gameObject.GetComponent<Bullet>().damage;

        // if layer is blocking or an actor
        } else if (other.gameObject.layer == BLOCKING_LAYER || other.gameObject.layer == ACTOR_LAYER) {
            CalculateMovement();
        }
    }
    
    // -- Parent Override Methods --
    protected override void Move() {
        // Horizontal:
        if (moveRight && !moveUp) {
            Rotate();
            rigidbody2D.MovePosition(transform.position + Vector3.right * (speed * Time.fixedDeltaTime));
        } else if (!moveRight && !moveUp) {
            Rotate();
            rigidbody2D.MovePosition(transform.position + Vector3.left * (speed * Time.fixedDeltaTime));
            
            // Vertical:
        } else if (moveUp && !moveRight) {
            rigidbody2D.MovePosition(transform.position + Vector3.up * (speed * Time.fixedDeltaTime));
        } else if (!moveUp && !moveRight) {
            rigidbody2D.MovePosition(transform.position + Vector3.down * (speed * Time.fixedDeltaTime));
        }
    }
    
    // -- Utility Methods --
    protected void Rotate() {
        transform.localScale = moveRight ? Vector3.one : new Vector3(-1, 1, 1);
    }

    protected void CalculateMovement() {
        // If 0, move right; if 1, move left; if 2, move up; if 3 move down; if 4, do nothing
        var movementDirection = Random.Range(0, 4);
        switch (movementDirection) {
            case 0: 
                moveRight = true;
                moveUp = false;
                break;
            case 1: moveRight = true;
                moveRight = false;
                moveUp = false;
                break;
            case 2: 
                moveUp = true;
                moveRight = false;
                break;
            case 3: 
                moveUp = false;
                moveRight = false;
                break;
            default:
                standStill = true;
                moveRight = false;
                break;
        }
    }

    protected void CalculateTime() {
        time += Time.fixedDeltaTime;

        if (!(time >= interpolationPeriod)) return;
        
        time = time - interpolationPeriod;
        wasShot = false;
        printedBlood = false;
        standStill = false;
        CalculateMovement();
    }

    protected virtual void WasShot() {
        Kill();

        if (!printedBlood) PrintBlood();

        standStill = true;
        moveRight = false;
            
        // Initiate timer; 
        time += Time.fixedDeltaTime;
            
        CalculateTime();
    }

    protected virtual void PrintBlood() {
        var positionVariant = new Vector3(Random.Range(0, bloodVariance), Random.Range(0, bloodVariance), 0);
        var blood = Instantiate(bloodPrefab, shotPosition + positionVariant, Quaternion.identity);

        blood.transform.parent = transform;
        printedBlood = true;
        
        Destroy(blood, deletePrefabTime);
    }

    protected virtual GameObject PrintCloud() {
        var prefab = Instantiate(cloudPrefab, transform.position, Quaternion.identity);
        prefab.transform.parent = transform;
        return prefab;
    }

    protected override void Kill() {
        if (health > 0) return;
        
        Destroy(PrintCloud(), deletePrefabTime);
        sprite.SetActive(false);
        Destroy(gameObject, deletePrefabTime);
    }
}
