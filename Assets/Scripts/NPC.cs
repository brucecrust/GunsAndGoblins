using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// TODO: Boss teleports?

public class NPC : Entity {
    
    // -- Variables --
    public float repeatMovementRate = 5;
    public float interpolationPeriod = 1;
    public float deletePrefabTime = 3;
    public float deleteBlurbTime = 2;
    public float bloodVariance = 0.1f;
    public float defaultBlurbOffset = 0.15f;

    private float time = 0.0f;

    private bool moveRight;
    private bool moveUp;
    private bool standStill = false;
    private bool wasShot = false;
    private bool printedBlood = false;
    
    private Vector3 shotPosition = Vector3.zero;
    
    // -- Components --
    private Image activeBlurb;
    
    public GameObject bloodPrefab;
    public GameObject cloudPrefab;
    public GameObject blurbParent;
    public Image blurbOuch;

    private void Start() {
        health = 100;
        damage = 10;
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        InvokeRepeating("CalculateMovement", 0, repeatMovementRate);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (activeBlurb != null) SetBlurbPosition();
        
        if (wasShot) {
            if (!printedBlood) PrintBlood();

            standStill = true;
            moveRight = false;
            
            // Initiate timer; 
            time += Time.fixedDeltaTime;
            
            CalculateTime();
        }
        
        if (standStill) return;
        
        Move();
        Kill();
    }
    
    // -- Public Methods --
    void OnCollisionEnter2D(Collision2D other) {
        // If layer is a bullet
        if (other.gameObject.layer == 12) {
            wasShot = true;
            shotPosition = other.contacts[0].point;
            health -= damage;

        // if layer is blocking or an actor
        } else if (other.gameObject.layer == 8 || other.gameObject.layer == 9) {
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
    private void Rotate() {
        transform.localScale = moveRight ? Vector3.one : new Vector3(-1, 1, 1);
    }

    private void CalculateMovement() {
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

    private void CalculateTime() {
        if (!(time >= interpolationPeriod)) return;
        
        time = time - interpolationPeriod;
        wasShot = false;
        printedBlood = false;
        standStill = false;
        CalculateMovement();
    }

    private void PrintBlood() {
        var positionVariant = new Vector3(Random.Range(0, bloodVariance), Random.Range(0, bloodVariance), 0);
        var blood = Instantiate(bloodPrefab, shotPosition + positionVariant, Quaternion.identity);
        
        PrintOuchBlurb();
        
        blood.transform.parent = transform;
        printedBlood = true;
        
        Destroy(blood, deletePrefabTime);
    }

    private void PrintOuchBlurb() {
        if (activeBlurb != null) return;
        
        activeBlurb = Instantiate(blurbOuch, blurbParent.transform.position, Quaternion.identity);
        activeBlurb.transform.SetParent(canvas.transform, false);
        
        SetBlurbPosition();

        foreach (Transform child in activeBlurb.transform) {
            // Delete children first
            Destroy(child.gameObject, deleteBlurbTime - 0.1f);
        }
        Destroy(activeBlurb, deleteBlurbTime);
    }

    private void SetBlurbPosition() {
        Vector2 viewportPoint = camera.WorldToViewportPoint(blurbParent.transform.position);
        activeBlurb.GetComponent<RectTransform>().anchorMin = viewportPoint;  
        activeBlurb.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }

    private void Kill() {
        if (health > 0) return;
        
        Destroy(Instantiate(cloudPrefab, transform.position, Quaternion.identity), deletePrefabTime);
        Destroy(gameObject);
    }
}
