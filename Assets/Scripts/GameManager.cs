using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public class LevelStat
    {
        public int LevelNr;
        public float Time;
        public int Score;
        public int ObjectsPushed;

        public LevelStat(float newTime, int newScore, int newLevel, int objects)
        {
            LevelNr = newLevel;
            Time = newTime;
            Score = newScore;
            ObjectsPushed = objects;
        }
    }
    public static GameManager Instance { get; private set; }
    public float bounds;
    public bool gameOver;
    public float time;
    public int pushedObjects;
    public int score;
    public int level;
    public GameObject[] objects;
    public enum ObjectType
    {
        Enemy,
        Collectible
    }

    private List<LevelStat> _levelStats;
    private ViewManager _viewManager;
    [SerializeField] private int maxLevel = 4;
    private const float MinSeconds = 4;
    private const float MaxSeconds = 12;
    [SerializeField] private float maxScorePerLevel = 100;

    private void Awake()
    {
        Instance = this;
        time = 0;
        level = 1;
        score = 0;
        pushedObjects = 0;
        gameOver = true;
        _levelStats = new List<LevelStat>();
        var square = GameObject.Find("Playground").gameObject;
        // adaptive value if square scale is changed during testing
        bounds = square.GetComponent<Transform>().localScale.x * 10 / 2;
    }

    private void Start()
    {
        _viewManager = ViewManager.Instance;
    }

    public void UpdateScore(int newScore)
    {
        score += newScore;
        CheckScore();
        _viewManager.UpdateLevelScore();
    }

    private void CheckScore()
    {
        if (score >= maxScorePerLevel * level)
        {
            PlayerWins();
        }
        else if (score < 0)
        {
            PlayerLoses();
        }
    }
    
    private void PlayerWins()
    {
        RefreshData();
        level++;
        if (level <= maxLevel) return;
        gameOver = true;
        StartCoroutine(_viewManager.ShowScreen(ViewManager.ScreenType.Win));
    }

    private void PlayerLoses()
    {
        gameOver = true;
        RefreshData();
        StartCoroutine(_viewManager.ShowScreen(ViewManager.ScreenType.Lose));
        SaveData();
    }

    private void Play()
    {
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

    public void StartOrRetry()
    {
        // Case win last level or lose
        if (level > maxLevel || score < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Case start first level
        gameOver = false;
        Play();
    }
    
    private void RefreshData()
    {
        var levelData = new LevelStat(time, score, level, pushedObjects);
        Debug.Log(levelData.ObjectsPushed);
        _levelStats.Add(levelData);
    }

    private void SaveData()
    {
        Debug.Log("Saving data");
        var gameData = _levelStats.ToArray();
        foreach (var datum in gameData)
        {
            Debug.Log(datum.Score);
            Debug.Log(datum.Time);
            Debug.Log(datum.ObjectsPushed);
        }
        var strOutput = JsonUtility.ToJson(gameData);
        
        File.WriteAllText(Application.dataPath + "/Resources/levelStats.json", strOutput);
    }
}
