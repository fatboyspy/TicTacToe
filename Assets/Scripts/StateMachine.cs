using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StateMachine
{
    /// <summary>
    /// Получение текущего состояния игры
    /// </summary>
    /// <returns></returns>
    GameState GetState();

    void ToggleState();
}
/// <summary>
/// Текущее состояние игры
/// </summary>
public enum GameState
{
    /// <summary>
    /// Ходит первый игрок
    /// </summary>
    FirstPlayerTurn,
    /// <summary>
    /// Ходит второй игрок
    /// </summary>
    SecondPlayerTurn,
    /// <summary>
    /// Игра окончена
    /// </summary>
    DeadHeat,
    FirstPlayerWin,
    SecondPlayerWin
}