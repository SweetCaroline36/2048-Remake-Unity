using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    public GameState GameState;
    private bool gameOver = false;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    private void Update()
    {
        if (Input.GetKeyDown("return"))
        {
            Reset();
        }
        if(Input.GetKeyDown("escape")) {
            QuitGame();
        }
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    public bool getGameOver()
    {
        return gameOver;
    }
    public void GameOver()
    {
        gameOver = true;
        //scoreCanvas.SetActive(false);
        IEnumerator end = WaitAndEnd(0.8f);
        StartCoroutine(end);
        //DiceManager.Instance.stopDragger();
    }

    private IEnumerator WaitAndEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        var canvasAnimator = gameOverCanvas.GetComponent<Animator>();
        gameOverCanvas.SetActive(true);
        canvasAnimator.Play("GameOverAppear", 0, 0);
    }

    public void Reset()
    {
        //Score.score = 0;
        gameOver = false;
        SceneManager.LoadScene(0);
        //Score.score = 0;
    }
    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.BoardInit();
                break;
            case GameState.SpawnBoxes:
                BoxManager.Instance.CreateTwoBoxes();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnBoxes = 1
}

