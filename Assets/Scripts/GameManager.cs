using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameplaySettings gameplaySettings;

    [SerializeField]
    private ShootingObject shootingObject;

    [SerializeField]
    private MenuUI menuUI;

    private Camera mainCamera;

    private List<GameObject> shootingObjects = new();

    private int selectedObjectsCount = 0;

    private void Awake()
    {
        mainCamera = Camera.main;
        menuUI.CreateMenuButtons(gameplaySettings);
        menuUI.ToggleUI(true);
        menuUI.OnStartButtonClicked += StartGame;
        menuUI.OnObjectsCountSelected += SetObjectsCount;
    }

    private void StartGame()
    {
        menuUI.ToggleUI(false);
        SpawnObjects(gameplaySettings.objectsCountSettings[selectedObjectsCount].count);
    }

    private void SetObjectsCount(int count) => selectedObjectsCount = count;

    private void SpawnObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var shootingObject = ObjectPooler.Instance.GetPooledObject(0);
            shootingObject.name = $"ShootingObject_{count}";
            shootingObject.transform.position = GetFreeRandomPosition();
            shootingObject.SetActive(true);
            shootingObjects.Add(shootingObject);
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
            if(safeCount >= 10)
            {
                mainCamera.orthographicSize = mainCamera.orthographicSize * 1.1f;
                safeCount = 0;
            }
        }
        return randomPosition;

        Vector3 GetRandomPosition()
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), mainCamera.farClipPlane / 2));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
