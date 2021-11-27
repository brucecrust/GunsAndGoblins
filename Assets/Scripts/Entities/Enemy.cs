using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC {
    
    // -- Variables --
    public float projectileSpeed;
    public float maxProjectiles;
    
    // -- Components --
    public GameObject projectilePrefab;

    private GameObject player;
    private List<GameObject> spawnedProjectiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        RemoveProjectiles();
        if (spawnedProjectiles.Count != 0 || spawnedProjectiles.Count < maxProjectiles) MoveProjectiles();
    }

    private void PrintProjectile() {
        var prefab = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        spawnedProjectiles.Add(prefab);
    }

    private void MoveProjectiles() {
        foreach (var projectile in spawnedProjectiles) {
            projectile.transform.position = Vector3.MoveTowards(position, player.transform.position, projectileSpeed);
        }
    }

    private void RemoveProjectiles() {
        foreach (var projectile in spawnedProjectiles) {
            if (projectile == null) spawnedProjectiles.Remove(projectile);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (!other.transform.CompareTag("Blocking")) return;
        PrintProjectile();
        MoveProjectiles();
    }
}
