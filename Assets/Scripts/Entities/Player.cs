using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {

    // -- Constants --
    private const int ENEMY_BULLET_LAYER = 15;
    
    // -- Variables --;
    public float jump = 0;

    private Vector3 moveDelta;
    private float xMovement;
    private float yMovement;
    
    // -- Components --
    private Animator animator;
    
    public GameObject activeGun;
    public GameObject backGun;
    public GameObject bulletPrefab;
    public GameObject forwardGun;
    public Slider healthBar;
    public GameObject sideGun;


    // Start is called before the first frame update
    protected override void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        foreach (Transform child in transform) {
            if (child.GetComponent<Animator>() != null) {
                animator = child.GetComponent<Animator>();
            }
        }

        SetHealthBar();
        UpdateHealthBar();
    }

    // Update is called once per frame
    protected override void FixedUpdate() {
        UpdatePosition();

        if (wasShot) WasShot();
        
        TrackMovement();
        
        // Assign input
        AssignInput();

        // Change Player animation direction
        ModifyAnimation();
        
        // Rotate player
        Rotate();

        // Move player and weapon
        Move();

        Kill();
    }
    
    void Update() {
        UpdateHealthBar();
        TrackMovement();
        var hasShot = Input.GetMouseButtonDown(0);

        if (hasShot) PrintBullet();
    }

    protected override void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == ENEMY_BULLET_LAYER) {
            Debug.Log($"Hit! Taking {other.gameObject.GetComponent<EnemyBullet>().damage} damage!");
            health -= other.gameObject.GetComponent<EnemyBullet>().damage;
            Debug.Log($"Health now at {health}");
        } else {
            Debug.Log($"Hit by {other.gameObject.layer}");
        }
    }

    // -- Utility Methods --
    private void AssignInput() {
        moveDelta = Vector3.zero;
        moveDelta = new Vector3(xMovement, yMovement, 0);
    }
    
    protected override void Kill() {
        if (!IsAlive()) {
            Destroy(activeGun);
            Destroy(this);
        }
        
        base.Kill();
    }
    
    private void ModifyAnimation() {
        var animationName = "movingNorth";

        if (moveDelta.y > 0) {
            animator.SetBool(animationName, true);
            sideGun.SetActive(false);
            backGun.SetActive(true);
            forwardGun.SetActive(false);

            activeGun = backGun;
        } else if (moveDelta.y < 0) {
            animator.SetBool(animationName, false);
            sideGun.SetActive(false);
            backGun.SetActive(false);
            forwardGun.SetActive(true);

            activeGun = forwardGun;
        } else {
            animator.SetBool(animationName, false);
            sideGun.SetActive(true);
            backGun.SetActive(false);
            forwardGun.SetActive(false);

            activeGun = sideGun;
        }
    }
    
    protected override void Move() {
        rigidbody2D.MovePosition(transform.position + moveDelta * (speed * Time.fixedDeltaTime));
    }
    
    private void PrintBullet() {
        var prefab = Instantiate(bulletPrefab, position, Quaternion.identity);
        var mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        prefab.GetComponent<Bullet>().damage = damage;
        prefab.GetComponent<Bullet>().moveDirection = mousePosition;
    }
    
    private void Rotate() {
        if (moveDelta.x > 0) {
            transform.localScale = Vector3.one;
        } else if (moveDelta.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void TrackMovement() {
        // Track input
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");
    }
    
    private void UpdateHealthBar() {
        healthBar.value = health;
    }

    private void SetHealthBar() {
        healthBar.maxValue = health;
        healthBar.minValue = 0;
    }
}
