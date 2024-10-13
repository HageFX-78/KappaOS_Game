using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public Vector2 bulletDirection = Vector2.up;

    // Update is called once per frame
    void Update()
    {
        if (bulletDirection != Vector2.zero)
            transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bounds"))
        {
            ObjectPoolManager.instance.DespawnBullet(gameObject);
        }
    }
}
