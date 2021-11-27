using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    // -- Variables --;
    public float jump = 0;

    private Vector3 moveDelta;
    private float xMovement;
    private float yMovement;
    
    // -- Components --
    private Animator animator;
    
    public GameObject bulletPrefab;
    public GameObject backGun;
    public GameObject sideGun;
    public GameObject forwardGun;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        base.FixedUpdate();
        
        TrackMovement();
        
        // Assign input
        moveDelta = Vector3.zero;
        moveDelta = new Vector3(xMovement, yMovement, 0);

        // Change Player animation direction
        ModifyAnimation();
        
        // Rotate player
        Rotate();

        // Move player and weapon
        Move();
    }
    
    void Update() {
        TrackMovement();
        var hasShot = Input.GetMouseButtonDown(0);

        if (hasShot) PrintBullet();
    }


    // -- Parent Override Methods --
    protected override void Move() {
        rigidbody2D.MovePosition(transform.position + moveDelta * (speed * Time.fixedDeltaTime));
    }

    // -- Utility Methods --
    private void TrackMovement() {
        // Track input
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");
    }

    private void Rotate() {
        if (moveDelta.x > 0) {
            transform.localScale = Vector3.one;
        } else if (moveDelta.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ModifyAnimation() {
        var animationName = "movingNorth";

        if (moveDelta.y > 0) {
            animator.SetBool(animationName, true);
            sideGun.SetActive(false);
            backGun.SetActive(true);
            forwardGun.SetActive(false);
        } else if (moveDelta.y < 0) {
            animator.SetBool(animationName, false);
            sideGun.SetActive(false);
            backGun.SetActive(false);
            forwardGun.SetActive(true);
        } else {
            animator.SetBool(animationName, false);
            sideGun.SetActive(true);
            backGun.SetActive(false);
            forwardGun.SetActive(false);
        }
    }

    private void PrintBullet() {
        var prefab = Instantiate(bulletPrefab, position, Quaternion.identity);
        var mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        prefab.GetComponent<Bullet>().damage = damage;
        prefab.GetComponent<Bullet>().moveDirection = mousePosition;
    }
}
