using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// TODO: Boss teleports?

public class NPC : Entity {
    
    // -- Constants --
    private const int ACTOR_LAYER = 9;
    private const int BLOCKING_LAYER = 8;
    
    // -- Variables --
    public float repeatMovementRate = 2;

    protected bool moveRight;
    protected bool moveUp;

    protected virtual void Start() {
        
        health = 100;
        damage = 10;

        InvokeRepeating("CalculateMovement", 0, repeatMovementRate);
    }

    // Update is called once per frame
    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (standStill) return;
        if (IsAlive()) Move();
    }    
    
    protected override void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == BLOCKING_LAYER || other.gameObject.layer == ACTOR_LAYER) {
            CalculateMovement();
        }
        
        base.OnCollisionEnter2D(other);
    }
    
    // -- Utility Methods --
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
    
    protected override void CalculateTime() {
        base.CalculateTime();
        CalculateMovement();
    }
    
    // -- Parent Override Methods --
    protected override void Move() {
        // Horizontal:
        if (moveRight && !moveUp) {
            Rotate();
            rigidbody2D.MovePosition(position + Vector3.right * (speed * Time.fixedDeltaTime));
        } else if (!moveRight && !moveUp) {
            Rotate();
            rigidbody2D.MovePosition(position + Vector3.left * (speed * Time.fixedDeltaTime));
            
            // Vertical:
        } else if (moveUp && !moveRight) {
            rigidbody2D.MovePosition(position + Vector3.up * (speed * Time.fixedDeltaTime));
        } else if (!moveUp && !moveRight) {
            rigidbody2D.MovePosition(position + Vector3.down * (speed * Time.fixedDeltaTime));
        }
    }

    protected void Rotate() {
        transform.localScale = moveRight ? Vector3.one : new Vector3(-1, 1, 1);
    }

    protected override void WasShot() {
        Kill();

        if (!printedBlood) PrintBlood();

        standStill = true;
        moveRight = false;

        CalculateTime();
    }
}
