using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Класс-скрипт для работы с представлением
/// </summary>
public class GameView : MonoBehaviour {
    [SerializeField]
    List<Button> buttons = new List<Button>(9);
    [SerializeField]
    GameObject messagePanel;
    [SerializeField]
    Button buttonX;
    [SerializeField]
    Button buttonO;
    GameController gc;
    [SerializeField]
    AudioClip clickSound;
    [SerializeField]
    AudioClip gameOverSound;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioSource mainSource;
    [SerializeField]
    Text deadHeat;

    [SerializeField]
    Text firstPlayerStats;
    [SerializeField]
    Text secondPlayerStats;

    public GameController Gc
    {
        get
        {
            return gc;
        }

        set
        {
            gc = value;
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameDifficulty diff = DataAdapter.GameData.difficulty != null ? DataAdapter.GameData.difficulty : GameDifficulty.Easy;
        PlayerType enemyType = DataAdapter.GameData.enemyType != null ?DataAdapter.GameData.enemyType: PlayerType.AI;
        Gc = DataAdapter.GameData.gameController != null ? DataAdapter.GameData.gameController : new GameController(diff, enemyType);
        Gc.SetDifficulty(diff);
        Gc.SetGameMode(enemyType);
        RedrawBoard(Gc.GetBoard());        
        ChangeCurrentColor();
    }

    /// <summary>
    /// Метод-обработчик нажатия на кнопку
    /// </summary>
    /// <param name="button">Нажимаемая кнопка</param>
    public void ButtonClick(Button button)
    {
        audioSource.PlayOneShot(clickSound);
        //дизейблим кнопку
        button.interactable = false;
        //меняем текст конкретной кнопки
        button.GetComponentInChildren<Text>().text = Gc.CurrentPlayer.CellValue;
        //делаем ход, при этом передавая контроллеру индекс нажатой кнопки
        Gc.TakeTurn(buttons.IndexOf(button));
        RedrawBoard(Gc.GetBoard());
    }

    public void NewGame()
    {
        audioSource.Stop();
        mainSource.Play();
        
        buttons.ForEach(b=>
        {
            b.GetComponentInChildren<Text>().text = "";
            b.interactable = true;
        });
        messagePanel.SetActive(false);
        Gc.NewGame();
        RedrawBoard(Gc.GetBoard());
    }

    private void DisableBoard()
    {
        buttons.ForEach(b=> { b.interactable = false; });
    }
    private void RedrawBoard(List<string> fields)
    {
        ChangeCurrentColor();
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = fields[i];
            if (!string.IsNullOrEmpty(fields[i]))
                buttons[i].interactable = false;
        }
        firstPlayerStats.text = string.Format("WIN:{0}\r\nLOSE:{1}",Gc.GetFirstPlayerStats()[0], Gc.GetFirstPlayerStats()[1]);
        secondPlayerStats.text = string.Format("WIN:{0}\r\nLOSE:{1}", Gc.GetSecondPlayerStats()[0], Gc.GetSecondPlayerStats()[1]);
        deadHeat.text = string.Format("DeadHeat:{0}", Gc.GetDeadHeat());

        switch (Gc.GetGameState())
        {
            case GameState.DeadHeat:
                mainSource.Stop();
                audioSource.PlayOneShot(gameOverSound);
                DisableBoard();
                messagePanel.GetComponentInChildren<Text>().text = "Ничья, победила дружба!";
                messagePanel.SetActive(true);
                break;
            case GameState.FirstPlayerWin:
                mainSource.Stop();
                audioSource.PlayOneShot(gameOverSound);
                DisableBoard();
                messagePanel.GetComponentInChildren<Text>().text = string.Format("Победил первый игрок, который играл \"{0}\"", Gc.CurrentPlayer.CellValue);
                messagePanel.SetActive(true);
                break;
            case GameState.SecondPlayerWin:
                mainSource.Stop();
                audioSource.PlayOneShot(gameOverSound);
                DisableBoard();
                messagePanel.GetComponentInChildren<Text>().text = string.Format("Победил второй игрок, который играл \"{0}\"", Gc.CurrentPlayer.CellValue);
                messagePanel.SetActive(true);
                break;
        }
    }
    private void ChangeCurrentColor()
    {
        if (Gc.CurrentPlayer.CellValue == buttonX.GetComponentInChildren<Text>().text)
        {
            ColorBlock colors = buttonX.colors;
            colors.normalColor = Color.green;
            buttonX.colors = colors;

            colors.normalColor = Color.white;
            buttonO.colors = colors;
        }
        else
        {
            ColorBlock colors = buttonX.colors;
            colors.normalColor = Color.white;
            buttonX.colors = colors;

            colors.normalColor = Color.green;
            buttonO.colors = colors;
        }
    }
    public void OpenMenu()
    {
        DataAdapter.GameData.gameController = Gc;
        SceneManager.LoadScene("menu");
    }

}
