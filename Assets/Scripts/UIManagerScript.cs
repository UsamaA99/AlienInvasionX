using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text _scoretext;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Image _playerLivesDisplay;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartText;

    [SerializeField]
    private Sprite[] _LivesSprites;

    private GameManagerScript _gameManager;

    private int _currentScore;
    private int _bestScore;

    void Start()
    {
        _bestScore = PlayerPrefs.GetInt("_BestScore");
        _GameOverText.gameObject.SetActive(false);
        _RestartText.gameObject.SetActive(false);
        _scoretext.text = "Score: " + 0;
        _bestScoreText.text = "Best: " + _bestScore.ToString();
        _playerLivesDisplay.sprite = _LivesSprites[3];

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManagerScript>();
    }

    public void UpdateScore(int _playerScore)
    {
        _currentScore = _playerScore;
        _scoretext.text = "Score: " + _currentScore.ToString();
    }

    private void BestScoreCheck()
    {
        if (_currentScore > _bestScore)
        {
            _bestScoreText.text = "Best: " + _currentScore.ToString();
            PlayerPrefs.SetInt("_BestScore", _currentScore);
        }
    }

    public void UpdatePlayerLives(int _playerLives)
    {
        _playerLivesDisplay.sprite = _LivesSprites[_playerLives];

        if (_playerLives == 0)
        {
            BestScoreCheck();
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _GameOverText.gameObject.SetActive(true);
        _RestartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _GameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _GameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
