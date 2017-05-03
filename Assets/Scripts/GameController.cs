using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс-скрипт для игрового контроллера
/// </summary>
public class GameController {
    Player currentPlayer;
    GameModel gm;
    public Player CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }

        set
        {
            currentPlayer = value;
        }
    }

    public GameController(GameDifficulty difficulty,PlayerType enemyType)
    {
        gm = new GameModel( new Player(PlayerType.Human, "X"), new Player(enemyType, "O"),difficulty);
        currentPlayer = gm.CurrentPlayer;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TakeTurn(int buttonIndex)
    {
        gm.TakeTurn(buttonIndex);
        currentPlayer= gm.CurrentPlayer;
    }
    public GameState GetGameState()
    {
        return gm.GetState();
    }
    public void NewGame()
    {
        gm.NewGame();
        gm.ToggleState();
        if (currentPlayer.Type == PlayerType.AI)
            gm.AITurn();
        currentPlayer = gm.CurrentPlayer;
    }
    public List<string> GetBoard()
    {
        return gm.gameBoard;
    }

    public int[] GetFirstPlayerStats()
    {
        return new int[] { gm.FirstPlayer.WinsCount, gm.FirstPlayer.LosesCount };
    }
    public int[] GetSecondPlayerStats()
    {
        return new int[] { gm.SecondPlayer.WinsCount, gm.SecondPlayer.LosesCount };
    }
    public int GetDeadHeat()
    {
        return gm.Deadheat;
    }
    public void SetDifficulty(GameDifficulty difficulty)
    {
        gm.ChangeDifficulty(difficulty);

    }
    public void SetGameMode(PlayerType enemyType)
    {
        gm.SecondPlayer.Type = enemyType;
    }
}
