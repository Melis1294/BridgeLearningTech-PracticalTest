using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject gameScreen;
    public TextMeshProUGUI titleText;
    public Button startButton;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public enum ScreenType
    {
        Start,
        Win,
        Lose,
        Finish
    }
    private GameManager _manager;
    private string title = "Game Over";
    [SerializeField] private float screenDuration = 3.0f;
    
    public static ViewManager Instance { get; private set; }
    private void Awake() => Instance = this;

    private void Start()
    {
        _manager = GameManager.Instance;
    }

    public void RefreshUI()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(StartOrRetry);
        _manager ??= GameManager.Instance;
        startScreen.SetActive(_manager.gameOver);
        gameScreen.SetActive(!_manager.gameOver);
        if (_manager.gameOver)
        {
            titleText.text = title;
        }
        else
        {
            scoreText.text = "Score: " + _manager.score;
            levelText.text = "Level " + _manager.level;
        }
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + _manager.score;
    }

    public IEnumerator ShowScreen(ScreenType screen, float? duration)
    {
        yield return new WaitForSeconds(duration ?? screenDuration);
        switch (screen)
        {
            // First try
            case ScreenType.Start:
                title = "Cylinder Game";
                buttonText.text = "Play";
                break;
            case ScreenType.Win:
                title = "Level Complete";
                buttonText.text = "Continue";
                break;
            case ScreenType.Lose:
                title = "Game Over";
                buttonText.text = "Retry";
                break;
            case ScreenType.Finish:
                title = "Game finished";
                buttonText.text = "Replay";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
        }
        RefreshUI();
    }
    
    private void StartOrRetry()
    {
        // Case win level
        if (_manager.level > 1) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Case win last level
        if (_manager.level > _manager.GetMaxLevel() || _manager.score < 0)
        {
            _manager.level = 1;
            PlayerPrefs.SetInt("Level", _manager.level);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Case start first level
        _manager.gameOver = false;
        _manager.Play();
    }
}
