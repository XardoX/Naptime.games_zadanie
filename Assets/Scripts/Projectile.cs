using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 50f;

    private void OnEnable()
    {
        
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
