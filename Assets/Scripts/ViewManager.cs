using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    public GameObject StartScreen;
    public GameObject GameScreen;
    public TextMeshProUGUI TitleText;
    public GameObject startButton;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LevelText;
    private GameManager _manager;
    private bool _firstTry;
    [SerializeField] private string startText = "Cylinder Game";
    [SerializeField] private string gameOverText = "Game Over";
    
    public static ViewManager Instance { get; private set; }
    private void Awake() => Instance = this;

    // Start is called before the first frame update
    private void Start()
    {
        _manager = GameManager.Instance;
        startButton.GetComponent<Button>().onClick.AddListener(StartOrRetry);
        _firstTry = true;
        RefreshUI();
    }

    private void RefreshUI()
    {
        StartScreen.SetActive(_manager.gameOver);
        GameScreen.SetActive(!_manager.gameOver);
        if (_manager.gameOver)
        {
            if (_firstTry)
            {
                _firstTry = false;
                TitleText.text = startText;
                startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
            }
            else
            {
                TitleText.text = gameOverText;
                startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
            }
        }
        else
        {
            ScoreText.text = "Score: " + _manager.score;
            LevelText.text = "Level " + _manager.level;
        }
    }

    public void UpdateScore()
    {
        ScoreText.text = "Score: " + _manager.score;
    }
    
    private void StartOrRetry()
    {
        GameManager.Instance.Play();
        RefreshUI();
    }
}
