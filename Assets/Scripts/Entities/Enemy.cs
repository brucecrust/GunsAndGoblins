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
        MoveProjectiles();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.transform.CompareTag("Player")) {
            if (CanPrintProjectile()) PrintProjectile();
        }
    }

    // -- Utility Methods --
    private void PrintProjectile() {
        var prefab = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        spawnedProjectiles.Add(prefab);
        Destroy(prefab, deletePrefabTime);
    }

    private void MoveProjectiles() {
        foreach (var projectile in spawnedProjectiles) {
            if (projectile != null) {
                projectile.transform.position = Vector3.MoveTowards(
                    projectile.transform.position, player.transform.position, 
                    projectileSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private bool CanPrintProjectile() {
        return spawnedProjectiles.Count < maxProjectiles;
    }

    private void RemoveProjectiles() {
        if (spawnedProjectiles.Count > 0) spawnedProjectiles.RemoveAll(item => item == null);
    }
}
