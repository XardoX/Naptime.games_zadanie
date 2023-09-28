using System;
using System.Collections;
using UnityEngine;

public class ShootingObject : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private SpriteRenderer body, head;

    [SerializeField]
    private int health = 3;

    [SerializeField]
    private float shootingCooldown = 1f;

    private float shootingTime, 
        rotationTime;

    private bool isAlive = true;

    private int id = -1;

    private GameObject lastShotProjectile;

    public int Health => health;

    public int Id => id;

    public Action<ShootingObject> OnHit, OnDeath;

    public void SetId(int newId) => id = newId;

    public void ToggleGraphic(bool toggle)
    {
        body.enabled = toggle;
        head.enabled = toggle;
    }

    public void Respawn(Vector3 newPosition)
    {
        gameObject.SetActive(true);
        ToggleGraphic(true);
        transform.position = newPosition;
        isAlive = true;
    }

    void Update()
    {
        if(isAlive)
        {
            Rotate();
            Shoot();
        }
    }

    private void Rotate()
    {
        rotationTime -= Time.deltaTime;

        if(rotationTime <= 0f)
        {
            rotationTime = UnityEngine.Random.Range(0f, 1f);
            float angle = UnityEngine.Random.Range(0f, 360f);

            transform.Rotate(Vector3.forward, angle);
        }

    }

    private void Shoot()
    {
        shootingTime += Time.deltaTime;

        if(shootingTime >= shootingCooldown)
        {
            shootingTime = 0f;
            var newProjectile = ObjectPooler.GetPooledObject(1);
            newProjectile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            lastShotProjectile = newProjectile;
            newProjectile.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") && isAlive)
        {
            if (lastShotProjectile == collision.gameObject) return;
            collision.gameObject.SetActive(false);

            health--;
            isAlive = false;
            ToggleGraphic(false);
            if(health > 0)
            {
                OnHit?.Invoke(this);
            }
            else
            {
                OnDeath?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }
}
