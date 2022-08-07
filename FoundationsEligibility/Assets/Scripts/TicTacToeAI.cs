﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum TicTacToeState
{
    None,
    Cross,
    Circle
}

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
}

public class TicTacToeAI : MonoBehaviour
{
    TicTacToeState[,] _boardState;

    //Call This event with the player number to denote the winner
    public WinnerEvent onPlayerWin;
    public UnityEvent onGameStarted;
    public bool isPlayerTurn;
    public int maxRounds = 9;
    public int currentRound = 1;

    [SerializeField] private TicTacToeState aiState = TicTacToeState.Cross;
    [SerializeField] private TicTacToeState playerState = TicTacToeState.Circle;
    [SerializeField] private GameObject xPrefab;
    [SerializeField] private GameObject oPrefab;
    [SerializeField] private GameObject[] grid;

    private int _aiLevel;
    private ClickTrigger[,] _triggers;
    public bool playerWin;
    public bool aiWin;

    private void Awake()
    {
        if (onPlayerWin == null)
        {
            onPlayerWin = new WinnerEvent();
        }
    }

    public void StartAI(int aiLevel)
    {
        _aiLevel = aiLevel;
        StartGame();
    }

    public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
    {
        _triggers[myCoordX, myCoordY] = clickTrigger;
    }

    private void StartGame()
    {
        _triggers = new ClickTrigger[3, 3];
        isPlayerTurn = true;
        onGameStarted.Invoke();
    }

    public void PlayerSelects(int coordX, int coordY)
    {
        SetVisual(coordX, coordY, playerState);
        isPlayerTurn = false;
        currentRound++;
        CheckForWin();
    }

    private void AiSelects(int coordX, int coordY)
    {
        if (playerWin) return;
        SetVisual(coordX, coordY, aiState);
        isPlayerTurn = true;
        currentRound++;
        CheckForWin();
    }

    private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
    {
        Instantiate(targetState == TicTacToeState.Circle ? oPrefab : xPrefab,
            _triggers[coordX, coordY].transform.position, Quaternion.identity);
    }

    public void CalculateMove()
    {
        if (_aiLevel == 0)
        {
            var foundEmptySpot = false;

            while (!foundEmptySpot)
            {
                var randomNumber = Random.Range(0, 9);
                var gridField = grid[randomNumber].GetComponent<ClickTrigger>();

                if (!gridField.canClick) continue;
                gridField.canClick = false;
                gridField.filledByAI = true;
                AiSelects(gridField.myCoordX, gridField.myCoordY);
                foundEmptySpot = true;
            }
        }
        else
        {
            // AI Hard Mode
        }
    }

    private void CheckForWin()
    {
        // Horizontal
        // First Row
        if (grid[0].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[1].GetComponent<ClickTrigger>().filledByPlayer && grid[2].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[0].GetComponent<ClickTrigger>().filledByAI && grid[1].GetComponent<ClickTrigger>().filledByAI &&
                 grid[2].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        // Middle Row
        if (grid[3].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[4].GetComponent<ClickTrigger>().filledByPlayer && grid[5].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[3].GetComponent<ClickTrigger>().filledByAI && grid[4].GetComponent<ClickTrigger>().filledByAI &&
                 grid[5].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        // Bottom Row
        if (grid[6].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[7].GetComponent<ClickTrigger>().filledByPlayer && grid[8].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[6].GetComponent<ClickTrigger>().filledByAI && grid[7].GetComponent<ClickTrigger>().filledByAI &&
                 grid[8].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        // Vertical
        // First Column
        if (grid[0].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[3].GetComponent<ClickTrigger>().filledByPlayer && grid[6].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[0].GetComponent<ClickTrigger>().filledByAI && grid[3].GetComponent<ClickTrigger>().filledByAI &&
                 grid[6].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        // Second Column
        if (grid[1].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[4].GetComponent<ClickTrigger>().filledByPlayer && grid[7].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[1].GetComponent<ClickTrigger>().filledByAI && grid[4].GetComponent<ClickTrigger>().filledByAI &&
                 grid[7].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        // Third Column
        if (grid[2].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[5].GetComponent<ClickTrigger>().filledByPlayer && grid[8].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[2].GetComponent<ClickTrigger>().filledByAI && grid[5].GetComponent<ClickTrigger>().filledByAI &&
                 grid[8].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        // Diagonal
        // Left to Right
        if (grid[0].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[4].GetComponent<ClickTrigger>().filledByPlayer && grid[8].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[0].GetComponent<ClickTrigger>().filledByAI && grid[4].GetComponent<ClickTrigger>().filledByAI &&
                 grid[8].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        // Right to Left
        if (grid[2].GetComponent<ClickTrigger>().filledByPlayer &&
            grid[4].GetComponent<ClickTrigger>().filledByPlayer && grid[6].GetComponent<ClickTrigger>().filledByPlayer)
        {
            onPlayerWin.Invoke(0);
            playerWin = true;
        }
        else if (grid[2].GetComponent<ClickTrigger>().filledByAI && grid[4].GetComponent<ClickTrigger>().filledByAI &&
                 grid[6].GetComponent<ClickTrigger>().filledByAI)
        {
            onPlayerWin.Invoke(1);
            aiWin = true;
        }

        else if (currentRound == maxRounds + 1 && !playerWin && !aiWin) onPlayerWin.Invoke(-1);
    }
}
