using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float lifeTime = 2f;

    private void Start() {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Add collision handling logic
        Destroy(gameObject);
    }
}
