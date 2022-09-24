using System;
using System.Collections;
using TMPro;
using UnityEngine;
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
        Win,
        Lose
    }
    private GameManager _manager;
    [SerializeField] private float screenDuration = 3.0f;
    
    public static ViewManager Instance { get; private set; }
    private void Awake() => Instance = this;

    private void Start()
    {
        _manager = GameManager.Instance;
        startButton.onClick.AddListener(StartOrRetry);
    }

    public void RefreshUI()
    {
        _manager ??= GameManager.Instance;
        startScreen.SetActive(_manager.gameOver);
        gameScreen.SetActive(!_manager.gameOver);
    }

    private void StartOrRetry()
    {
        _manager.StartOrRetry();
    }

    public void UpdateLevelScore()
    {
        scoreText.text = "Score: " + _manager.score;
        levelText.text = "Level: " + _manager.level;
    }

    public IEnumerator ShowScreen(ScreenType screen)
    {
        if (_manager.gameOver) yield return new WaitForSeconds(screenDuration);
        else yield return new WaitForSeconds(0);
        switch (screen)
        {
            // First try
            case ScreenType.Lose:
                titleText.text = "Game Over";
                buttonText.text = "Retry";
                break;
            case ScreenType.Win:
                titleText.text = "Winner";
                buttonText.text = "Replay";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
        }
        RefreshUI();
    }
}
