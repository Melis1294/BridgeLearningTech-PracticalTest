using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
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
    private const float MinSeconds = 4;
    private const float MaxSeconds = 12;
    [SerializeField] private float maxScorePerLevel = 100;

    public static GameManager Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        gameOver = true;
        score = 0;
        level = 1;
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
        _viewManager.UpdateScore();
    }

    private void CheckScore()
    {
        if (score >= maxScorePerLevel)
        {
            gameOver = true;
            Debug.Log("You win!!");
        }
        else if (score < 0)
        {
            Debug.Log("You lose!!");
            gameOver = true;
        }
    }

    public void Play()
    {
        gameOver = false;
        SpawnNewObject(ObjectType.Collectible);
        StartCoroutine(ManageEnemySpawn());
    }
    
    private IEnumerator ManageEnemySpawn() {
        while(!gameOver) {
            SpawnNewObject(ObjectType.Enemy);
            var seconds = Random.Range(MinSeconds / level, MaxSeconds / level);
            yield return new WaitForSeconds(seconds);
        }
    }
    
    public void SpawnNewObject(ObjectType obj)
    {
        var x = Random.Range(-bounds, bounds);
        var z = Random.Range(-bounds, bounds);
        Instantiate(objects[(int)obj], new Vector3(x, 0, z), objects[(int)obj].transform.rotation);
    }
}
