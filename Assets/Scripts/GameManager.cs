using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameplaySettings gameplaySettings;

    [SerializeField]
    private ShootingObject shootingObject;

    [SerializeField]
    private MenuUI menuUI;

    [Header("Debug")]
    [SerializeField][Range(0f, 10f)]
    private float timeScale = 1f;

    private Camera mainCamera;

    private List<ShootingObject> shootingObjects = new();

    private int selectedObjectsCount = 0;

    public static Vector3 GetFreeRandomPosition()
    {
        var camera = Camera.main;
        Vector3 randomPosition = GetRandomPosition();
        int safeCount = 0;
        while (Physics2D.OverlapCircle(randomPosition, 0.5f) && safeCount < 15)
        {
            randomPosition = GetRandomPosition();
            
            safeCount++;
        }
        return randomPosition;

        Vector3 GetRandomPosition()
        {
            var width = Screen.width * Instance.gameplaySettings.screenFillPercent;
            var height = Screen.height * Instance.gameplaySettings.screenFillPercent;
            return camera.ScreenToWorldPoint(new Vector3(Random.Range(Screen.width - width, width), 
                   Random.Range(Screen.height - height, height), camera.farClipPlane / 2));
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        mainCamera = Camera.main;
        menuUI.CreateMenuButtons(gameplaySettings);
        menuUI.ToggleMenuUI(true);
        menuUI.OnStartButtonClicked += StartGame;
        menuUI.OnObjectsCountSelected += SetObjectsCount;
    }

    private void StartGame()
    {
        menuUI.ToggleMenuUI(false);
        SpawnObjects(gameplaySettings.objectsCountSettings[selectedObjectsCount].count);
        mainCamera.orthographicSize = gameplaySettings.objectsCountSettings[selectedObjectsCount].cameraSize;
    }

    private void SetObjectsCount(int count) => selectedObjectsCount = count;

    private void SpawnObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var shootingObject = ObjectPooler.Instance.GetPooledObject(0).GetComponent<ShootingObject>();
            shootingObject.name = $"ShootingObject_{i}";
            shootingObject.transform.position = GetFreeRandomPosition();
            shootingObject.gameObject.SetActive(true);
            shootingObject.SetId(i);
            shootingObject.OnDeath += RemoveObject;
            shootingObjects.Add(shootingObject);
        }
    }

    private void RemoveObject(ShootingObject deadObject)
    {
        shootingObjects.Remove(deadObject);
        if(shootingObjects.Count <= 1)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        foreach(var shootingObject in shootingObjects)
        {
            shootingObject.enabled = false;
        }

        menuUI.ToggleEndGameUI(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Time.timeScale = timeScale;  
    }
#endif
}
