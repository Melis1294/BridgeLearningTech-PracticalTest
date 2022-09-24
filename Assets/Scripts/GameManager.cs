using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float bounds;
    public bool gameOver;
    public int score;
    public int level;
    public GameObject[] objects;
    public enum ObjectType
    {
        Enemy,
        Collectible
    }
    
    private ViewManager _viewManager;
    [SerializeField] private int maxLevel = 4;
    private const float MinSeconds = 4;
    private const float MaxSeconds = 12;
    [SerializeField] private float maxScorePerLevel = 100;

    private void Awake()
    {
        Instance = this;
        score = 0;
        var square = GameObject.Find("Playground").gameObject;
        // adaptive value if square scale is changed during testing
        bounds = square.GetComponent<Transform>().localScale.x * 10 / 2;
    }

    private void Start()
    {
        _viewManager = ViewManager.Instance;
        var newLevel = PlayerPrefs.GetInt("Level");
        if (newLevel > 1)
        {
            gameOver = false;
            level = newLevel;
            Play();
        }
        else
        {
            gameOver = true;
            level = 1;
            StartCoroutine(_viewManager.ShowScreen(ViewManager.ScreenType.Start, 0));
        }
    }

    public int GetMaxLevel()
    {
        return maxLevel;
    }

    public void UpdateScore(int newScore)
    {
        score += newScore;
        CheckScore();
        _viewManager.UpdateScore();
    }

    private void CheckScore()
    {
        if (score >= maxScorePerLevel)
        {
            gameOver = true;
            PlayerWins();
        }
        else if (score < 0)
        {
            gameOver = true;
            PlayerLoses();
        }
    }
    
    private void PlayerWins()
    {
        level++;
        if (level > maxLevel)
        {
            StartCoroutine(_viewManager.ShowScreen(ViewManager.ScreenType.Finish, null));
        }
        else
        {
            PlayerPrefs.SetInt("Level", level);
            StartCoroutine(_viewManager.ShowScreen(ViewManager.ScreenType.Win, null));
        }
    }

    private void PlayerLoses()
    {
        StartCoroutine(_viewManager.ShowScreen(ViewManager.ScreenType.Lose, null));
    }

    public void Play()
    {
        score = 0;
        _viewManager.RefreshUI();
        if (gameOver) return;
        SpawnNewObject(ObjectType.Collectible);
        StartCoroutine(ManageEnemySpawn());
    }

    private IEnumerator ManageEnemySpawn() {
        while(!gameOver) {
            yield return new WaitForSeconds(Random.Range(MinSeconds / level, MaxSeconds / level));
            SpawnNewObject(ObjectType.Enemy);
        }
    }
    
    public void SpawnNewObject(ObjectType obj)
    {
        var x = Random.Range(-bounds, bounds);
        var z = Random.Range(-bounds, bounds);
        Instantiate(objects[(int)obj], new Vector3(x, 0, z), objects[(int)obj].transform.rotation);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Level");
    }
}
