using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.CompareTag("Enemy")) return;
        Debug.Log($"{other.transform.tag}");
        Destroy(gameObject);
    }
}
