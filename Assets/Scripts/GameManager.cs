using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int objectCount = 50;
    [SerializeField]
    private ShootingObject shootingObject;
    // Start is called before the first frame update
    void Start()
    {
        SpawnObjects(objectCount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var shootingObject = ObjectPooler.Instance.GetPooledObject(0);
            shootingObject.transform.position = GetFreeRandomPosition();
            shootingObject.SetActive(true);
        }
    }

    private Vector3 GetFreeRandomPosition()
    {

        Vector3 randomPosition = GetRandomPosition();
        int safeCount = 0;
        while (Physics2D.OverlapCircle(randomPosition, 0.5f) && safeCount < 10)
        {
            randomPosition = GetRandomPosition();
            safeCount++;
        }
        return randomPosition;

        Vector3 GetRandomPosition()
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        }
    }
}
