using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObject : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private int health = 3;

    [SerializeField]
    private float shootingCooldown = 1f;

    private float shootingTime;

    private float rotationTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Shoot();
    }

    private void Rotate()
    {
        rotationTime -= Time.deltaTime;

        if(rotationTime <= 0f)
        {
            rotationTime = Random.Range(0f, 1f);
            float angle = Random.Range(0f, 360f);

            transform.Rotate(Vector3.forward, angle);
        }

    }

    private void Shoot()
    {
        shootingTime += Time.deltaTime;

        if(shootingTime >= shootingCooldown)
        {
            shootingTime = 0f;
            var newProjectile = ObjectPooler.Instance.GetPooledObject(1);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.rotation = transform.rotation;
            newProjectile.SetActive(true);
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);

            health--;
            gameObject.SetActive(false);
            if(health > 0)
            {
               // StartCoroutine(Respawn());
            }
        }
    }
}
