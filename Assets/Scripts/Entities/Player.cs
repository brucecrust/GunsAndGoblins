using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {

    // -- Constants --
    private const int ENEMY_BULLET_LAYER = 15;
    
    // -- Variables --
    public bool godMode = false;
    public float healTextInterpolationPeriod = 3;

    private bool displayedHealText = false;
    private float healTextTimer = 0.0f;
    private Vector3 moveDelta;
    private float xMovement;
    private float yMovement;
    
    // -- Components --
    private Animator animator;
    
    public GameObject activeGun;
    public GameObject backGun;
    public GameObject bulletPrefab;
    public GameObject forwardGun;
    public GameObject sideGun;
    
    // -- UI --
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI godModeText;
    public Slider healthBar;

    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
        
        foreach (Transform child in transform) {
            if (child.GetComponent<Animator>() != null) {
                animator = child.GetComponent<Animator>();
            }
        }

        AssessGodMode();
        SetHealthBar();
        UpdateHealthBar();
    }

    // Update is called once per frame
    protected override void FixedUpdate() {
        UpdatePosition();

        if (wasShot) WasShot();

        // Change Player animation direction
        ModifyAnimation();
        
        // Rotate player
        Rotate();

        // Move player and weapon
        Move();

        Kill();
    }
    
    void Update() {
        AssessGodMode();
        CalculateTime();
        
        if (Input.GetKeyUp("space")) canJump = true;
        TrackMovement();
        AssignInput();
        
        UpdateHealthBar();
        TrackMovement();
        var hasShot = Input.GetMouseButtonDown(0);

        if (hasShot) PrintBullet();
    }

    protected override void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == ENEMY_BULLET_LAYER) {
            if (!godMode) health -= other.gameObject.GetComponent<EnemyBullet>().damage;
        }
    }

    // -- Utility Methods --
    private void ActivateGodMode() {
        godModeText.gameObject.SetActive(true);
    }

    public void ActivateHealText() {
        Debug.Log("Heal!");
        healText.gameObject.SetActive(true);
        displayedHealText = true;
    }

    private void AssessGodMode() {
        if (godMode) ActivateGodMode();
        else DeactivateGodMode();
    }
    
    private void AssignInput() {
        moveDelta = Vector3.zero;
        moveDelta = new Vector3(xMovement, yMovement, 0);
    }

    protected override void CalculateTime() {
        bool interpolatedPeriodReached = healTextTimer >= healTextInterpolationPeriod;

        if (displayedHealText) healTextTimer += Time.deltaTime;
        
        base.CalculateTime();
        
        if (!interpolatedPeriodReached) return;

        healTextTimer -= healTextInterpolationPeriod;
        DeactivateHealText();
    }
    
    private void DeactivateGodMode() {
        godModeText.gameObject.SetActive(false);
    }

    private void DeactivateHealText() {
        healText.gameObject.SetActive(false);
        displayedHealText = false;
    }
    
    public void Heal() {
        health = initialHealth;
    }

    protected override void Kill() {
        if (!IsAlive()) {
            Destroy(activeGun);
            Destroy(this);
            deathText.gameObject.SetActive(true);
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

    private void SetHealthBar() {
        healthBar.maxValue = health;
        healthBar.minValue = 0;
    }
    
    private void TrackMovement() {
        // Track input
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");
    }

    private void UpdateHealthBar() {
        healthBar.value = health;
    }
}
