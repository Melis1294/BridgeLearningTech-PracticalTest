using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class LevelStat
    {
        public int Level;
        public double Time;
        public int Score;
        public int Spheres;
        public int Capsules;

        public LevelStat(float newTime, int newScore, int newLevel, int spheres, int capsules)
        {
            Level = newLevel;
            Time = Math.Round(newTime, 2);
            Score = newScore;
            Spheres = spheres;
            Capsules = capsules;
        }
    }
    [Serializable]
    public class LevelStats
    {
        public LevelStat[] level;
    }

    public static GameManager Instance { get; private set; }
    public float bounds;
    public bool gameOver;
    public float time;
    public int spheres, capsules;
    public int score;
    public int level;
    public GameObject[] objects;
    public LevelStats levelStats = new LevelStats();
    public enum ObjectType
    {
        Enemy,
        Sphere,
        Capsule
    }
    private ViewManager _viewManager;
    [SerializeField] private int maxLevel = 4;
    private const float MinSeconds = 4;
    private const float MaxSeconds = 12;
    [SerializeField] private float maxScorePerLevel = 100;

    private void Awake()
    {
        Instance = this;
        level = 1;
        time = 0;
        score = 0;
        spheres = 0;
        capsules = 0;
        gameOver = true;
        levelStats.level = new LevelStat[maxLevel];
        var square = GameObject.Find("Playground").gameObject;
        // adaptive value if square scale is changed during testing
        bounds = square.GetComponent<Transform>().localScale.x * 10 / 2;
    }

    private void Start()
    {
        _viewManager = ViewManager.Instance;
    }

    public void UpdateScore(int newScore, ObjectType? obj)
    {
        switch (obj)
        {
            case ObjectType.Sphere:
                spheres++;
                break;
            case ObjectType.Capsule:
                capsules++;
                break;
            case ObjectType.Enemy:
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
        }
        
        if (newScore > 0)
            SpawnNewObject(GetRandomCollectible());
        score += newScore;
        CheckScore();
        _viewManager.UpdateLevelScore();
    }

    private void CheckScore()
    {
        if (score >= maxScorePerLevel * level)
            PlayerWins();
        else if (score < 0)
            PlayerLoses();
    }
    
    private void PlayerWins()
    {
        RefreshData();
        level++;
        if (level <= maxLevel) return;
        EndGame(ViewManager.ScreenType.Win);
    }

    private void PlayerLoses()
    {
        RefreshData();
        EndGame(ViewManager.ScreenType.Lose);
    }

    private void EndGame(ViewManager.ScreenType screen)
    {
        gameOver = true;
        SaveData();
        StartCoroutine(_viewManager.ShowScreen(screen));
    }

    private void Play()
    {
        _viewManager.RefreshUI();
        if (gameOver) return;
        SpawnNewObject(GetRandomCollectible());
        StartCoroutine(ManageEnemySpawn());
    }

    private static ObjectType GetRandomCollectible()
    {
        return Random.Range(0, 2) == 0 ? ObjectType.Sphere : ObjectType.Capsule;
    }

    private IEnumerator ManageEnemySpawn() {
        while(!gameOver) {
            yield return new WaitForSeconds(Random.Range(MinSeconds / level, MaxSeconds / level));
            SpawnNewObject(ObjectType.Enemy);
        }
    }

    private void SpawnNewObject(ObjectType obj)
    {
        var x = Random.Range(-bounds, bounds);
        var z = Random.Range(-bounds, bounds);
        var index = (int)obj;
        Instantiate(objects[index], new Vector3(x, 0, z), objects[index].transform.rotation);
    }

    public void StartOrRetry()
    {
        // Case win last level or lose
        if (level > maxLevel || score < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Case start game
        gameOver = false;
        Play();
    }
    
    private void RefreshData()
    {
        var levelData = new LevelStat(time, score, level, spheres, capsules);
        levelStats.level[level - 1] = levelData;
    }

    private void SaveData()
    {
        var jsonOutput = JsonConvert.SerializeObject(levelStats.level, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/levelStats.json", jsonOutput);
    }
}
