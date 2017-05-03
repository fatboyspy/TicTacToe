using System;

/// <summary>
/// Базовый класс игрока
/// </summary>
public class Player {
    /// <summary>
    /// Значение ячейки, которое вписывается в поле при совершении игроком хода
    /// </summary>
    String cellValue;
    /// <summary>
    /// Количество побед игрока
    /// </summary>
    int winsCount;
    /// <summary>
    /// Количество поражений иргока
    /// </summary>
    int losesCount;
    /// <summary>
    /// Тип игрока - Человек/АИ
    /// </summary>
    PlayerType type;

    #region Свойства для полей
    public String CellValue
    {
        get
        {
            return cellValue;
        }
        set
        {
            cellValue = value;
        }
    }

    public int WinsCount
    {
        get
        {
            return winsCount;
        }

        set
        {
            winsCount = value;
        }
    }

    public int LosesCount
    {
        get
        {
            return losesCount;
        }

        set
        {
            losesCount = value;
        }
    }

    public PlayerType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    } 
    #endregion

    /// <summary>
    /// Конструктор по-умолчанию
    /// </summary>
    public Player()
    {
        Type = PlayerType.Human;
        cellValue = "X";
        winsCount = 0;
        losesCount = 0;
    }
    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    /// <param name="playerType">Тип игрока</param>
    /// <param name="value">Значение для игровой клетки</param>
    public Player(PlayerType playerType, String value)
    {
        Type = playerType;
        cellValue = value;
        WinsCount = 0;
        LosesCount = 0;
    }
}

public enum PlayerType
{
    Human,
    AI
}

