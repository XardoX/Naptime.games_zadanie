using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 50f;

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
