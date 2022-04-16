using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private bool _isGameOver;
    private bool _isGameWon;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    private Animator _anim;


    private void Start()
    {
        _anim = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseGame()
    {
        _pauseMenuPanel.SetActive(true);
        _anim.SetBool("_isPaused", true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
