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
    public GameState GameState;

    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private bool gameOver = false;


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
    }

    public bool getGameOver()
    {
        return gameOver;
    }
    public void GameOver()
    {
        gameOver = true;
        scoreCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        //DiceManager.Instance.stopDragger();
    }

    public void Reset()
    {
        //Score.score = 0;
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
            case GameState.Play:
                //snapController.SetActive(true);
                //sc.checkGameOver();
                break;
            case GameState.GameOver:
                GameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnBoxes = 1,
    Play = 2,
    GameOver = 3
}

