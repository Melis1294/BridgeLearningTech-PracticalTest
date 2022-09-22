using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ViewManager _viewManager;
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

    public static GameManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gameOver = true;
        score = 0;
        var square = GameObject.Find("Playground").gameObject;
        // adaptive value if square scale is changed during testing
        bounds = square.GetComponent<Transform>().localScale.x * 10 / 2;
    }

    private void Start()
    {
        _viewManager = ViewManager.Instance;
    }

    public void SpawnNewObject(ObjectType obj)
    {
        var x = Random.Range(-bounds, bounds);
        var z = Random.Range(-bounds, bounds);
        Instantiate(objects[(int)obj], new Vector3(x, 0, z), Quaternion.identity);
    }

    public void UpdateScore(int newScore)
    {
        score += newScore;
        _viewManager.UpdateScore();
    }

    public void Play()
    {
        gameOver = false;
        SpawnNewObject(ObjectType.Collectible);
        SpawnNewObject(ObjectType.Enemy);
    }
}
